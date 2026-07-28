using AutoMapper;
using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Application.Common;
using CatalogService.Application.Interfaces.Storage;
using CatalogService.Domain.Aggregates.CategoryAggregate;
using CatalogService.Domain.Aggregates.ProductAggregate;
using CatalogService.Domain.Interfaces;
using MediatR;

namespace CatalogService.Application.Features.ProductFeatures.Commands.UpdateProduct
{
    public sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, Result<bool>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILocalFileStorage _localFileStorage;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper, ILocalFileStorage localFileStorage, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _localFileStorage = localFileStorage;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<bool>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            // Kiểm tra xem category có tồn tại không
            var category = await _categoryRepository.GetCategoryByIdAsync(CategoryId.Create(Guid.Parse(request.CategoryId)), cancellationToken);
            if (category == null)
            {
                return Result<bool>.Failure(new[] { new Error("Loại sản phẩm không tìm thấy", "Loại sản phẩm với ID {request.CategoryId} không tồn tại.") });
            }

            // Lấy sản phẩm từ cơ sở dữ liệu
            var product = await _productRepository.GetProductByIdAsync(ProductId.Create(Guid.Parse(request.Id)), cancellationToken);
            if (product == null)
            {
                return Result<bool>.Failure(new[] { new Error("Sản phẩm không tìm thấy", "Sản phẩm với ID {request.Id} không tồn tại.") });
            }
            // Cập nhật thông tin sản phẩm
            product.Update(
                productName: ProductName.Create(request.Name),
                productSlug: ProductSlug.Create(request.Slug),
                description: request.Description,
                categoryId: category.Id,
                thumbnailUrl: product.ThumbnailUrl
            );

            // Xử lý ảnh đại diện (thumbnail)
            if (request.Thumbnail != null && !string.IsNullOrEmpty(request.Thumbnail.FileName))
            {
                try
                {
                    var thumbnailSubPath = Path.Combine("thumbnails", product.Id.Value.ToString());
                    await _localFileStorage.StoreImageAsync(request.Thumbnail, thumbnailSubPath);
                    product.UpdateThumbnail(await _productRepository.GetThumbnailUrlAsync(product.Id, cancellationToken));
                }
                catch (ArgumentException ex)
                {
                    return Result<bool>.Failure(new[] { new Error("Ảnh đại diện không hợp lệ", ex.Message) });
                }
            }
            

            // Xử lý ảnh phụ
            if (request.Variants != null && request.Variants.Any())
            {
                var existingVariants = await _productRepository.GetVariantsAsync(product.Id, cancellationToken);

                foreach (var variantDto in request.Variants)
                {
                    string variantImageUrl = string.Empty;

                    // Xử lý upload ảnh biến thể nếu có
                    if (!string.IsNullOrEmpty(variantDto.Image?.FileName))
                    {
                        try
                        {
                            var variantSubPath = Path.Combine("variants", product.Id.Value.ToString());
                            variantImageUrl = await _localFileStorage.StoreImageAsync(variantDto.Image, variantSubPath);
                        }
                        catch (ArgumentException ex)
                        {
                            return Result<bool>.Failure(new[] { new Error("Ảnh biến thể không hợp lệ", ex.Message) });
                        }
                    }

                    ProductVariant variant;
                    try
                    {
                        if (!existingVariants.Any(v => v.ProductSku == ProductSku.Create(variantDto.Sku)))
                        {
                            variant = product.AddVariant(
                                sku: new ProductSku(variantDto.Sku),
                                price: Money.Create(variantDto.Price, "VND"),
                                originalPrice: variantDto.OriginalPrice.HasValue
                                    ? Money.Create(variantDto.OriginalPrice.Value, "VND")
                                    : null,
                                attributes: _mapper.Map<List<VariantAttribute>>(variantDto.Attributes),
                                imageUrl: variantImageUrl
                            );
                        }
                        else
                        {
                            var existingVariant = existingVariants.First(v => v.ProductSku == ProductSku.Create(variantDto.Sku));
                            if (variantDto.Image != null)
                            {
                                await _localFileStorage.DeleteFileAsync(existingVariant.ImageUrl, cancellationToken);
                                product.RemoveImage(existingVariant.ImageUrl);
                            }
                            existingVariant.Update(
                                newPrice: Money.Create(variantDto.Price, "VND"),
                                newOriginalPrice: variantDto.OriginalPrice.HasValue
                                    ? Money.Create(variantDto.OriginalPrice.Value, "VND")
                                    : null,
                                variantAttributes: _mapper.Map<List<VariantAttribute>>(variantDto.Attributes),
                                imageUrl: await _localFileStorage.StoreImageAsync(variantDto.Image, "variants")
                            );
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        return Result<bool>.Failure(new[] { new Error("Lỗi biến thể", ex.Message) });
                    }
                }

                // Xóa các ảnh biến thể không còn tồn tại
                foreach (var existingVariant in existingVariants.ToList())
                {
                    if (!request.Variants.Any(v => ProductSku.Create(v.Sku) == existingVariant.ProductSku))
                    {
                        await _localFileStorage.DeleteFileAsync(existingVariant.ImageUrl, cancellationToken);
                        product.RemoveVariant(existingVariant.ProductVariantId);
                    }
                }
            }

            // Lưu thay đổi
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
