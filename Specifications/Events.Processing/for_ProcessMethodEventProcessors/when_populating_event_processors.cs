﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Dolittle.Time;
using Machine.Specifications;
using Moq;
using It=Machine.Specifications.It;

namespace Dolittle.Events.Processing.for_ProcessMethodEventProcessors
{
    [Subject(typeof(ProcessMethodEventProcessors), nameof(ProcessMethodEventProcessors.Populate))]
    public class when_populating_event_processors : given.event_processors
    {
        static ProcessMethodEventProcessors event_processors;

        Establish context = () => event_processors = new ProcessMethodEventProcessors(artifact_type_map.Object, object_factory.Object, implementations.Object, container.Object, Mock.Of<ISystemClock>(), logger.Object);

        Because of = () => event_processors.Populate();

        It should_generate_an_event_processor_for_each_event_processing_method = () => event_processors.Count().ShouldEqual(6);
        It should_identify_each_event_processor_with_the_id_of_the_method = () => event_processors.Select(_ => _.Identifier).ShouldContainOnly(event_processor_ids);
    }
}