using MeetUp.EShop.Presentation.Services.Inteerfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetUp.EShop.Tests.TestApiContext
{
    //public class TestApiContext
    //{
    //    public IUserAPI UserAPI { get; set; }  
    //    public IAuthAPI AuthAPI { get; set; }

    //    public TestApiContext()
    //    {
    //        var factory = new WebApplicationFactory<Program>();
    //        var client = factory.CreateClient();

    //        var refitSettings = new RefitSettings
    //        {
    //            ContentSerializer = new NewtonsoftJsonContentSerializer()
    //        };
    //        UserAPI = RestService.For<IUserAPI>(client, refitSettings);
    //        AuthAPI = RestService.For<IAuthAPI>(client, refitSettings);
    //    }
    //}
}
