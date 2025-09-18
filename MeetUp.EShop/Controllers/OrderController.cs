using System.Net;
using DocumentFormat.OpenXml.ExtendedProperties;
using MeetUp.EShop.Api.Cache;
using MeetUp.EShop.Api.Exceptions;
using MeetUp.EShop.Business.Cache.Interfaces;
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
        private readonly ICacheService _cacheService;

        public OrderController(OrderService orderService, ICacheService cache)
        {
            _orderService = orderService;
            _cacheService = cache;
        }

        [HttpGet("getOrders")]
        public async Task<IResult> GetOrders()
        {
            var ordersCache = await _cacheService.GetCacheAsync<IEnumerable<Order>>(CacheKeys.Orders);
            if(ordersCache != null)
            {
                Log.Information("Retrieved orders from cache successfully");
                return Results.Ok(ordersCache);
            }

            var orders = _orderService.GetOrders();
            if (orders == null)
            {
                throw new ControllerException("Not found orders", HttpStatusCode.NotFound);
            }

            await _cacheService.SetCacheAsync(CacheKeys.Orders, orders);

            Log.Information("Retrieved {Count} orders successfully", orders.Count());
            return Results.Ok(orders);
        }

        [HttpGet("getOrder")]
        public async Task<IResult> GetOrder(Guid id)
        {
            var orderCache = await _cacheService.GetCacheAsync<Order>($"{CacheKeys.SingleOrder}+{id}");
            if (orderCache != null)
            {
                Log.Information("Retrieved order with ID {OrderId} from cache successfully", id);
                return Results.Ok(orderCache);
            }

            var order = _orderService.Get(id);
            if (order == null)
            {
                throw new ControllerException($"Not found order with id: {id}", HttpStatusCode.NotFound);
            }

            await _cacheService.SetCacheAsync($"{CacheKeys.SingleOrder}+{id}", order);
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

            var cacheKey = $"{CacheKeys.SingleOrder}+{result}";
            await _cacheService.SetCacheAsync(cacheKey, order);
            await _cacheService.SetCacheAsync(CacheKeys.Orders, _orderService.GetOrders().ToList());

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

            var cacheKey = $"{CacheKeys.SingleOrder}+{order.Id}";
            await _cacheService.SetCacheAsync(cacheKey, order);
            await _cacheService.SetCacheAsync(CacheKeys.Orders, _orderService.GetOrders().ToList());

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

            var cacheKey = $"{CacheKeys.SingleOrder}+{id}";
            await _cacheService.RemoveCacheAsync(cacheKey);
            await _cacheService.SetCacheAsync(CacheKeys.Orders, _orderService.GetOrders().ToList());

            Log.Information("Deleted order with ID {OrderId} successfully", id);
            return Results.Ok();
        }
    }
}
