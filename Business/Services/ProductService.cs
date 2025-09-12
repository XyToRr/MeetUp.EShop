using MeetUp.EShop.Core.Interfaces;
using MeetUp.EShop.Core.Models.Product;

namespace MeetUp.EShop.Business.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        private bool IsProductValid(Product product)
        {
            if (product == null) return false;
            if (string.IsNullOrWhiteSpace(product.Code)) return false;
            if (string.IsNullOrWhiteSpace(product.Name)) return false;
            if (product.Price <= 0) return false;

            return true;
        }

        public async Task<Guid?> AddProduct(Product product)
        {
           if(!IsProductValid(product)) return Guid.Empty;

            return await _productRepository.Add(product);
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            if (id == Guid.Empty) return false;

            return await _productRepository.Delete(id);
        }

        public IEnumerable<Product> GetProducts()
        {
            return _productRepository.GetProducts();
        }

        public Product? GetProduct(Guid id)
        {
            if(id == Guid.Empty) return null;

            return _productRepository.GetProduct(id);
        }

        public async Task<bool> UpdateProduct(Product validProduct)
        {
            if(!IsProductValid(validProduct)) return false;

            return await _productRepository.Update(validProduct);
        }
    }
}