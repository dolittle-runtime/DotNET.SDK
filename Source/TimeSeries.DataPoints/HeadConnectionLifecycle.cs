// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Dolittle.Heads;

namespace Dolittle.TimeSeries.DataPoints
{
    /// <summary>
    /// Represents the <see cref="ITakePartInHeadConnectionLifecycle">head procedure</see> for dealing
    /// with data points.
    /// </summary>
    public class HeadConnectionLifecycle : ITakePartInHeadConnectionLifecycle
    {
        readonly IDataPointsProcessors _processors;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadConnectionLifecycle"/> class.
        /// </summary>
        /// <param name="processors"><see cref="IDataPointsProcessors"/> in the system.</param>
        public HeadConnectionLifecycle(IDataPointsProcessors processors)
        {
            _processors = processors;
        }

        /// <inheritdoc/>
        public bool IsReady() => true;

        /// <inheritdoc/>
        public Task OnConnected(CancellationToken token)
        {
            _processors.Start();
            return new TaskCompletionSource<bool>().Task;
        }

        /// <inheritdoc/>
        public void OnDisconnected()
        {
        }
    }
}