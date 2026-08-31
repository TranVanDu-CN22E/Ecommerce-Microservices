using AuthenticationShared.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Features.OrderFeatures.Queries.GetOrderById;
using OrderService.Application.Features.OrderFeatures.Queries.GetOrderByUserId;

namespace OrderService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator mediator;
        public OrdersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrderById(GetOrderByIdQuery query)
        {
            var result = await mediator.Send(query);
            if (result == null)
                return NotFound();
            var userId = User.GetUserIdString();
            if (result.Value.CustomerId != userId)
                return Forbid();
            return Ok(result.Value);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrderByCustomerId(GetOrderByUserIdQuery query)
        {
            var userId = User.GetUserIdString();
            query.CustomerId = userId;
            var result = await mediator.Send(query);
            if (result == null)
                return NotFound();
            return Ok(result.Value);
        }
    }
}
