using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MeetUp.EShop.Presentation.Services.Product;
using MeetUp.EShop.Presentation.Models.Product;
using MeetUp.EShop.Presentation.Services.Inteerfaces;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MeetUp.EShop.Presentation.Services.Extensions;

namespace MeetUp.EShop.Presentation.Components.Shared
{
    public partial class ProductList
    {
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] private ProductService ProductService { get; set; }
        [Inject] private IUserAPI UserAPI { get; set; }

        private List<Models.Product.Product> Products { get; set; } = new List<Models.Product.Product>();
        private List<Guid> CurrentUserCart { get; set; } = new List<Guid>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            if (firstRender)
            {
                Products = await ProductService.GetProducts();

                var currentUserId = await AuthStateProvider.GetCurrentUserId();
                if (currentUserId != Guid.Empty)
                {
                    var res = await UserAPI.GetCart(currentUserId);
                    CurrentUserCart = (await UserAPI.GetCart(currentUserId)).Content;
                    if(CurrentUserCart == null)
                    {
                        CurrentUserCart = new List<Guid>();
                    }
                }
                StateHasChanged();
            }
            base.OnAfterRenderAsync(firstRender);
        }
    }
}

