namespace VOD.Domain.Requests.Genre.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using FluentValidation;

    public class AddGenreRequestValidator : AbstractValidator<AddGenreRequest>
    {
        public AddGenreRequestValidator()
        {
            RuleFor(x => x.GenreName).MaximumLength(30).NotEmpty();
        }
    }
}
