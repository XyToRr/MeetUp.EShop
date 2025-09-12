using MeetUp.EShop.Presentation.Models.User;
using MeetUp.EShop.Presentation.Services;
using MeetUp.EShop.Presentation.Services.Authorization;
using MeetUp.EShop.Presentation.Services.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Text.Json;

namespace MeetUp.EShop.Presentation.Components.Pages
{
    public partial class EditUser
    {
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private UserService UserService { get; set; }
        public UpdateUserData User { get; set; } = new UpdateUserData();

        private string Message { get; set; }
        private bool IsUpdateSuccessful { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (!state.User.Identity.IsAuthenticated)
                NavigationManager.NavigateTo("/");

            var userId = await AuthenticationStateProvider.GetCurrentUserId();
            var user = await UserService.GetUserById(userId);
            if (user != null)
            {
                User.Login = user.Login;
                User.Email = user.Email;
                User.FirstName = user.FirstName;
                User.LastName = user.LastName;

            }
        }

        public async Task UpdateUser()
        {
            User.Id = await AuthenticationStateProvider.GetCurrentUserId();
            User.Password = string.Empty;
            var json = JsonSerializer.Serialize(User);
            var res = await UserService.UpdateUser(User);
            
            IsUpdateSuccessful = res;
            if (res)
            {
               
                Message = localizer["SuccessfullUpdateMessage"];
            }
            else
            {
                Message = localizer["FailUpdateMessage"];
            }
        }
    }
}
