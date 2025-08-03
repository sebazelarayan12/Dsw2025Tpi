using Azure.Core;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrdersManagementService _service;

    public OrdersController(IOrdersManagementService service)
    {
        _service = service;
    }

    [HttpGet()]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllOrders([FromQuery] OrderModel.SearchOrder request)
    {
        var orders = await _service.GetAllOrders(request);
        if (orders == null || !orders.Any()) throw new NoContentException("Empty List");
        return Ok(orders);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> AddOrder([FromBody] OrderModel.RequestOrderModel request)
    {
        var orders = await _service.AddOrder(request);
        return CreatedAtAction(nameof(GetOrderById), new { id = orders.Id }, orders);
    }


    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var order = await _service.GetOrderById(id);
        return Ok(order);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] string newStatus)
    {
        var updatedOrder = await _service.UpdateOrderStatus(id, newStatus);
        if (updatedOrder == null) throw new EntityNotFoundException("Order not found");
        return Ok(updatedOrder);
    }
}

