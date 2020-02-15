﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Machine.Specifications;

namespace Dolittle.Queries.Validation.Specs.for_QueryArgumentValidationResult
{
    public class when_asking_for_success_on_result_without_any_broken_rules : given.a_query_argument_validation_result_without_any_broken_rules
    {
        static bool success;

        Because of = () => success = result.Success;

        It should_be_considered_successful = () => success.ShouldBeTrue();
    }
}
