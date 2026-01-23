using IdentityService.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Common;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
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
