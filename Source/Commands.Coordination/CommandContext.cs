﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Dolittle.Domain;
using Dolittle.Events;
using Dolittle.Events.Handling;
using Dolittle.Execution;
using Dolittle.Logging;

namespace Dolittle.Commands.Coordination
{
    /// <summary>
    /// Represents a <see cref="ICommandContext">ICommandContext</see>.
    /// </summary>
    public class CommandContext : ICommandContext
    {
        readonly IEventStore _eventStore;
        readonly IEventProcessingCompletion _eventProcessingCompletion;
        readonly List<AggregateRoot> _aggregateRootsTracked = new List<AggregateRoot>();

        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandContext"/> class.
        /// </summary>
        /// <param name="command">The <see cref="CommandRequest">command</see> the context is for.</param>
        /// <param name="executionContext">The <see cref="ExecutionContext"/> for the command.</param>
        /// <param name="eventStore">The <see cref="IEventStore"/> to use for committing events.</param>
        /// <param name="eventProcessingCompletion"><see cref="IEventProcessingCompletion"/> for waiting on event handlers.</param>
        /// <param name="logger"><see cref="ILogger"/> to use for logging.</param>
        public CommandContext(
            CommandRequest command,
            ExecutionContext executionContext,
            IEventStore eventStore,
            IEventProcessingCompletion eventProcessingCompletion,
            ILogger logger)
        {
            Command = command;
            ExecutionContext = executionContext;
            _eventStore = eventStore;
            _eventProcessingCompletion = eventProcessingCompletion;
            _logger = logger;
        }

        /// <inheritdoc/>
        public CommandRequest Command { get; }

        /// <inheritdoc/>
        public ExecutionContext ExecutionContext { get; }

        /// <inheritdoc/>
        public void RegisterForTracking(AggregateRoot aggregateRoot)
        {
            if (_aggregateRootsTracked.Contains(aggregateRoot)) return;
            _aggregateRootsTracked.Add(aggregateRoot);
        }

        /// <inheritdoc/>
        public IEnumerable<AggregateRoot> GetAggregateRootsBeingTracked() => _aggregateRootsTracked;

        /// <inheritdoc/>
        public void Dispose()
        {
            Commit();
        }

        /// <inheritdoc/>
        public void Commit()
        {
            _logger.Debug("Commit transaction for command {CommandType} with correlation {Correlation}", Command.Type, Command.CorrelationId);
            var trackedAggregateRoots = GetAggregateRootsBeingTracked();
            _logger.Trace("Total number of objects tracked '{Count}", trackedAggregateRoots.Count());
            foreach (var trackedAggregateRoot in trackedAggregateRoots)
            {
                _logger.Trace("Committing events from {TrackedAggregateRoot}", trackedAggregateRoot.GetType().AssemblyQualifiedName);
                var events = trackedAggregateRoot.UncommittedEvents;
                if (events.HasEvents)
                {
                    _eventProcessingCompletion.Perform(Command.CorrelationId, events, () =>
                    {
                        _logger.Trace("Events present - commit them to the event store");
                        _eventStore.CommitForAggregate(events).GetAwaiter().GetResult();
                        _logger.Trace("Commit object");
                        trackedAggregateRoot.Commit();
                    }).GetAwaiter().GetResult();
                }
            }
        }

        /// <inheritdoc/>
        public void Rollback()
        {
            // Todo : Should rollback any aggregated roots that are being tracked - this should really only be allowed to happen if we have not stored the events yet
            // once the events are stored, we can't roll back
        }
    }
}
