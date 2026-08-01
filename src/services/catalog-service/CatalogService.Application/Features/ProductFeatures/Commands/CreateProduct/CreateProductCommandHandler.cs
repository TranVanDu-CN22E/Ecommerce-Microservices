using AutoMapper;
using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Application.Common;
using CatalogService.Application.Interfaces.Storage;
using CatalogService.Domain.Aggregates.CategoryAggregate;
using CatalogService.Domain.Aggregates.ProductAggregate;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Application.Features.ProductFeatures.Commands.CreateProduct
{
    public sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Result<Guid>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILocalFileStorage _localFileStorage;
        private readonly IUnitOfWork _unitOfWork;
        public CreateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper, ILocalFileStorage localFileStorage, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _localFileStorage = localFileStorage;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {

            // Check if category exists
            var category = await _categoryRepository.GetCategoryByIdAsync(new CategoryId(Guid.Parse(request.CategoryId)), cancellationToken);
            if (category == null)
            {
                return Result<Guid>.Failure(new[] { new Error("Product.CategoryNotFound", "Category with ID {request.CategoryId} does not exist.") });
            }

            // Create a new Product instance using the domain model
            Product product;
            try
            {
                product = Product.Create(
                    productName: new ProductName(request.Name),
                    productSlug: new ProductSlug(request.Slug),
                    sellerId: Guid.Parse(request.SellerId),
                    description: request.Description,
                    categoryId: category.Id,
                    thumbnailUrl: string.Empty // Tạm thời để empty, sẽ update sau khi có Id
                );
            }
            catch (ArgumentException ex) when (ex.Message.Contains("ProductName") || ex.Message.Contains("ProductSlug"))
            {
                return Result<Guid>.Failure(new[] { new Error("Product.ValidationError", ex.Message) });
            }

            // Upload thumbnail với cấu trúc thư mục thumbnails/{productId}/
            string thumbnailUrl = string.Empty;
            if (request.Thumbnail != null && request.Thumbnail.Length > 0)
            {
                try
                {
                    var thumbnailSubPath = Path.Combine("thumbnails", product.Id.Value.ToString());
                    thumbnailUrl = await _localFileStorage.StoreImageAsync(request.Thumbnail, thumbnailSubPath);
                }
                catch (ArgumentException ex)
                {
                    return Result<Guid>.Failure(new[] { new Error("Product.InvalidThumbnail", ex.Message) });
                }
            }

            // Update thumbnail URL nếu có
            if (!string.IsNullOrEmpty(thumbnailUrl))
            {
                product.UpdateThumbnail(thumbnailUrl);
            }

            // Add additional images (handle file uploads)
            if (request.Images != null && request.Images.Any())
            {
                var imageUrls = new List<string>();
                foreach (var image in request.Images)
                {
                    var imageUrl = await _localFileStorage.StoreImageAsync(image, "products");
                    imageUrls.Add(imageUrl);
                }
                product.AddImages(imageUrls);
            }

            // Add variants (if any)
            if (request.Variants != null && request.Variants.Any())
            {
                foreach (var variantDto in request.Variants)
                {
                    string variantImageUrl = string.Empty;

                    // Xử lý upload ảnh variant nếu có
                    if (variantDto.Image != null && variantDto.Image.Length > 0)
                    {
                        try
                        {
                            var variantSubPath = Path.Combine("variants", product.Id.Value.ToString());
                            variantImageUrl = await _localFileStorage.StoreImageAsync(variantDto.Image, variantSubPath);
                        }
                        catch (ArgumentException ex)
                        {
                            return Result<Guid>.Failure(new[] { new Error("Product.InvalidVariantImage", ex.Message) });
                        }
                    }
                    ProductVariant variant;
                    try
                    {
                        /*variant = product.AddVariant(
                            sku: new ProductSku(variantDto.Sku),
                            price: Money.Create(variantDto.Price, "VND"),
                            originalPrice: variantDto.OriginalPrice.HasValue
                                ? Money.Create(variantDto.OriginalPrice.Value, "VND")
                                : null,
                            attributes: _mapper.Map<List<VariantAttribute>>(variantDto.Attributes),
                            imageUrl: variantImageUrl
                        );*/
                        var attributes = new List<VariantAttribute>();

                        foreach (var attr in variantDto.Attributes)
                        {
                            attributes.Add(new VariantAttribute(name: attr.Name, value: attr.Value, stockQuantity: attr.StockQuantity));
                        }
                        variant = product.AddVariant(
                            sku: new ProductSku(variantDto.Sku),
                            price: Money.Create(variantDto.Price, "VND"),
                            originalPrice: variantDto.OriginalPrice.HasValue
                                ? Money.Create(variantDto.OriginalPrice.Value, "VND")
                                : null,
                            attributes: attributes,
                            imageUrl: variantImageUrl
                            );
                    }
                    catch (InvalidOperationException ex)
                    {
                        return Result<Guid>.Failure(new[] { new Error("Product.VariantError", ex.Message) });
                    }
                }
            }

            await _productRepository.AddProductAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(product.Id.Value);

        }
    }
}