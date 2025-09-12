using MeetUp.EShop.Presentation.Services.Inteerfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MeetUp.EShop.Presentation.Services.Product;
using MeetUp.EShop.Presentation.Services.Extensions;
using MeetUp.EShop.Presentation.Models.Order;
using System.Text.Json;

namespace MeetUp.EShop.Presentation.Components.Pages
{
    public partial class Cart
    {
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] private ProductService ProductService { get; set; }
        [Inject] private IUserAPI UserAPI { get; set; }
        [Inject] private IOrderAPI OrderAPI { get; set; }

        private List<Models.Product.Product> Products { get; set; } = new List<Models.Product.Product>();
        private List<Guid> CurrentUserCart { get; set; } = new List<Guid>();
        private Order CurrentOrder { get; set; } = new Order();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var currentUserId = await AuthStateProvider.GetCurrentUserId();
                if (currentUserId != Guid.Empty)
                {
                    Products = await ProductService.GetProducts();
                    CurrentUserCart = (await UserAPI.GetCart(currentUserId)).Content;
                    if(CurrentUserCart == null)
                    {
                        CurrentUserCart = new List<Guid>();
                    }
                    CurrentOrder = (await UserAPI.GetLastOrder(currentUserId)).Content;
                }
                StateHasChanged();
            }
        }

        private async Task BuyProducts()
        {
            if (CurrentOrder != null)
            {
                CurrentOrder.Status = Enums.OrderStatus.Delivered;
                var json = JsonSerializer.Serialize(CurrentOrder);
                var res = await OrderAPI.UpdateOrder(CurrentOrder);
                CurrentUserCart = new List<Guid>();
                StateHasChanged();
            }
             

        }
    }
}
