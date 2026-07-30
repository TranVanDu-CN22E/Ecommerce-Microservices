using Grpc.Core;
using MediatR;
using IdentityShared.Protos;
using IdentityShared.Extensions;
using IdentityService.Application.Features.User.Queries.GetUserById;
using Google.Protobuf.WellKnownTypes;

namespace IdentityService.Api.Services
{
    public class UserGrpc : UserGrpcService.UserGrpcServiceBase   
    {
        private readonly IMediator _mediator;

        public UserGrpc(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<GetUserByIdResponse> GetUserById(
            GetUserByIdRequest request,
            ServerCallContext context)
        {
            // Validate input (tương tự REST Controller)
            if (string.IsNullOrEmpty(request.UserId))
            {
                return new GetUserByIdResponse
                {
                    Success = false,
                    Errors = { "User ID is required." }
                };
            }

            // Dispatch query qua MediatR (giống hệt REST)
            var query = new GetUserByIdQuery(request.UserId);
            var result = await _mediator.Send(query);

            // Handle failure
            if (result.IsFailure)
            {
                return new GetUserByIdResponse
                {
                    Success = false,
                    Errors = { result.Errors.Select(e => e.Message).ToArray() }
                };
            }
            // Map Application Response -> Shared Model
            var sharedModel = result.Value.ToSharedModel();
            // Map Shared Model -> Proto
            var protoUser = sharedModel.ToProto();
            // Map Application DTO -> Proto DTO
            return new GetUserByIdResponse
            {
                Success = true,
                User = protoUser
            };
        }
    }
}