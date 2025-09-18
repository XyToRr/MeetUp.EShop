using System.Net;
using System.Threading.Tasks;
using MeetUp.EShop.Api.Cache;
using MeetUp.EShop.Api.Exceptions;
using MeetUp.EShop.Business.Cache.Implementation;
using MeetUp.EShop.Business.Cache.Interfaces;
using MeetUp.EShop.Business.Services;
using MeetUp.EShop.Core.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;

namespace MeetUp.EShop.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly IHybridCacheService _hybridCacheService;

        public ProductController(ProductService productService, IHybridCacheService cache)
        {
            _productService = productService;
            _hybridCacheService = cache;
        }

        [HttpGet("GetProducts")]
        public async Task<IResult> GetProducts()
        { 
            var products = await _hybridCacheService.GetCacheAsync(CacheKeys.Products,
                async () => await Task.FromResult(_productService.GetProducts().ToList()));
          
            if (products == null || !products.Any())
            {
                throw new ControllerException("Not found products", HttpStatusCode.NotFound);
            }
            
            Log.Information("Retrieved {Count} products successfully", products.Count());
            return Results.Ok(products);
        }

        [HttpGet("Get")]
        public async Task<IResult> Get(Guid id)
        {
            var productCacheKey = $"{CacheKeys.SingleProduct}{id}";

            var product = await _hybridCacheService.GetCacheAsync(productCacheKey,
                async () => await Task.FromResult(_productService.GetProduct(id)));
          
            if (product == null)
            {
                throw new ControllerException($"Not found product with id: {id}", HttpStatusCode.NotFound);
            }

            Log.Information("Retrieved product with ID {ProductId} successfully", id);
            return Results.Ok(product);
        }

        [HttpPost("Add")]
        public async Task<IResult> Add(Product product)
        {
            var id = await _productService.AddProduct(product);
            if (id == Guid.Empty)
            {
                throw new ControllerException("Bad addProduct request", HttpStatusCode.BadRequest);
            }

            var productCacheKey = $"{CacheKeys.SingleProduct}_{id}";
            await _hybridCacheService.SetCacheAsync(productCacheKey, _productService.GetProduct((Guid)id));
            await _hybridCacheService.SetCacheAsync(CacheKeys.Products, _productService.GetProducts().ToList());

            Log.Information("Added product with ID {ProductId} successfully", id);
            return Results.Ok(id);
        }

        [HttpPut("Update")]
        public async Task<IResult> Update(Product product)
        {
            var result = await _productService.UpdateProduct(product);
            if (!result)
            {
                throw new ControllerException("Bad updateProduct request", HttpStatusCode.BadRequest);
            }

            var productCacheKey = $"{CacheKeys.SingleProduct}_{product.Id}";
            var updatedProduct = _productService.GetProduct(product.Id);
            await _hybridCacheService.SetCacheAsync(productCacheKey, updatedProduct);
            await _hybridCacheService.SetCacheAsync(CacheKeys.Products, _productService.GetProducts().ToList());

            Log.Information("Updated product with ID {ProductId} successfully", product.Id);
            return Results.Ok();
        }

        [HttpDelete("Delete")]
        public async Task<IResult> Delete(Guid id)
        {
            var result = await _productService.DeleteProduct(id);
            if (!result)
            {
                throw new ControllerException("Bad deleteProduct request", HttpStatusCode.BadRequest);
            }
            
            var productCacheKey = $"{CacheKeys.SingleProduct}_{id}";
            await _hybridCacheService.RemoveCacheAsync(productCacheKey);
            await _hybridCacheService.SetCacheAsync(CacheKeys.Products, _productService.GetProducts().ToList());

            Log.Information("Deleted product with ID {ProductId} successfully", id);
            return Results.Ok();
        }
    }
}
