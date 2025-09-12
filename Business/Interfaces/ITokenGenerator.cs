using MeetUp.EShop.Core.Models.Token;
using MeetUp.EShop.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetUp.EShop.Business.Interfaces
{
    public interface ITokenGenerator
    {
        public AccessToken GenerateToken(User user);
    }
}
