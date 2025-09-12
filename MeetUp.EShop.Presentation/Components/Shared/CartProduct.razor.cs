using MeetUp.EShop.Presentation.Models.Product;
using MeetUp.EShop.Presentation.Services.Authorization;
using MeetUp.EShop.Presentation.Services.Extensions;
using MeetUp.EShop.Presentation.Services.Inteerfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MeetUp.EShop.Presentation.Components.Shared
{
    public partial class CartProduct
    {
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] private IUserAPI UserAPI { get; set; }
        [Parameter]public Product Product { get; set; }
    }


}
