﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Runtime.Events;

namespace Dolittle.Domain
{
    /// <summary>
    /// Defines the basic functionality for finding and getting aggregated roots.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="AggregateRoot"/>.</typeparam>
    public interface IAggregateRootRepositoryFor<T>
        where T : class, IAggregateRoot
    {
        /// <summary>
        /// Get an aggregated root by id.
        /// </summary>
        /// <param name="id"><see cref="EventSourceId"/> of aggregated root to get.</param>
        /// <returns>An instance of the aggregated root.</returns>
        T Get(EventSourceId id);
    }
}
