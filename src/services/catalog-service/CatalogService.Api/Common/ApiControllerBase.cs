using CatalogService.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new
            {
                errors = result.Errors.Select(e => new
                {
                    e.Code,
                    e.Message
                })
            });
        }

        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return BadRequest(new
            {
                errors = result.Errors.Select(e => new
                {
                    e.Code,
                    e.Message
                })
            });
        }
    }
}
