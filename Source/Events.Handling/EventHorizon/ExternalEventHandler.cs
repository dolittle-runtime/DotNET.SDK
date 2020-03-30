// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Dolittle.Applications;
using Dolittle.DependencyInversion;

namespace Dolittle.Events.Handling.EventHorizon
{
    /// <summary>
    /// Represents an implementation of <see cref="AbstractEventHandlerForEventHandlerType{T}" /> that can invoke events on instances of <see cref="ICanHandleExternalEvents"/>.
    /// </summary>
    public class ExternalEventHandler : AbstractEventHandlerForEventHandlerType<ICanHandleExternalEvents>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalEventHandler"/> class.
        /// </summary>
        /// <param name="container"><see cref="IContainer"/> for getting instances of <see cref="ICanHandleEvents"/>.</param>
        /// <param name="identifier">The unique <see cref="EventHandlerId">identifier</see>.</param>
        /// <param name="type"><see cref="Type"/> of <see cref="ICanHandleEvents"/>.</param>
        /// <param name="scope">The <see cref="ScopeId" />.</param>
        /// <param name="producerMicroservices">The list of <see cref ="Microservice" /> ids that produces the events that are handled.</param>
        /// <param name="methods"><see cref="IEnumerable{T}"/> of <see cref="EventHandlerMethod{T}"/>.</param>
        public ExternalEventHandler(IContainer container, EventHandlerId identifier, Type type, ScopeId scope, IEnumerable<Microservice> producerMicroservices, IEnumerable<IEventHandlerMethod> methods)
        : base(container, scope, identifier, type, false, methods)
        {
            ProducerMicroservices = producerMicroservices;
        }

        /// <summary>
        /// Gets the list of <see cref ="Microservice" /> ids that produces the events that are handled.
        /// </summary>
        public IEnumerable<Microservice> ProducerMicroservices { get; }
    }
}