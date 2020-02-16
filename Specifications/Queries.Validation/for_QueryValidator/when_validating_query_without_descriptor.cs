﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Dolittle.Queries.Validation.Specs.for_QueryValidator
{
    public class when_validating_query_without_descriptor : given.a_query_validator
    {
        static SomeQuery query;
        static QueryValidationResult result;

        Establish context = () =>
        {
            query = new SomeQuery();
            query_validation_descriptors_mock.Setup(q => q.HasDescriptorFor<SomeQuery>()).Returns(false);
        };

        Because of = () => result = query_validator.Validate(query);

        It should_return_a_valid_result = () => result.Success.ShouldBeTrue();
        It should_check_if_has_descriptors = () => query_validation_descriptors_mock.Verify(q => q.HasDescriptorFor<SomeQuery>(), Times.Once());
        It should_not_get_descriptor = () => query_validation_descriptors_mock.Verify(q => q.GetDescriptorFor<SomeQuery>(), Times.Never());
    }
}
