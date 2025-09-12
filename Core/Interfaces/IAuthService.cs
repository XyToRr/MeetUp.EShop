using MeetUp.EShop.Core.Models.Token;
using MeetUp.EShop.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetUp.EShop.Core.Interfaces
{
    public interface IAuthService
    {
        public Task<AccessToken?> Login(LoginUser user);
        public Task<AccessToken?> RefreshToken(string refreshToken);
    }
}
