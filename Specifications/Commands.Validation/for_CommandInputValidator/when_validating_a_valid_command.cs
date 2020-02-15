// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Dolittle.Commands.Validation;
using Dolittle.Validation;
using Machine.Specifications;

namespace Dolittle.Commands.Validation.for_CommandInputValidator
{
    [Subject(typeof(CommandInputValidatorFor<>))]
    public class when_validating_a_valid_command : given.a_command_input_validator
    {
        static IEnumerable<ValidationResult> results;

        Establish context = () =>
        {
            simple_command.SomeString = "Something, something, something, Dark Side";
            simple_command.SomeInt = 42;
        };

        Because of = () => results = simple_command_input_validator.ValidateFor(simple_command);

        It should_have_no_invalid_properties = () => results.ShouldBeEmpty();
    }
}