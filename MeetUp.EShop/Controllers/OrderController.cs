using System.Net;
using MeetUp.EShop.Api.Exceptions;
using MeetUp.EShop.Business.Services;
using MeetUp.EShop.Core.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MeetUp.EShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("getOrders")]
        public IResult GetOrders()
        {
            var orders = _orderService.GetOrders();
            if (orders == null)
            {
                throw new ControllerException("Not found orders", HttpStatusCode.NotFound);
            }

            Log.Information("Retrieved {Count} orders successfully", orders.Count());
            return Results.Ok(orders);
        }

        [HttpGet("getOrder")]
        public IResult GetOrder(Guid id)
        {
            var order = _orderService.Get(id);
            if (order == null)
            {
                throw new ControllerException($"Not found order with id: {id}", HttpStatusCode.NotFound);
            }

            Log.Information("Retrieved order with ID {OrderId} successfully", id);
            return Results.Ok(order);
        }

        [HttpPost("addOrder")]
        public async Task<IResult> AddOrder(Order order)
        {
            var result = await _orderService.AddOrder(order);
            if (result == Guid.Empty)
            {
                throw new ControllerException("Bad addOrder request", HttpStatusCode.BadRequest);
            }

            Log.Information("Added order with ID {OrderId} successfully", result);
            return Results.Ok(result);
        }

        [HttpPut("updateOrder")]
        public async Task<IResult> UpdateOrder(Order order)
        {
            var result = await _orderService.UpdateOrder(order);
            if ((bool)!result)
            {
                throw new ControllerException("Bad updateOrder request", HttpStatusCode.BadRequest);
            }

            Log.Information("Updated order with ID {OrderId} successfully", order.Id);
            return Results.Ok();
        }

        [HttpDelete("deleteOrder")]
        public async Task<IResult> DeleteOrder(Guid id)
        {
            var result = await _orderService.DeleteOrder(id);
            if ((bool)!result)
            {
                throw new ControllerException("Bad deleteOrder request", HttpStatusCode.BadRequest);
            }

            Log.Information("Deleted order with ID {OrderId} successfully", id);
            return Results.Ok();
        }
    }
}
