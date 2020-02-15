﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dolittle.Commands.Validation.Specs.for_CommandValidators
{
    public class second_command_validator : ICommandValidator
    {
        public CommandValidationResult result_to_return;
        public bool validate_called;
        public CommandRequest command_passed_to_validate;

        public CommandValidationResult Validate(CommandRequest command)
        {
            validate_called = true;
            command_passed_to_validate = command;
            return result_to_return;
        }
    }
}
