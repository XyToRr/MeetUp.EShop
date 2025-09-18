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
        private readonly IHybridCacheService _hybridCacheService;

        public OrderController(OrderService orderService, IHybridCacheService cache)
        {
            _orderService = orderService;
            _hybridCacheService = cache;
        }

        [HttpGet("getOrders")]
        public async Task<IResult> GetOrders()
        {
            var orders = await _hybridCacheService.GetCacheAsync(CacheKeys.Orders,
                async () => await Task.FromResult(_orderService.GetOrders()));

            Log.Information("Retrieved {Count} orders successfully", orders.Count());
            return Results.Ok(orders);
        }

        [HttpGet("getOrder")]
        public async Task<IResult> GetOrder(Guid id)
        {
            var order = await _hybridCacheService.GetCacheAsync($"{CacheKeys.SingleOrder}{id}",
                async () => await Task.FromResult(_orderService.Get(id)));
            
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

            var cacheKey = $"{CacheKeys.SingleOrder}{result}";
            await _hybridCacheService.SetCacheAsync(cacheKey, order);
            await _hybridCacheService.SetCacheAsync(CacheKeys.Orders, _orderService.GetOrders().ToList());

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

            var cacheKey = $"{CacheKeys.SingleOrder}{order.Id}";
            await _hybridCacheService.SetCacheAsync(cacheKey, order);
            await _hybridCacheService.SetCacheAsync(CacheKeys.Orders, _orderService.GetOrders().ToList());

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

            var cacheKey = $"{CacheKeys.SingleOrder}{id}";
            await _hybridCacheService.RemoveCacheAsync(cacheKey);
            await _hybridCacheService.SetCacheAsync(CacheKeys.Orders, _orderService.GetOrders().ToList());

            Log.Information("Deleted order with ID {OrderId} successfully", id);
            return Results.Ok();
        }
    }
}
