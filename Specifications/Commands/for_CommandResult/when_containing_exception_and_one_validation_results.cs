﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Dolittle.Validation;
using Machine.Specifications;

namespace Dolittle.Commands.for_CommandResult
{
    public class when_containing_exception_and_one_validation_results
    {
        static CommandResult result;
        static string error_message = "Something";

        Because of = () => result = new CommandResult
        {
            Exception = new NotImplementedException(),
            ValidationResults = new[]
            {
                new ValidationResult("Something")
            }
        };

        It should_not_be_valid = () => result.Invalid.ShouldBeTrue();
        It should_not_be_successful = () => result.Success.ShouldBeFalse();

        It should_have_only_the_validation_result_in_all_validation_errors = () =>
                                                                                    {
                                                                                        result.AllValidationMessages.Count().ShouldEqual(1);
                                                                                        result.AllValidationMessages.First().ShouldEqual(error_message);
                                                                                    };
    }
}
