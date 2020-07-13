namespace VOD.Domain.Requests.Video.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using VOD.Domain.Requests.Genre;
    using VOD.Domain.Requests.Kind;
    using VOD.Domain.Responses;
    using VOD.Domain.Services;

    public class AddVideoRequestValidator : AbstractValidator<AddVideoRequest>
    {
        public AddVideoRequestValidator(IGenreService genreService, IKindService kindService)
        {
            this._genreService = genreService;
            this._kindService = kindService;

            RuleFor(x => x.AltTitle).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Description).MaximumLength(1000).When(x => x.Description != null);
            RuleFor(x => x.Duration).InclusiveBetween(0, 65535).When(x => x.Duration != null);
            RuleFor(x => x.Episode).InclusiveBetween(0, 65535).When(x => x.Episode != null);
            RuleFor(x => x.GenreId)
                .NotEmpty()
                .MustAsync(GenreExists).WithMessage("Genre must exists.");
            RuleFor(x => x.KindId)
                .NotEmpty()
                .MustAsync(KindExists).WithMessage("Kind must exists.");
            //RuleFor(x => x.ReleaseYear);
            RuleFor(x => x.Season).InclusiveBetween(0, 65535).When(x => x.Season != null);
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        }

        private readonly IGenreService _genreService;

        private readonly IKindService _kindService;

        private async Task<bool> GenreExists(Guid id, CancellationToken cancelationToken)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return false;
            }

            GenreResponse result = await _genreService.GetGenreAsync(new GetGenreRequest() { Id = id });

            return result != null;
        }

        private async Task<bool> KindExists(Guid id, CancellationToken cancelationToken)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return false;
            }

            KindResponse result = await _kindService.GetKindAsync(new GetKindRequest() { Id = id });

            return result != null;
        }
    }
}
