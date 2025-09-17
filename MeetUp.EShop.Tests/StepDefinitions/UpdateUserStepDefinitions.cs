using System;
using FluentAssertions;
using MeetUp.EShop.Core.Models.User;
using MeetUp.EShop.Presentation.Models.User;
using MeetUp.EShop.Presentation.Services.Inteerfaces;
using Refit;
using Reqnroll;

namespace MeetUp.EShop.Tests.StepDefinitions
{
    [Binding]
    public class UpdateUserStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext; 
        private readonly IUserAPI _userApiClient;

        private ApiResponse<Guid> _apiResponse;

        public UpdateUserStepDefinitions(ScenarioContext scenarioContext, IUserAPI userApiClient)
        {
            _scenarioContext = scenarioContext;
            _userApiClient = userApiClient;
        }

        [When("updating existing user")]
        public async Task WhenUpdatingExistingUser()
        {
            var userId = _scenarioContext.Get<Guid>("ExistingUserId");
            var updateUser = new UpdateUser
            {
                Id = userId,
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                Email = "updatedMail@mail.com",
                Login = "updatedLogin",
                Password = "updatedPassword_123"
            };

            _scenarioContext["UsersToRegister"] = new RegisterUser
            {
                FirstName = updateUser.FirstName,
                LastName = updateUser.LastName,
                Email = updateUser.Email,
                Login = updateUser.Login,
                Password = updateUser.Password
            };

            _apiResponse = await _userApiClient.UpdateUser(updateUser);
        }

        [Then("the user shoul be updated successfulll")]
        public void ThenTheUserShoulBeUpdatedSuccessfulll()
        {
           _apiResponse.IsSuccessStatusCode.Should().BeTrue();
        }

        [When("updating non-existing user")]
        public async Task WhenUpdatingNon_ExistingUser()
        {
            var updateUser = new UpdateUser
            {
                Id = Guid.NewGuid(),
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                Email = "updatedMail@mail.com",
                Login = "updatedLogin",
                Password = "updatedPassword_123"
            };
            _apiResponse = await _userApiClient.UpdateUser(updateUser);
        }

        [Then("the user should not be updated")]
        public void ThenTheUserShouldNotBeUpdated()
        {
            _apiResponse.IsSuccessStatusCode.Should().BeFalse();
        }
    }
}
