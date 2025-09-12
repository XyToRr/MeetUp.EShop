using MeetUp.EShop.Core.Models.Order;

namespace MeetUp.EShop.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<Guid?> AddOrder(Order order);
        Task<bool> DeleteOrder(Guid guid);
        Order? Get(Guid guid);
        IEnumerable<Order> GetOrders();
        Task<bool> UpdateOrder(Order order);
    }
}