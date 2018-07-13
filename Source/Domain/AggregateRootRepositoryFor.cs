﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Reflection;
using Dolittle.Logging;
using Dolittle.Events;
using Dolittle.Runtime.Events;
using Dolittle.Runtime.Events.Storage;
using Dolittle.Runtime.Commands.Coordination;
using Dolittle.Applications;

namespace Dolittle.Domain
{
    /// <summary>
    /// Defines a concrete implementation of <see cref="IAggregateRootRepositoryFor{T}">IAggregatedRootRepository</see>
    /// </summary>
    /// <typeparam name="T">Type the repository is for</typeparam>
    public class AggregateRootRepositoryFor<T> : IAggregateRootRepositoryFor<T>
        where T : AggregateRoot
    {
        ICommandContextManager _commandContextManager;
        IEventStore _eventStore;
        IEventSourceVersions _eventSourceVersions;
        IApplicationArtifactIdentifierToTypeMaps _aaiToTypeMaps;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="AggregateRootRepositoryFor{T}">AggregatedRootRepository</see>
        /// </summary>
        /// <param name="commandContextManager"> <see cref="ICommandContextManager"/> to use for tracking </param>
        /// <param name="eventStore"><see cref="IEventStore"/> for getting <see cref="IEvent">events</see></param>
        /// <param name="eventSourceVersions"><see cref="IEventSourceVersions"/> for working with versioning of <see cref="AggregateRoot"/></param>
        /// <param name="aaiToTypeMaps"><see cref="IApplicationArtifactIdentifierToTypeMaps"/> for being able to identify resources</param>
        /// <param name="logger"><see cref="ILogger"/> to use for logging</param>
        public AggregateRootRepositoryFor(
            ICommandContextManager commandContextManager,
            IEventStore eventStore,
            IEventSourceVersions eventSourceVersions,
            IApplicationArtifactIdentifierToTypeMaps aaiToTypeMaps, 
            ILogger logger)
        {
            _commandContextManager = commandContextManager;
            _eventStore = eventStore;
            _eventSourceVersions = eventSourceVersions;
            _aaiToTypeMaps = aaiToTypeMaps;
            _logger = logger;
        }

        /// <inheritdoc/>
		public T Get(EventSourceId id)
        {
            _logger.Trace($"Get '{typeof(T).AssemblyQualifiedName}' with Id of '{id?.Value.ToString() ?? "<unknown id>"}'");

            var commandContext = _commandContextManager.GetCurrent();
            var type = typeof(T);
            var constructor = GetConstructorFor(type);
            ThrowIfConstructorIsInvalid(type, constructor);

            var aggregateRoot = GetInstanceFrom(id, constructor);
            if (null != aggregateRoot)
            {
                if (!aggregateRoot.IsStateless())
                    ReApplyEvents(commandContext, aggregateRoot);
                else
                    FastForward(commandContext, aggregateRoot);
            }
            commandContext.RegisterForTracking(aggregateRoot);

            return aggregateRoot;
        }

        void FastForward(ICommandContext commandContext, T aggregateRoot)
        {
            _logger.Trace($"FastForward - {typeof(T).AssemblyQualifiedName}");
            var identifier = _aaiToTypeMaps.Map(typeof(T));
            _logger.Trace($"With identifier '{identifier?.ToString()??"<unknown identifier>"}'");
            
            var version = _eventSourceVersions.GetFor(identifier, aggregateRoot.EventSourceId);
            aggregateRoot.FastForward(version);
        }

        void ReApplyEvents(ICommandContext commandContext, T aggregateRoot)
        {
            var identifier = _aaiToTypeMaps.Map(typeof(T));
            var events = _eventStore.GetFor(identifier, aggregateRoot.EventSourceId);
            var stream = new CommittedEventStream(aggregateRoot.EventSourceId, events);
            if (stream.HasEvents)
                aggregateRoot.ReApply(stream);
        }

        T GetInstanceFrom(EventSourceId id, ConstructorInfo constructor)
        {
            return (constructor.GetParameters()[0].ParameterType == typeof(EventSourceId) ?
                constructor.Invoke(new object[] { id }) :
                constructor.Invoke(new object[] { id.Value })) as T;
        }

        ConstructorInfo GetConstructorFor(Type type)
        {
            return type.GetTypeInfo().GetConstructors().Where(c =>
            {
                var parameters = c.GetParameters();
                return parameters.Length == 1 &&
                    (parameters[0].ParameterType == typeof(Guid) ||
                    parameters[0].ParameterType == typeof(EventSourceId));
            }).SingleOrDefault();
        }

        void ThrowIfConstructorIsInvalid(Type type, ConstructorInfo constructor)
        {
            if (constructor == null) throw new InvalidAggregateRootConstructorSignature(type);
        }
    }
}