using System;
using FluentAssertions;
using MeetUp.EShop.Presentation.Services.Inteerfaces;
using Refit;
using Reqnroll;

namespace MeetUp.EShop.Tests.StepDefinitions
{
    [Binding]
    public class DeleteUserStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IUserAPI _userApiClient;

        private ApiResponse<object> _response;

        public DeleteUserStepDefinitions(IUserAPI userApiClient, ScenarioContext scenarioContext)
        {
            _userApiClient = userApiClient;
            _scenarioContext = scenarioContext;
        }

        [When("try to delete existing user")]
        public async Task WhenTryToDeleteExistingUser()
        {
            var userId = _scenarioContext.Get<Guid>("ExistingUserId");
            _response = await _userApiClient.DeleteUser(userId);
        }


        [When("try to delete non-existing user")]
        public async Task WhenTryToDeleteNon_ExistingUser()
        {
            var userId = Guid.NewGuid();
            _response =  await _userApiClient.DeleteUser(userId);
        }


        [Then("user should be deleted successfully")]
        public void ThenUserShouldBeDeletedSuccessfully()
        {
            _response.IsSuccessStatusCode.Should().BeTrue();
        }

        [Then("user should not be found")]
        public void ThenUserShouldNotBeFound()
        {
            _response.IsSuccessStatusCode.Should().BeFalse();
            _response.StatusCode.Should().NotBe(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}
