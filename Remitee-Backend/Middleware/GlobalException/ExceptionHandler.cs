using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace Remitee_Backend.Middleware.GlobalException
{
    public sealed class ExceptionHandler
    {

        private readonly RequestDelegate nextMiddleware;

        public ExceptionHandler(RequestDelegate nextMiddleware)
        {
            this.nextMiddleware = nextMiddleware;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await nextMiddleware.Invoke(context);
            }
            catch (KeyNotFoundException knfException)
            {
                await SendPayload(context, Envelope.Error(knfException.Message), HttpStatusCode.NotFound);
            }
            catch (Exception exception)
            {
                await SendPayload(context, Envelope.Error(exception.Message));
            }
        }

        private async Task SendPayload<TPayload>(HttpContext context, TPayload payload, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var result = JsonConvert.SerializeObject(payload, settings);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(result);
        }
    }
}
