using System.Net;
using System.Text.Json;
using Talabat.Api.Errors;

namespace Talabat.Api.MiddleWares
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleWare> logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddleWare(RequestDelegate next,ILogger<ExceptionMiddleWare> logger,IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;

        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode=(int) HttpStatusCode.InternalServerError;
                // start camelcase
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy=JsonNamingPolicy.CamelCase
                };
                // end
                var response = env.IsDevelopment() ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError,
                    ex.Message, ex.StackTrace.ToString())
                    :new ApiErrorResponse((int)HttpStatusCode.InternalServerError,ex.Message);
                var json=JsonSerializer.Serialize(response,options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
