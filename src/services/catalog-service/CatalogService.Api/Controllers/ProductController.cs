using AuthenticationShared.Extensions;
using CatalogService.Application.Features.ProductFeatures.Commands.CreateProduct;
using CatalogService.Application.Features.ProductFeatures.Commands.UpdateProduct;
using CatalogService.Application.Features.ProductFeatures.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] CreateProductCommand cmd)
        {
            var userId = User.GetUserIdString();
            if (userId != null)
            {
                cmd.SellerId = userId;
                var result = await _mediator.Send(cmd);
                if (result.IsFailure)
                    return BadRequest(result.Errors);
                // Trả về mã 200 OK kèm dữ liệu sản phẩm vừa tạo
                return Ok(result.Value);
            }
            else
            {
                return Unauthorized("User ID is null.");
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] UpdateProductCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            if (result.IsFailure)
                return BadRequest(result.Errors);
            return Ok(result.Value);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(GetProductByIdQuery cmd)
        {
            var result = await _mediator.Send(cmd);
            if (result.IsFailure)
                return BadRequest(result.Errors);
            return Ok(result.Value);
        }

        [HttpGet("hehehe")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var userId = User.GetUserIdString();
            var roles = User.GetRoles();

            return Ok(new { UserId = userId, Roles = roles });
        }
    }
}
