using CatalogService.Application.Common.Mappings;
using CatalogService.Application.Features.ProductFeatures.Queries.GetProductById;
using CatalogShared.Extensions;
using CatalogShared.Protos;
using Grpc.Core;
using MediatR;

namespace CatalogService.Api.Services
{
    public class ProductGrpc : ProductGrpcService.ProductGrpcServiceBase
    {
        private readonly Mediator _mediator;
        public ProductGrpc(Mediator mediator)
        {
            _mediator = mediator;
        }
        public override async Task<GetProductByIdResponse> GetProductById(GetProductByIdRequest request, ServerCallContext context)
        {
            // Validate input
            if (string.IsNullOrEmpty(request.ProductId))
            {
                return new GetProductByIdResponse
                {
                    Success = false,
                    Errors = { "ProductId is required." }
                };
            }
            // Dispatch query via MediatR
            var query = new GetProductByIdQuery { ProductId = request.ProductId };
            var result = await _mediator.Send(query);
            // Handle failure
            if (result.IsFailure)
            {
                return new GetProductByIdResponse
                {
                    Success = false,
                    Errors = { string.Join(", ", result.Errors.Select(e => e.Message)) }
                };
            }
            // Map Application Response -> Shared Model
            var sharedModel = result.Value.ToSharedModel();
            // Map Shared Model -> Proto
            var protoProduct = sharedModel.ToProto();
            return new GetProductByIdResponse
            {
                Product = protoProduct,
                Success = true
            };
        }
    }
}
