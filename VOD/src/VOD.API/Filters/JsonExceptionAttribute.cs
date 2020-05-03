namespace VOD.API.Filters
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System.Net;
    using VOD.API.Exceptions;

    public class JsonExceptionAttribute : TypeFilterAttribute
    {
        public JsonExceptionAttribute() : base(typeof(HttpCustomExceptionFilterImpl))
        {

        }

        public class HttpCustomExceptionFilterImpl : IExceptionFilter
        {
            public HttpCustomExceptionFilterImpl(IWebHostEnvironment env,
                ILogger<HttpCustomExceptionFilterImpl> logger)
            {
                _env = env;
                _logger = logger;
            }

            private readonly IWebHostEnvironment _env;
            private readonly ILogger<HttpCustomExceptionFilterImpl> _logger;

            public void OnException(ExceptionContext context)
            {
                EventId eventId = new EventId(context.Exception.HResult);

                _logger.LogError(eventId,
                    context.Exception,
                    context.Exception.Message);

                JsonErrorPayload json = new JsonErrorPayload { EventId = eventId.Id };

                if (_env.IsDevelopment())
                {
                    json.DetailedMessage = context.Exception;
                }

                ObjectResult exceptionObject = new ObjectResult(json) { StatusCode = 500 };

                context.Result = exceptionObject;
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }
    }
}
