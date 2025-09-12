using System.Net;

namespace MeetUp.EShop.Api.Exceptions
{
    public class ControllerException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public ControllerException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
