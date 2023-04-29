using Serilog;
using System.Text;

namespace Events.It.Academy.Ge.Api.Infrastructure.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            await LogRequest(context.Request);

            Stream originalBody = context.Response.Body;

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    context.Response.Body = memoryStream;

                    await _next(context);
                    LoggingResponse(context.Response, memoryStream);

                    memoryStream.Position = 0;
                    await memoryStream.CopyToAsync(originalBody);
                }
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }

        private void LoggingResponse(HttpResponse response, MemoryStream memoryStream)
        {
            memoryStream.Position = 0;
            string responseBody = new StreamReader(memoryStream).ReadToEnd();
            Console.WriteLine(responseBody);

            var toLog = $"-----------------Response------------------ " +
               $"StatusCode = {response.StatusCode}\n" +
               $"TraceIdentifier = {response.HttpContext.TraceIdentifier}\n" +
               $"Body = {responseBody}\n" +
               $"ResponseTime = {DateTime.Now}\n";

            Log.Information(toLog);
        }

        private async Task LogRequest(HttpRequest request)
        {

            var toLog = $" ------------------Request----------------\n" +
                $"IP = {request.HttpContext.Connection.RemoteIpAddress}\n" +
                $"Address = {request.Scheme}\n" +
                $"Method = {request.Method}\n" +
                $"Path = {request.Path}\n" +
                $"IsSecured = {request.IsHttps}\n" +
                $"QueryString = {request.QueryString}\n" +
                $"RequestBody = {await ReadRequestBody(request)}\n" +
                $"Time = {DateTime.Now}\n";

            Log.Information(toLog);
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();

            var buffer = new byte[request.ContentLength ?? 0];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            var bodyAsText = Encoding.UTF8.GetString(buffer);

            request.Body.Position = 0;

            return bodyAsText;
        }
    }
}
