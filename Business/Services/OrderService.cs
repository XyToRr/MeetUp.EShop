using MeetUp.EShop.Core.Interfaces;
using MeetUp.EShop.Core.Models.Order;

namespace MeetUp.EShop.Business.Services
{
    public class OrderService(IOrderRepository orderRepository)
    {
        public async Task<Guid?> AddOrder(Order order)
        {
            if(!IsOrderValid(order)) return Guid.Empty;
            
            return await orderRepository.AddOrder(order);
        }

        private bool IsOrderValid(Order order)
        {
            if (order == null)
                return false;
            if (order.TotalPrice <= 0)
                return false;
            if (order.UserId == Guid.Empty)
                return false;
            if (order.Number <= 0)
                return false;
            if (order.CreatedAt > DateTime.Now)
                return false;

            return true;
        }

        public async Task<bool?> DeleteOrder(Guid id)
        {
            if(id == Guid.Empty) return false;

            return await orderRepository.DeleteOrder(id);
        }

        public Order? Get(Guid id)
        {
            if(id == Guid.Empty) return null;
            
            return orderRepository.Get(id);
        }

        public IEnumerable<Order> GetOrders()
        {
            return orderRepository.GetOrders();
        }

        public async Task<bool?> UpdateOrder(Order order)
        {
           if(!IsOrderValid(order)) return false;

            return await orderRepository.UpdateOrder(order);
        }
    }
}