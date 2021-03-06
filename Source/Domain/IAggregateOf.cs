// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Events;

namespace Dolittle.Domain
{
    /// <summary>
    /// Defines a way to work with <see cref="AggregateRoot"/>.
    /// </summary>
    /// <typeparam name="TAggregate"><see cref="AggregateRoot"/> type.</typeparam>
    public interface IAggregateOf<TAggregate>
        where TAggregate : AggregateRoot
    {
        /// <summary>
        /// Create a new <see cref="AggregateRoot"/> with a new <see cref="EventSourceId"/>.
        /// </summary>
        /// <returns><see cref="IAggregateRootOperations{T}">Operations</see> that can be performed.</returns>
        IAggregateRootOperations<TAggregate> Create();

        /// <summary>
        /// Create a new <see cref="AggregateRoot"/> with a given <see cref="EventSourceId"/>.
        /// </summary>
        /// <param name="eventSourceId"><see cref="EventSourceId"/> of the <see cref="AggregateRoot"/>.</param>
        /// <returns><see cref="IAggregateRootOperations{T}">Operations</see> that can be performed.</returns>
        IAggregateRootOperations<TAggregate> Create(EventSourceId eventSourceId);

        /// <summary>
        /// Rehydrate an existing <see cref="AggregateRoot"/> with a given <see cref="EventSourceId"/>.
        /// </summary>
        /// <param name="eventSourceId"><see cref="EventSourceId"/> of the <see cref="AggregateRoot"/>.</param>
        /// <returns><see cref="IAggregateRootOperations{T}">Operations</see> that can be performed.</returns>
        IAggregateRootOperations<TAggregate> Rehydrate(EventSourceId eventSourceId);
    }
}
