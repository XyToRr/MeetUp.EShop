using System;
using System.Threading.Tasks;
using FluentAssertions;
using MeetUp.EShop.Presentation.Models.User;
using MeetUp.EShop.Presentation.Services.Inteerfaces;
using Refit;
using Reqnroll;

namespace MeetUp.EShop.Tests.StepDefinitions
{
    [Binding]
    public class GetUsersStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IUserAPI _userApiClient;

        private ApiResponse<List<RegisterUserUI>> _getUsersResponse;
        private ApiResponse<RegisterUserUI> _getUserResponse;

        public GetUsersStepDefinitions(IUserAPI userApiClient, ScenarioContext scenarioContext)
        {
            _userApiClient = userApiClient;
            _scenarioContext = scenarioContext;
        }

        [When("try to get all users")]
        public async Task WhenTryToGetAllUsers()
        {
            _getUsersResponse = await _userApiClient.GetUsers();
        }

        [Then("all users should be returned")]
        public void ThenAllUsersShouldBeReturned()
        {
            _getUsersResponse.IsSuccessStatusCode.Should().BeTrue();
        }

        [Then("no users should be returned")]
        public void ThenNoUsersShouldBeReturned()
        {
            _getUsersResponse?.IsSuccessStatusCode.Should().BeFalse();
        }

        [When("try to get user by existing id")]
        public async Task WhenTryToGetUserByExistingId()
        {
            var userId = _scenarioContext.Get<Guid>("ExistingUserId");
            _getUserResponse = await _userApiClient.GetUser(userId);
        }

        [Then("user should be returned")]
        public void ThenUserShouldBeReturned()
        {
            _getUserResponse.IsSuccessStatusCode.Should().BeTrue();
            _getUserResponse.Content.Should().NotBeNull();
        }

        [When("try to get user by non-existing id")]
        public async Task WhenTryToGetUserByNon_ExistingId()
        {
            var nonExistingId = Guid.NewGuid();
            _getUserResponse = await _userApiClient.GetUser(nonExistingId);
        }

        [Then("user should not be returned")]
        public void ThenUserShouldNotBeReturned()
        {
            _getUserResponse.IsSuccessStatusCode.Should().BeFalse();
        }
    }
}
