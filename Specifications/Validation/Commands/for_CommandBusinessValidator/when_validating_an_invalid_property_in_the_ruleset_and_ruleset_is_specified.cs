// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Dolittle.Commands.Validation;
using Dolittle.Validation;
using Machine.Specifications;

namespace Dolittle.FluentValidation.Commands.for_CommandBusinessValidator
{
    [Subject(typeof(CommandInputValidatorFor<>))]
    public class when_validating_an_invalid_property_in_the_ruleset_and_ruleset_is_specified : given.a_command_business_validator_with_ruleset
    {
        static IEnumerable<ValidationResult> results;

        Establish context = () =>
        {
            simple_command.SomeString = "Alms for an ex-leper";
            simple_command.SomeInt = -1;
        };

        Because of = () => results = simple_command_business_validator.ValidateFor(simple_command, SimpleCommandInputValidatorWithRuleset.SERVER_ONLY_RULESET);

        It should_have_an_invalid_property = () => results.Count().ShouldEqual(1);
    }
}