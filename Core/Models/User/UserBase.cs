using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetUp.EShop.Core.Models.User
{
    public abstract class UserBase
    {
        public string Login { get; set; }
        public string Password { get; set; }

    }
}
