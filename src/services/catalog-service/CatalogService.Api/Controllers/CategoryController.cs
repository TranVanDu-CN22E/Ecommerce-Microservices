using CatalogService.Api.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CatalogService.Application.Features.CatalogFeatures.Commands.CreateCategory;
using CatalogService.Application.Features.CatalogFeatures.Commands.UpdateCategory;
using CatalogService.Application.Features.CatalogFeatures.Commands.DeleteCategory;

namespace CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateCategoryCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            if (result.IsFailure)
                return BadRequest(result.Errors);

            return CreatedAtAction("Success", result.Value);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            return HandleResult(result);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteCategoryCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            return HandleResult(result);
        }
    }
}
