using MeetUp.EShop.Core.Enums;
using MeetUp.EShop.Core.Interfaces;
using MeetUp.EShop.Core.Models.Order;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeetUp.EShop.Business.Reposirories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EShopDbContext _context;

        public OrderRepository(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<Guid?> AddOrder(Order order)
        {
            order.Id = Guid.NewGuid(); 
            _context.Orders.Add(order); 
            await _context.SaveChangesAsync(); 
            return order.Id;
        }

        public async Task<bool> DeleteOrder(Guid guid)
        {
            var order = _context.Orders.Include(o => o.Products).FirstOrDefault(o => o.Id == guid); 

            if (order == null)
            {
                return false; 
            }

            _context.Orders.Remove(order); 
            return (await _context.SaveChangesAsync()) > 0; 
        }

        public Order? Get(Guid guid)
        {
            return _context.Orders.Include(o => o.Products).FirstOrDefault(o => o.Id == guid); 
        }

        public IEnumerable<Order> GetOrders()
        {
            return _context.Orders.Include(o => o.Products);
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            var oldOrder = _context.Orders.FirstOrDefault(o => o.Id == order.Id);

            if (oldOrder == null)
            {
                return false;
            }
            oldOrder.Number = order.Number;
            oldOrder.TotalPrice = order.TotalPrice;
            oldOrder.Status = order.Status;
            oldOrder.CreatedAt = order.CreatedAt;
            oldOrder.UserId = order.UserId;

            _context.Orders.Update(oldOrder); 
            return await _context.SaveChangesAsync() > 0; 
        }
    }
}
