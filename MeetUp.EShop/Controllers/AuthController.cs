using MeetUp.EShop.Api.Exceptions;
using MeetUp.EShop.Core.Interfaces;
using MeetUp.EShop.Core.Models.User;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;

namespace MeetUp.EShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IResult> Login(LoginUser loginUser)
        {
            var result = await _authService.Login(loginUser);
            if (result == null)
                throw new ControllerException("Invalid login or password", HttpStatusCode.Unauthorized);

            Log.Information($"User {loginUser.Login} logged in");
            return Results.Ok(result);
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IResult> RefreshToken(string refreshToken)
        {
            var result = await _authService.RefreshToken(refreshToken);
            if (result == null)
                throw new ControllerException("Invalid token", HttpStatusCode.Unauthorized);
            
            return Results.Ok(result);
        }
    }
}
