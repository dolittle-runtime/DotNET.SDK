// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Dolittle.Commands.Validation;
using Dolittle.Validation;
using Machine.Specifications;

namespace Dolittle.Commands.Validation.for_ComposedCommandBusinessValidator
{
    [Subject(typeof(ComposedCommandBusinessValidatorFor<>))]
    public class when_validating_a_valid_command_with_a_composed_validator : given.a_composed_command_business_validator
    {
        static IEnumerable<ValidationResult> result;

        Because of = () => result = composed_validator.ValidateFor(valid_command);

        It should_be_valid = () => result.ShouldBeEmpty();
    }
}