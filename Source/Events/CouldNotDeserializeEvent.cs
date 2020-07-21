// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.Artifacts;
using Dolittle.Protobuf.Contracts;

namespace Dolittle.Events
{
    /// <summary>
    /// Exception that gets thrown when an <see cref="IEvent"/> couldn't be deserialized.
    /// </summary>s
    public class CouldNotDeserializeEvent : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CouldNotDeserializeEvent"/> class.
        /// </summary>
        /// <param name="artifact">The <see cref="ArtifactId"/> of the Artifact.</param>
        /// <param name="eventType">The Type of the event.</param>
        /// <param name="content">The content of the event.</param>
        /// <param name="scopeId">The scope of the event.</param>
        /// <param name="sequenceNumber">The Events position in the event log.</param>
        /// <param name="ex">The catched exception which made deserialization impossible in the first place.</param>
        public CouldNotDeserializeEvent(
            ArtifactId artifact,
            Type eventType,
            string content,
            Uuid scopeId,
            ulong sequenceNumber,
            Exception ex)
            : base($"Couldn't deserialize artifact '{artifact}' to event '{eventType}' in scope '{scopeId}' with EventLogSequenceNumber '{sequenceNumber}' and content '{content}'.{Environment.NewLine}Original occurred exception: {ex}")
        {
        }
    }
}
