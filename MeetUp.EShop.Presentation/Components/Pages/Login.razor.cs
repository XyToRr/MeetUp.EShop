using MeetUp.EShop.Presentation.Models.User;
using MeetUp.EShop.Presentation.Services.Authorization;
using MeetUp.EShop.Presentation.Services.Inteerfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json.Serialization;

namespace MeetUp.EShop.Presentation.Components.Pages
{
    public partial class Login
    {
        [Inject] private IAuthAPI Api { get; set; }
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        public LoginUser User { get; set; } = new();

        public async Task LoginUser()
        {
            try
            {
                var response = await Api.Login(User);

                if (!response.IsSuccessStatusCode)
                {
                    return;
                }

                if (AuthStateProvider is EShopAuthStateProvider stateProvider)
                {
                    var token = response.Content;
                    if (token != null)
                    {
                        await stateProvider.Login(token);
                        NavigationManager.NavigateTo("/");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
