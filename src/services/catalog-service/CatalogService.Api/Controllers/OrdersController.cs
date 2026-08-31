// CatalogService.Api/Controllers/OrdersController.cs
using AuthenticationShared.Extensions;
using CatalogService.Api.Models.Requests;
using CatalogService.Application.Features.OrderFeatures.Commands.PlaceOrder;
using CatalogShared.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // Bắt buộc authenticated
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// User đặt hàng → Reserve stock + Push outbox → Kafka async
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PlaceOrder(
        [FromBody] PlaceOrderRequest request,  // API model riêng biệt
        CancellationToken ct)
    {
        var userId = User.GetUserIdString();
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { Error = "User not authenticated" });

        // Map Request → Command
        var command = new PlaceOrderCommand(new OrderPlaceEvent
        {
            OrderId = request.OrderId,       // FE-generated idempotency key
            CustomerId = userId,                   // Server-side injection, client KHÔNG thể override
            PaymentMethod = request.PaymentMethod,
            ShippingAddress = request.ShippingAddress,
            ShippingFee = request.ShippingFee,
            Note = request.Note,
            OrderItems = request.OrderItems,
            OccurredOnUtc = DateTime.UtcNow        // Server timestamp, không trust client
        });

        var result = await _mediator.Send(command, ct);

        return result.IsSuccess
            ? Ok(new { OrderId = result.Value })   // Consistent response shape
            : BadRequest(new { Errors = result.Errors });
    }
}