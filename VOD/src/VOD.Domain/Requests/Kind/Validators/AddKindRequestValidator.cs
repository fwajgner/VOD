namespace VOD.Domain.Requests.Kind.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using FluentValidation;

    public class AddKindRequestValidator : AbstractValidator<AddKindRequest>
    {
        public AddKindRequestValidator()
        {
            RuleFor(x => x.KindName).MaximumLength(30).NotEmpty();
        }
    }
}
