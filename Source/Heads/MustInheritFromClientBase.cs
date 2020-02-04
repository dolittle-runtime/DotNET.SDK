// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Grpc.Core;

namespace Dolittle.Heads
{
    /// <summary>
    /// Exception that gets thrown when a <see cref="Type"/> does not implement <see cref="ClientBase{T}"/>.
    /// </summary>
    public class MustInheritFromClientBase : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MustInheritFromClientBase"/> class.
        /// </summary>
        /// <param name="type"><see cref="Type"/> that must inherit <see cref="ClientBase{T}"/>.</param>
        public MustInheritFromClientBase(Type type)
            : base($"Type '{type.AssemblyQualifiedName}' does inherit {typeof(ClientBase<>).AssemblyQualifiedName}")
        {
        }
    }
}