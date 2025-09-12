using MeetUp.EShop.Core.Enums;
using MeetUp.EShop.Core.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetUp.EShop.Core.Models.User
{
    public class User : UserBase
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpire { get; set; }

        public List<Order.Order>? Orders { get; set; }
        public UserRole? Role { get; set; }
    }
}