// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dolittle.Events
{
    /// <summary>
    /// Represents a sequence of <see cref="IEvent"/>s applied by an AggregateRoot to an Event Source that have not been committed to the Event Store.
    /// </summary>
    public class UncommittedAggregateEvents : UncommittedEvents
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UncommittedAggregateEvents"/> class.
        /// </summary>
        /// <param name="eventSource">The Event Source that the uncommitted events was applied to.</param>
        /// <param name="aggregateRoot">The <see cref="Type"/> of the Aggregate Root that applied the events to the Event Source.</param>
        /// <param name="expectedAggregateRootVersion">The <see cref="AggregateRootVersion"/> of the Aggregate Root that was used to apply the rules that resulted in the Events.</param>
        public UncommittedAggregateEvents(EventSourceId eventSource, Type aggregateRoot, AggregateRootVersion expectedAggregateRootVersion)
        {
            EventSource = eventSource;
            AggregateRoot = aggregateRoot;
            ExpectedAggregateRootVersion = expectedAggregateRootVersion;
        }

        /// <summary>
        /// Gets the Event Source that the uncommitted events was applied to.
        /// </summary>
        public EventSourceId EventSource { get; }

        /// <summary>
        /// Gets the <see cref="Type"/> of the Aggregate Root that applied the events to the Event Source.
        /// </summary>
        public Type AggregateRoot { get; }

        /// <summary>
        /// Gets the <see cref="AggregateRootVersion"/> of the Aggregate Root that was used to apply the rules that resulted in the Events.
        /// The events can only be committed to the Event Store if the version of Aggregate Root has not changed.
        /// </summary>
        public AggregateRootVersion ExpectedAggregateRootVersion { get; }
    }
}