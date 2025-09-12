
using MeetUp.EShop.Presentation.Models.Product;
using MeetUp.EShop.Presentation.Services;
using MeetUp.EShop.Presentation.Services.Product;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MeetUp.EShop.Presentation.Components.Pages
{
    public partial class Home
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] private UserService UserService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var user = (await AuthStateProvider.GetAuthenticationStateAsync()).User;

            if(!user.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/login");
            }
            base.OnInitializedAsync();
        }

       
    }
}
