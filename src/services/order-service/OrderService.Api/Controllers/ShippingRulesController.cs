using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Features.ShippingRuleFeatures.Commands.CreateShippingRule;
using OrderService.Application.Features.ShippingRuleFeatures.Commands.UpdateShippingRule;
using OrderService.Application.Features.ShippingRuleFeatures.Queries.GetListShippingRule;

namespace OrderService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingRulesController : ControllerBase
    {
        private readonly IMediator mediator;
        public ShippingRulesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateShippingRule([FromBody] CreateShippingRuleCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListShippingRules(GetListShippingRuleQuery query)
        {
            var result = await mediator.Send(query ?? new GetListShippingRuleQuery());
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateShippingRule([FromBody] UpdateShippingRuleCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}
