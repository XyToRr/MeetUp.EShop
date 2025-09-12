using MeetUp.EShop.Api.Exceptions;
using Serilog;
using System.Net;

namespace MeetUp.EShop.Api.Middlewares
{
    public class ExceptionCatcher
    {
        private readonly RequestDelegate _next;
        public ExceptionCatcher(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(ControllerException ex)
            {
                context.Response.StatusCode = (int)ex.StatusCode;
                Log.Logger.Error(ex, ex.Message);
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Log.Logger.Error(ex, ex.Message);
                await context.Response.WriteAsync("Internal server error");
            }
        }
    }
}
