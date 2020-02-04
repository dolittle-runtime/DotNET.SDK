﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Commands.Validation;
using Dolittle.Validation;
using Machine.Specifications;
using It = Machine.Specifications.It;

namespace Dolittle.FluentValidation.Commands.for_CommandValidatorProvider
{
    [Subject(typeof(CommandValidatorProvider))]
    public class when_getting_a_business_validator_for_a_command_with_no_business_validator_for_the_first_time : given.a_command_validator_provider_with_input_and_business_validators
    {
        static ICanValidate business_validator;
        static MySimpleCommand command;

        Establish context = () => command = new MySimpleCommand();

        Because of = () => business_validator = command_validator_provider.GetBusinessValidatorFor(command);

        It should_return_a_dynamically_constructed_validator = () => business_validator.ShouldBeOfExactType(typeof(ComposedCommandBusinessValidatorFor<MySimpleCommand>));
    }
}