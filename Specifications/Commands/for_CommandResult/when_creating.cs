﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Machine.Specifications;

namespace Dolittle.Commands.for_CommandResult
{
    public class when_creating
    {
        static CommandResult result;

        Because of = () => result = new CommandResult();

        It should_have_an_empty_validation_results = () => result.ValidationResults.ShouldBeEmpty();
        It should_have_an_empty_command_error_messages = () => result.CommandValidationMessages.ShouldBeEmpty();
        It should_have_no_exception_instance = () => result.Exception.ShouldBeNull();
    }
}
