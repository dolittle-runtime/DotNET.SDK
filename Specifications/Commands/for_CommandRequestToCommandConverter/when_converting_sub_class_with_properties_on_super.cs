// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Dolittle.Artifacts;
using Dolittle.Execution;
using Machine.Specifications;
using It = Machine.Specifications.It;

namespace Dolittle.Commands.for_CommandRequestToCommandConverter
{
    public class when_converting_sub_class_with_properties_on_super : given.a_converter
    {
        const int an_integer = 42;

        class super : ICommand
        {
            public int an_integer { get; set; }
        }

        class sub : super
        {
        }

        static CorrelationId correlation_id;
        static Artifact identifier;
        static CommandRequest request;

        static IDictionary<string, object> content;

        static sub result;

        Establish context = () =>
        {
            correlation_id = CorrelationId.New();
            identifier = Artifact.New();

            content = new Dictionary<string, object>
            {
                { "an_integer", an_integer }
            };

            request = new CommandRequest(correlation_id, identifier.Id, identifier.Generation, content);

            artifact_type_map.Setup(_ => _.GetTypeFor(identifier)).Returns(typeof(sub));
        };

        Because of = () => result = converter.Convert(request) as sub;

        It should_hold_an_integer = () => result.an_integer.ShouldEqual(an_integer);
    }
}