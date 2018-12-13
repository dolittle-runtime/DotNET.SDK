/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Collections.Generic;
using Dolittle.Applications;
using Dolittle.Applications.Configuration;
using Machine.Specifications;

namespace Dolittle.Build.Topology.for_Topology.for_TopologyBuilder.for_when_using_modules.for_when_stripping_one_namespace_segment.given
{
    public class a_configuration_without_topology_with_valid_artifacts_without_module : valid_artifacts_without_module
    {
        protected static readonly BoundedContextTopology configuration = new BoundedContextTopology(
            new Applications.Configuration.Topology(
                new Dictionary<Module,ModuleDefinition>(),
                new Dictionary<Feature,FeatureDefinition>()
            ), true, new Dictionary<Area, IEnumerable<string>>());

        protected static readonly TopologyBuilder topology_builder = new TopologyBuilder(artifacts, configuration, logger);

        Establish context = () => configuration.NamespaceSegmentsToStrip.Add("Specs", new []{"ToStrip"}); 
        
    }
}