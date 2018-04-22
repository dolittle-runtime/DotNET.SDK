﻿using Dolittle.Commands.Validation;
using FluentValidation;

namespace Dolittle.FluentValidation.Commands
{
    public class AnotherSimpleCommandInputValidator : CommandInputValidatorFor<AnotherSimpleCommand>
    {
        public AnotherSimpleCommandInputValidator()
        {
            RuleFor(asc => asc.SomeString).NotEmpty();
            RuleFor(asc => asc.SomeInt).GreaterThanOrEqualTo(1);
        }
    }
}