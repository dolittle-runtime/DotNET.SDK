/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dolittle.Artifacts.Configuration;

namespace Dolittle.Artifacts.Tools
{
    /// <summary>
    /// Represents a type of <see cref="Artifact"/>
    /// </summary>
    public class ArtifactType
    {
        /// <summary>
        /// Gets the type of <see cref="Artifact"/>
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets the human friendly type name
        /// </summary>
        public string TypeName { get; set; } 

        /// <summary>
        /// Gets the expression for accessing the collection of <see cref="ArtifactDefinition"/> on the configuration object
        /// </summary>
        public Expression<Func<ArtifactsByTypeDefinition, IEnumerable<ArtifactDefinition>>> TargetPropertyExpression { get; set; }
    }
}