using MeetUp.EShop.Presentation.Models.Product;
using MeetUp.EShop.Presentation.Services.Authorization;
using MeetUp.EShop.Presentation.Services.Extensions;
using MeetUp.EShop.Presentation.Services.Inteerfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MeetUp.EShop.Presentation.Components.Shared
{
    public partial class ProductComponent
    {
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] private IUserAPI UserAPI { get; set; }

        [Parameter] public bool IsInCart { get; set; }
        [CascadingParameter(Name = "Product")]
        public Product Product { get; set; }

        public async Task AddProductToOrder()
        {
            var userId = await AuthStateProvider.GetCurrentUserId();
            if (userId == Guid.Empty)
            {
                if(AuthStateProvider is EShopAuthStateProvider eShopAuthStateProvider)
                {
                    await eShopAuthStateProvider.MakeAnonymous();
                }
            }
            var result = (await UserAPI.AddProductToOrder(Product.Id, userId)).Content;
           
                IsInCart = true;
            
            StateHasChanged();
        }
    }


}
