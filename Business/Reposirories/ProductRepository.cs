using MeetUp.EShop.Core.Interfaces;
using MeetUp.EShop.Core.Models.Product;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeetUp.EShop.Business.Reposirories
{
    public class ProductRepository : IProductRepository
    {
        private readonly EShopDbContext _context;

        public ProductRepository(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Add(Product product)
        {
            product.Id = Guid.NewGuid();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<bool> Delete(Guid guid)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == guid);
            if (product == null)
                return false;
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public Product? GetProduct(Guid guid)
        {
            return _context.Products.FirstOrDefault(p => p.Id == guid);
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        public async Task<bool> Update(Product product)
        {
            var oldProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id);
            if (oldProduct == null)
                return false;
            oldProduct.Name = product.Name;
            oldProduct.Code = product.Code;
            oldProduct.Price = product.Price;
            _context.Products.Update(oldProduct);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
