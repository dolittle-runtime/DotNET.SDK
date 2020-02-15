// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Commands.Validation;
using Dolittle.Validation;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Dolittle.Commands.Coordination.Runtime.for_CommandCoordinator
{
    [Subject(typeof(CommandCoordinator))]
    public class when_handling_an_invalid_command : given.a_command_coordinator
    {
        static CommandResult result;
        static CommandValidationResult validation_errors;

        Establish context = () =>
        {
            validation_errors = new CommandValidationResult
            {
                ValidationResults = new[]
                {
                    new ValidationResult("First validation failure"),
                    new ValidationResult("Second validation failure")
                }
            };

            command_validators_mock
                .Setup(cvs => cvs.Validate(command))
                .Returns(validation_errors);
        };

        Because of = () => result = coordinator.Handle(command);

        It should_have_validated_the_command = () => command_validators_mock.VerifyAll();
        It should_have_a_result = () => result.ShouldNotBeNull();
        It should_have_success_set_to_false = () => result.Success.ShouldBeFalse();
        It should_have_a_record_of_each_validation_failure = () => result.ValidationResults.ShouldContainOnly(validation_errors.ValidationResults);
        It should_not_handle_the_command = () => command_handler_manager_mock.Verify(chm => chm.Handle(command), Times.Never());
        It should_rollback_the_command_context = () => command_context_mock.Verify(c => c.Rollback(), Times.Once());
    }
}