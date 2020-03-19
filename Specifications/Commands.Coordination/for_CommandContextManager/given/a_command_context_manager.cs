﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Events;
using Dolittle.Events.Handling;
using Dolittle.Execution;
using Dolittle.Logging;
using Machine.Specifications;
using Moq;

namespace Dolittle.Commands.Coordination.for_CommandContextManager.given
{
    public class a_command_context_manager
    {
        protected static CommandContextManager Manager;
        protected static Mock<IUncommittedEventStreamCoordinator> uncommitted_event_stream_coordinator;
        protected static Mock<IExecutionContextManager> execution_context_manager_mock;
        protected static Mock<IEventHandlersWaiters> event_handlers_waiters;
        protected static CommandContextFactory factory;
        protected static Mock<ILogger> logger;

        Establish context = () =>
                                        {
                                            CommandContextManager.ResetContext();
                                            uncommitted_event_stream_coordinator = new Mock<IUncommittedEventStreamCoordinator>();
                                            execution_context_manager_mock = new Mock<IExecutionContextManager>();
                                            event_handlers_waiters = new Mock<IEventHandlersWaiters>();
                                            logger = new Mock<ILogger>();

                                            factory = new CommandContextFactory(
                                                uncommitted_event_stream_coordinator.Object,
                                                execution_context_manager_mock.Object,
                                                event_handlers_waiters.Object,
                                                logger.Object);

                                            Manager = new CommandContextManager(factory);
                                        };
    }
}
