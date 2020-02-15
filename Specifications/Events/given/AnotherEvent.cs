// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Events;

namespace Dolittle.Events.given
{
    public class AnotherEvent : IEvent
    {
        public string Content { get; set; }
    }
}