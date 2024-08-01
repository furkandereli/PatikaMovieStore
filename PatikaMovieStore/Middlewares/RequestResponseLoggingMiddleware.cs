using System.Text;

namespace PatikaMovieStore.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await LogRequest(context);

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            await LogResponse(context);

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            await context.Request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length));
            var requestBody = Encoding.UTF8.GetString(buffer);
            context.Request.Body.Position = 0;

            _logger.LogInformation("Incoming Request:");
            _logger.LogInformation($"Scheme: {context.Request.Scheme}");
            _logger.LogInformation($"Host: {context.Request.Host}");
            _logger.LogInformation($"Path: {context.Request.Path}");
            _logger.LogInformation($"QueryString: {context.Request.QueryString}");
            _logger.LogInformation($"Request Body: {requestBody}");
        }

        private async Task LogResponse(HttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation("Outgoing Response:");
            _logger.LogInformation($"Status Code: {context.Response.StatusCode}");
            _logger.LogInformation($"Response Body: {responseBody}");
        }
    }
}
