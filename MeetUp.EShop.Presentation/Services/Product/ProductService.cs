using MeetUp.EShop.Presentation.Services.Inteerfaces;

namespace MeetUp.EShop.Presentation.Services.Product
{
    public class ProductService
    {
        private readonly IProductAPI _productAPI;

        public ProductService(IProductAPI productAPI)
        {
            _productAPI = productAPI;
        }

        public async Task<List<Models.Product.Product>> GetProducts()
        {

            var response = await _productAPI.GetProducts();
            if (response.IsSuccessStatusCode)
            {
                return response.Content;
            }
            else
            {
                return null;
            }

        }
    }
}
