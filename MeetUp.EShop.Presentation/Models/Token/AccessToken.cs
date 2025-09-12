using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetUp.EShop.Presentation.Models.Token
{
    public class AccessToken
    {
        public string JWTToken { get; set; }
        public string RefreshToken { get; set; }    
        public DateTime RefreshTokenExpire {  get; set; }
    }
}
