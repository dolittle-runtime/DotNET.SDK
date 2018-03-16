/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Artifacts;

namespace Dolittle.Events.Processing
{
    /// <summary>
    /// Represents a <see cref="IArtifactType">application resource type</see> for 
    /// <see cref="ICanProcessEvents">event processors</see>
    /// </summary>
    public class EventProcessorArtifactType : IArtifactTypeMapFor<ICanProcessEvents>
    {
        /// <inheritdoc/>
        public string Identifier => "EventProcessor";
    }
}