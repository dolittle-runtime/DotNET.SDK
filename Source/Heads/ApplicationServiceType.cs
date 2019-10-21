/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Services;

namespace Dolittle.Heads
{
    /// <summary>
    /// Represents a <see cref="IRepresentServiceType">host type</see> that is for application client communication
    /// </summary>
    /// <remarks>
    /// Application client is considered the channel in which a runtime connects - application client is considered
    /// the representation of the application and is usually represented through an SDK
    /// </remarks>
    public class ApplicationServiceType : IRepresentServiceType
    {
        /// <summary>
        /// Gets the name of the <see cref="ServiceType"/> for application client
        /// </summary>
        internal const string ServiceType = "Application";

        readonly HeadPort _port;

        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationServiceType"/>
        /// </summary>
        /// <param name="port"><see cref="HeadPort"/> to expose</param>
        public ApplicationServiceType(HeadPort port)
        {
            _port = port;
        }

        /// <inheritdoc/>
        public ServiceType Identifier => ServiceType;

        /// <inheritdoc/>
        public Type BindingInterface => typeof(ICanBindHeadServices);

        /// <inheritdoc/>
        public EndpointVisibility Visibility => EndpointVisibility.Private;
    }
}