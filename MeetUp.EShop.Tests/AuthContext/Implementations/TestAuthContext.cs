using MeetUp.EShop.Tests.AuthContext.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetUp.EShop.Tests.AuthContext.Implementations
{
    public class TestAuthContext : ITestAuthContext
    {
        public string AccessToken { get ; set; }
    }
}
