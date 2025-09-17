using MeetUp.EShop.Tests.AuthContext.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetUp.EShop.Tests.RequestHandlers
{
    public class TestAuthHandler : DelegatingHandler
    {
        private readonly ITestAuthContext _authContext;
        public TestAuthHandler(ITestAuthContext context)
        {
            _authContext = context;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authContext.AccessToken);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
