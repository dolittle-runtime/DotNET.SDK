// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

extern alias contracts;

using Dolittle.Artifacts;
using Dolittle.Execution;
using Dolittle.Lifecycle;
using Dolittle.Logging;
using Dolittle.Runtime.Events;
using Dolittle.Runtime.Events.Coordination;
using Dolittle.Serialization.Json;
using Dolittle.Time;
using static contracts::Dolittle.Runtime.Events.EventStore;
using grpc = contracts::Dolittle.Runtime.Events;

namespace Dolittle.Events
{
    /// <summary>
    /// Represents a <see cref="IUncommittedEventStreamCoordinator"/>.
    /// </summary>
    /// <remarks>This implementation has been placed here temporarily.  It is not where it should be.</remarks>
    [Singleton]
    public class UncommittedEventStreamCoordinator : IUncommittedEventStreamCoordinator
    {
        readonly EventStoreClient _eventStoreClient;
        readonly ILogger _logger;
        readonly IArtifactTypeMap _artifactMap;
        readonly ISystemClock _systemClock;
        readonly IExecutionContextManager _executionContextManager;
        readonly ISerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="UncommittedEventStreamCoordinator"/> class.
        /// </summary>
        /// <param name="eventStoreClient">A see cref="EventStoreClient"/> for connecting to the runtime.</param>
        /// <param name="artifactMap">An instance of <see cref="IArtifactTypeMap" /> to get the artifact for Event Source and Events.</param>
        /// <param name="logger"><see cref="ILogger"/> for doing logging.</param>
        /// <param name="systemClock"><see cref="ISystemClock"/> for getting the time.</param>
        /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for accessing the current <see cref="ExecutionContext" />.</param>
        /// <param name="serializer">A JSON <see cref="ISerializer"/>.</param>
        public UncommittedEventStreamCoordinator(
            EventStoreClient eventStoreClient,
            IArtifactTypeMap artifactMap,
            ILogger logger,
            ISystemClock systemClock,
            IExecutionContextManager executionContextManager,
            ISerializer serializer)
        {
            _eventStoreClient = eventStoreClient;
            _logger = logger;
            _artifactMap = artifactMap;
            _systemClock = systemClock;
            _executionContextManager = executionContextManager;
            _serializer = serializer;
        }

        /// <inheritdoc/>
        public void Commit(CorrelationId correlationId, UncommittedEvents eventStream)
        {
            _logger.Information($"Committing uncommitted event stream with correlationId '{correlationId}'");
            var uncommittedAggregateEvents = new grpc.UncommittedAggregateEvents();
            _eventStoreClient.CommitForAggregate(uncommittedAggregateEvents);
        }
    }
}