﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Events;
using Dolittle.Execution;
using Machine.Specifications;

namespace Dolittle.Commands.Coordination.for_CommandContext
{
    public class when_committing : given.a_command_context_for_a_simple_command_with_one_tracked_object_with_one_uncommitted_event
    {
        static UncommittedEvents event_stream;

        Establish context = () => uncommitted_event_stream_coordinator.Setup(e => e.Commit(command_context.CorrelationId, Moq.It.IsAny<UncommittedAggregateEvents>())).Callback((CorrelationId i, UncommittedEvents s) => event_stream = s);

        Because of = () => command_context.Commit();

        It should_commit_on_the_uncommitted_event_stream_coordinator = () => event_stream.ShouldNotBeNull();
        It should_commit_on_the_uncommitted_event_stream_coordinator_with_the_event_in_event_stream = () => event_stream.ShouldContainOnly(uncommitted_event);
        It should_commit_aggregated_root = () => aggregated_root.CommitCalled.ShouldBeTrue();
    }
}
