using MF.OrderManagement.Application.Orders.DTOs;
using MF.OrderManagement.Application.Orders.UseCases.ApproveOrder;
using MF.OrderManagement.Application.Orders.UseCases.CreateOrder;
using MF.OrderManagement.Application.Orders.UseCases.GetOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MF.OrderManagement.Api.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController(
    CreateOrderUseCase create,
    GetOrdersUseCase get,
    ApproveOrderUseCase approve)
    : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateOrderResultDto>> Create([FromBody] CreateOrderRequest request, CancellationToken ct)
    {
        var result = await create.ExecuteAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { orderId = result.OrderId }, result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderListItemDto>>> GetAll(CancellationToken ct)
    {
        var list = await get.ExecuteAsync(ct);
        return Ok(list);
    }

    // Extra: GetById simples (útil para CreatedAtAction)
    [HttpGet("{orderId:guid}")]
    public async Task<ActionResult<OrderListItemDto>> GetById([FromRoute] Guid orderId, CancellationToken ct)
    {
        // para manter simples sem criar use case específico:
        var list = await get.ExecuteAsync(ct);
        var item = list.FirstOrDefault(x => x.OrderId == orderId);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpPut("{orderId:guid}/approve")]
    public async Task<IActionResult> Approve([FromRoute] Guid orderId, CancellationToken ct)
    {
        await approve.ExecuteAsync(orderId, ct);
        return NoContent();
    }
}