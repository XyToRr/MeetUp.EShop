using MeetUp.EShop.Presentation.Models.User;
using MeetUp.EShop.Presentation.Services.Inteerfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MeetUp.EShop.Presentation.Components.Pages
{
    public partial class Register
    {
        [Inject] private IAuthAPI AuthAPI { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        public RegisterUser User { get; set; } = new();

        public async Task RegisterUser()
        {
            var json = System.Text.Json.JsonSerializer.Serialize(User);
            var response = await AuthAPI.Register(User);

            if (response.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo("/login");
            }
        }
    }
}
