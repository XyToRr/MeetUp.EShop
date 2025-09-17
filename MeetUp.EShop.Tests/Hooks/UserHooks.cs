using DataAccess.Context;
using MeetUp.EShop.Core.Interfaces;
using MeetUp.EShop.Core.Models.User;
using MeetUp.EShop.Presentation.Services.Inteerfaces;
using MeetUp.EShop.Tests.AuthContext.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LoginUser = MeetUp.EShop.Presentation.Models.User.LoginUser;

namespace MeetUp.EShop.Tests.Hooks
{
    [Binding]
    public class UserHooks
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IUserRepository _userRepository;
        private readonly IAuthAPI _authApiClient;
        private readonly ITestAuthContext _authContext;
        public UserHooks(
            ScenarioContext scenarioContext,
            IUserRepository userRepository,
            IAuthAPI authAPI,
            ITestAuthContext authContext)
        {
            _scenarioContext = scenarioContext;
            _userRepository = userRepository;
            _authApiClient = authAPI;
            _authContext = authContext;

        }

        [AfterScenario("@apiChangingDB")]
        public async Task AfterScenario()
        {
            //Thread.Sleep(1000);
            var user = _scenarioContext.Get<RegisterUser>("UsersToRegister");

            var userId = _userRepository.GetByName(user.Login);
            if (userId != null)
                await _userRepository.Delete((Guid)userId);

            _scenarioContext.Clear();
        }


        [BeforeScenario("@requresExistingUser", Order = 1)]
        public async Task SeedUser()
        {
            var user = new RegisterUser
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Email = "someMail@mail.com",
                Login = "test",
                Password = "test_123"
            };
            await _userRepository.Register(user);
            _scenarioContext["UsersToRegister"] = user;
            //context["UsersToRegister"] = user;

            var id = _userRepository.GetByName(user.Login);
            //_scenarioContext["ExistingUserId"] = id;
            _scenarioContext["ExistingUserId"] = id;

        }

        [AfterScenario("@fullCleanUp", Order = 1)]
        public async Task DeleteAllUsers()
        {
            var userIds = _scenarioContext.Get<List<Guid?>?>("ExistingUserIds"); 
            if (userIds != null)
            {
                foreach (var userId in userIds)
                {
                    await _userRepository.Delete((Guid)userId);
                }
            }
        }
       

        [BeforeScenario("@requiresUsers", Order = 1)]
        public async Task SeedUsers()
        {
            // Створюємо список користувачів
            var users = new List<RegisterUser>
            {
                new RegisterUser
                {
                    FirstName = "TestFirstName1",
                    LastName = "TestLastName1",
                    Email = "user1@mail.com",
                    Login = "test1",
                    Password = "test_123"
                },
                new RegisterUser
                {
                    FirstName = "TestFirstName2",
                    LastName = "TestLastName2",
                    Email = "user2@mail.com",
                    Login = "test2",
                    Password = "test_123"
                },
                new RegisterUser
                {
                    FirstName = "TestFirstName3",
                    LastName = "TestLastName3",
                    Email = "user3@mail.com",
                    Login = "test3",
                    Password = "test_123"
                }
            };

            // Додаємо користувачів у репозиторій
            foreach (var user in users)
            {
                await _userRepository.Register(user);
            }

            // Отримуємо їхні ID після збереження
            var userIds = users.Select(u => _userRepository.GetByName(u.Login)).ToList();

            // Зберігаємо користувачів і їхні ID у ScenarioContext
            _scenarioContext["ExistingUserIds"] = userIds;
        }


        [BeforeScenario("@requiresLogin", Order = 2)]
        public async Task LoginTestUser()
        {
            var responce = await _authApiClient.Login(new LoginUser()
            {
                Login = "test",
                Password = "test_123"
            });

            _authContext.AccessToken = responce.Content.JWTToken;
            _scenarioContext["AccessToken"] = responce.Content.JWTToken;
            //context["AccessToken"] = responce.Content.JWTToken;


        }
    }
}

