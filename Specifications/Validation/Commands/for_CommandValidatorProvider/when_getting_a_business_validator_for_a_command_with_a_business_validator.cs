﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Commands.Validation;
using Dolittle.Validation;
using Machine.Specifications;

namespace Dolittle.FluentValidation.Commands.for_CommandValidatorProvider
{
    [Subject(typeof(CommandValidatorProvider))]
    public class when_getting_a_business_validator_for_a_command_with_a_business_validator : given.a_command_validator_provider_with_input_and_business_validators
    {
        static ICanValidate business_validator;

        Establish context = () => container_mock.Setup(c => c.Get(typeof(SimpleCommandBusinessValidator))).Returns(() => new SimpleCommandBusinessValidator());

        Because of = () => business_validator = command_validator_provider.GetBusinessValidatorFor(new SimpleCommand());

        It should_return_the_correct_business_validator = () => business_validator.ShouldBeOfExactType<SimpleCommandBusinessValidator>();
    }
}