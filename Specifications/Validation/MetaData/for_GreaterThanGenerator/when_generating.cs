﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Validation.MetaData;
using FluentValidation.Validators;
using Machine.Specifications;

namespace Dolittle.FluentValidation.MetaData.for_GreaterThanGenerator
{
    public class when_generating
    {
        static GreaterThanValidator validator;
        static GreaterThanGenerator generator;
        static GreaterThan result;

        Establish context = () =>
        {
            validator = new GreaterThanValidator(5.7f);
            generator = new GreaterThanGenerator();
        };

        Because of = () => result = generator.GeneratorFrom("someProperty", validator) as GreaterThan;

        It should_create_a_rule = () => result.ShouldNotBeNull();
        It should_pass_along_the_value = () => result.Value.ShouldEqual(validator.ValueToCompare);
    }
}
