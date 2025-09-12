using System.Net;
using MeetUp.EShop.Api.Exceptions;
using MeetUp.EShop.Business.Services;
using MeetUp.EShop.Core.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MeetUp.EShop.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetProducts")]
        public IResult GetProducts()
        {
            var products = _productService.GetProducts();
            if (products == null || !products.Any())
            {
                throw new ControllerException("Not found products", HttpStatusCode.NotFound);
            }

            Log.Information("Retrieved {Count} products successfully", products.Count());
            return Results.Ok(products);
        }

        [HttpGet("Get")]
        public IResult Get(Guid id)
        {
            var product = _productService.GetProduct(id);
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

            Log.Information("Deleted product with ID {ProductId} successfully", id);
            return Results.Ok();
        }
    }
}
