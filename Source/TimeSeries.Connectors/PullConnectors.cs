// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

extern alias contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using contracts::Dolittle.Runtime.TimeSeries.Connectors;
using Dolittle.Collections;
using Dolittle.Lifecycle;
using Dolittle.Protobuf;
using Dolittle.TimeSeries.DataPoints;
using Dolittle.TimeSeries.DataTypes;
using Dolittle.Types;
using Grpc.Core;
using static contracts::Dolittle.Runtime.TimeSeries.Connectors.PullConnectors;

namespace Dolittle.TimeSeries.Connectors
{
    /// <summary>
    /// Represents an implementation of <see cref="IPullConnectors"/>.
    /// </summary>
    [Singleton]
    public class PullConnectors : IPullConnectors
    {
        readonly IDictionary<ConnectorId, IAmAPullConnector> _connectors;
        readonly PullConnectorsClient _pullConnectorsClient;
        readonly PullConnectorsConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="PullConnectors"/> class.
        /// </summary>
        /// <param name="pullConnectorsClient">The <see cref="PullConnectorsClient"/> for connecting to runtime.</param>
        /// <param name="connectors">Instances of <see cref="IAmAPullConnector"/>.</param>
        /// <param name="configuration"><see cref="PullConnectorsConfiguration"/> for configuring pull connectors.</param>
        public PullConnectors(
            PullConnectorsClient pullConnectorsClient,
            IInstancesOf<IAmAPullConnector> connectors,
            PullConnectorsConfiguration configuration)
        {
            _connectors = connectors.ToDictionary(_ => (ConnectorId)Guid.NewGuid(), _ => _);
            _pullConnectorsClient = pullConnectorsClient;
            _configuration = configuration;
        }

        /// <inheritdoc/>
        public IAmAPullConnector GetById(ConnectorId connectorId)
        {
            return _connectors[connectorId];
        }

        /// <inheritdoc/>
        public void Register()
        {
            _connectors.ForEach(_ =>
            {
                var interval = _configuration[_.Value.Name]?.Interval ?? 10000;

                var pullConnector = new PullConnector
                {
                    Id = _.Key.ToProtobuf(),
                    Name = _.Value.Name,
                    Interval = interval
                };

                Task.Run(async () =>
                {
                    try
                    {
                        var streamCall = _pullConnectorsClient.Connect(pullConnector);

                        while (await streamCall.ResponseStream.MoveNext().ConfigureAwait(false))
                        {
                            var pullRequest = streamCall.ResponseStream.Current;

                            var result = await _.Value.Pull().ConfigureAwait(false);
                            var tagDataPoints = result.Select(tagDataPoint => tagDataPoint.ToRuntime());
                            var writeMessage = new WriteMessage
                            {
                                ConnectorId = pullConnector.Id
                            };
                            writeMessage.Data.Add(tagDataPoints);
                            await _pullConnectorsClient.WriteAsync(writeMessage);
                        }
                    }
                    catch
                    {
                        Environment.Exit(1);
                    }
                });
            });
        }
    }
}