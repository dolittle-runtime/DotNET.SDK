using System;
using Dolittle.Commands;
using Dolittle.Events;
using Dolittle.Types;
using Machine.Specifications;
using Moq;

namespace Dolittle.Applications.for_ApplicationArtifactsResolver.given
{
    public class a_aai_to_type_map_for_Commands : a_system_for_finding_Commands
    {
        protected static IApplicationArtifactIdentifierToTypeMaps aai_to_type_maps;
        protected static Mock<ITypeFinder> type_finder_for_aai_to_type_maps;
        Establish context = () =>
        {
            type_finder_for_aai_to_type_maps = new Mock<ITypeFinder>();
            type_finder_for_aai_to_type_maps.Setup(_ => _.FindMultiple(typeof(ICommand))).Returns(new Type[] {typeof(ACommand)});
            type_finder_for_aai_to_type_maps.Setup(_ => _.FindMultiple(typeof(IEvent))).Returns(new Type[] {typeof(AnEvent)});

            aai_to_type_maps = new ApplicationArtifactIdentifierToTypeMaps(application_configuration.application, location_resolver, artifact_type_to_type_maps, type_finder_for_aai_to_type_maps.Object);
        }; 
    }
}