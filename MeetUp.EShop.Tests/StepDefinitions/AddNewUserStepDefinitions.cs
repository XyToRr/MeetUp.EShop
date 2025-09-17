using System;
using MeetUp.EShop.Core.Models.User;
using MeetUp.EShop.Presentation.Models.User;
using MeetUp.EShop.Presentation.Services.Inteerfaces;
using FluentAssertions;
using OfficeOpenXml;
using Refit;
using Reqnroll;

namespace MeetUp.EShop.Tests.StepDefinitions
{
    [Binding]
    public class AddNewUserStepDefinitions
    {
        private readonly IUserAPI _userApiClient;
        private readonly ScenarioContext _scenarioContext;
        private readonly IAuthAPI _authApiClient;

        private List<RegisterUser> _usersToRegister = new();
        private RegisterUser _newUser;
        private ApiResponse<Guid> _apiResponse;
        

        public AddNewUserStepDefinitions(IUserAPI userCLient, IAuthAPI authClient, ScenarioContext scenarioContext)
        {
            _userApiClient = userCLient;
            _authApiClient = authClient;
            _scenarioContext = scenarioContext;
        }
        
        [Given("user exists:")]
        public async Task GivenUserExists(DataTable dataTable)
        {
            var user = dataTable.CreateInstance<RegisterUser>();
            _scenarioContext["UsersToRegister"] = user;
            await _authApiClient.Register(new RegisterUserUI
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Login = user.Login,
                Password = user.Password
            });
        }

        [Given("user tries tu register with data:")]
        public void GivenUserTriesTuRegisterWithData(DataTable dataTable)
        {
            var user = dataTable.CreateInstance<RegisterUser>();
            _scenarioContext["UsersToRegister"] = user;
            _newUser = user;
        }

        [When("user tries to register with given data")]
        public async Task WhenUserTriesToRegisterWithGivenData()
        {
                var registerUser = new RegisterUserUI
                {
                    FirstName = _newUser.FirstName,
                    LastName = _newUser.LastName,
                    Email = _newUser.Email,
                    Login = _newUser.Login,
                    Password = _newUser.Password
                };

            _apiResponse = await _authApiClient.Register(registerUser);
        }

        [Then("user should be registered successfully")]
        public void ThenUserShouldBeRegisteredSuccessfully()
        {   
                _apiResponse.IsSuccessStatusCode.Should().BeTrue();
                _apiResponse.Content.Should().NotBeEmpty();
        }

        [Then("user should not be registered")]
        public void ThenUserShouldNotBeRegistered()
        { 
            _apiResponse.IsSuccessStatusCode.Should().BeFalse();
        }
    }
}
