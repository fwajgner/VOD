namespace VOD.API.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VOD.API.Exceptions;
    using VOD.Domain.Requests.Kind;
    using VOD.Domain.Responses;
    using VOD.Domain.Services;

    public class KindExistsAttribute : TypeFilterAttribute
    {
        public KindExistsAttribute() : base(typeof(KindExistsFilterImpl))
        {

        }

        public class KindExistsFilterImpl : IAsyncActionFilter
        {
            public KindExistsFilterImpl(IKindService kindService)
            {
                this.KindService = kindService;
            }

            private IKindService KindService { get; }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (!(context.ActionArguments["id"] is Guid id))
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                KindResponse result = await KindService.GetKindAsync(new GetKindRequest { Id = id });

                if (result == null)
                {
                    context.Result = new NotFoundObjectResult(new JsonErrorPayload
                    { DetailedMessage = $"Kind with id {id} not exist." });
                    return;
                }

                await next();
            }
        }
    }
}
