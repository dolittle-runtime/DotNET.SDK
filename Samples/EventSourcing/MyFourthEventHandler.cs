// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Dolittle.Events.Handling;
using Dolittle.Logging;

namespace EventSourcing
{
    [EventHandler("9985b24e-1e9f-4fc0-bb37-92c95b2bce36")]
    public class MyFourthEventHandler : ICanHandleEvents
    {
        readonly ILogger _logger;

        public MyFourthEventHandler(ILogger logger)
        {
            _logger = logger;
        }

        public Task Handle(MyEvent @event)
        {
            _logger.Information($"Processing event : '{@event}'");
            return Task.CompletedTask;
        }

        public Task Handle(MySecondEvent @event)
        {
            _logger.Information($"Processing event : '{@event}'");
            return Task.CompletedTask;
        }
    }
}