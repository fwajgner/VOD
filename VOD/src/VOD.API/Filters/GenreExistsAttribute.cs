namespace VOD.API.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VOD.API.Exceptions;
    using VOD.Domain.Requests.Genre;
    using VOD.Domain.Responses;
    using VOD.Domain.Services;

    public class GenreExistsAttribute : TypeFilterAttribute
    {
        public GenreExistsAttribute() : base(typeof(GenreExistsFilterImpl))
        {

        }

        public class GenreExistsFilterImpl : IAsyncActionFilter
        {
            public GenreExistsFilterImpl(IGenreService genreService)
            {
                this.GenreService = genreService;
            }

            private IGenreService GenreService { get; }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (!(context.ActionArguments["id"] is Guid id))
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                GenreResponse result = await GenreService.GetGenreAsync(new GetGenreRequest { Id = id });

                if (result == null)
                {
                    context.Result = new NotFoundObjectResult(new JsonErrorPayload
                    { DetailedMessage = $"Genre with id {id} not exist." });
                    return;
                }

                await next();
            }
        }
    }
}
