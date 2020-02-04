// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Machine.Specifications;
using Moq;

using Specs.Feature;
using IAssemblyContext = Dolittle.Assemblies.IAssemblyContext;

namespace Dolittle.Build.for_ArtifactsDiscoverer.given
{
    public class all_generic_artifacts_and_their_sub_types : given.all
    {
        protected static readonly IEnumerable<Type> generic_types = new[]
        {
            typeof(GenericCommand<>),
            typeof(GenericEvent<>),
            typeof(GenericEventSource<>),
            typeof(GenericQuery<>),
            typeof(GenericReadModel<>),
        };

        protected static readonly IEnumerable<Type> non_generic_subtypes_of_the_generic_types = new[]
        {
            typeof(ImplementationOfGenericCommand),
            typeof(ImplementationOfGenericEvent),
            typeof(ImplementationOfGenericEventSource),
            typeof(ImplementationOfGenericQuery),
            typeof(ImplementationOfGenericReadModel),
        };

        protected static Mock<IAssemblyContext> assemvly_context;

        Establish context = () =>
        {
            assemvly_context = new Mock<IAssemblyContext>();

            var assembly_mock = new Mock<Assembly>();
            assembly_mock.Setup(_ => _.ExportedTypes).Returns(generic_types.Concat(non_generic_subtypes_of_the_generic_types));
            assemvly_context.Setup(_ => _.GetProjectReferencedAssemblies()).Returns(new[] { assembly_mock.Object });
        };
    }
}