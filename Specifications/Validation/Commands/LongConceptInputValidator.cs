// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.FluentValidation.Concepts.given;
using Dolittle.Validation;
using FluentValidation;

namespace Dolittle.FluentValidation.Commands
{
    public class LongConceptInputValidator : InputValidator<LongConcept>
    {
        public LongConceptInputValidator()
        {
            RuleFor(s => s.Value)
                .NotEmpty();
        }
    }
}