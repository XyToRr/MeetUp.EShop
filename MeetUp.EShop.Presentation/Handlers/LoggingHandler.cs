namespace MeetUp.EShop.Presentation.Handlers;
public class LoggingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var body = request.Content != null ? await request.Content.ReadAsStringAsync() : "<no body>";
        Console.WriteLine($"Request: {request.Method} {request.RequestUri}\nBody: {body}");

        var response = await base.SendAsync(request, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response: {(int)response.StatusCode} {response.ReasonPhrase}\nBody: {responseBody}");

        return response;
    }
}