using MeetUp.EShop.Core.Models.Product;

namespace MeetUp.EShop.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Guid> Add(Product product);
        Task<bool> Delete(Guid guid);
        Task<bool> Update(Product product);
        Product? GetProduct(Guid guid);
        IEnumerable<Product> GetProducts();
    }
}