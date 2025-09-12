using MeetUp.EShop.Business.Interfaces;
using MeetUp.EShop.Core.Interfaces;
using MeetUp.EShop.Core.Models.Token;
using MeetUp.EShop.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetUp.EShop.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly UserService _userService;
        public AuthService(ITokenGenerator tokenGenerator, UserService userService) 
        {
            _tokenGenerator = tokenGenerator;
            _userService = userService;
        }
        public async Task<AccessToken?> Login(LoginUser loginUser)
        {
            var userId = _userService.GetByName(loginUser.Login);
            if (userId == null) 
                return null;

            var user = _userService.Get((Guid)userId);

            if(user == null)
                return null;
            if(!BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password))
                return null;

            var token = _tokenGenerator.GenerateToken(user);
            
            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpire = token.RefreshTokenExpire;

            await _userService.UpdateTokens(user);

            return token;
        }
        public async Task<AccessToken> RefreshToken(string refreshToken)
        {
            var userId = _userService.GetByRefreshToken(refreshToken);
            if (userId == Guid.Empty || userId == null)
                return null;


            var user = _userService.Get(userId);
            if (user == null)
                return null;
            
            if(user.RefreshTokenExpire > DateTime.UtcNow)
                return null;

            var token = _tokenGenerator.GenerateToken(user);
            
            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpire = token.RefreshTokenExpire;
            
            _userService.Update(user);
            return token;
        }
    }
}
