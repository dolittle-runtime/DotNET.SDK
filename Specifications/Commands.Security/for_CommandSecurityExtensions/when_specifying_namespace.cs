﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Dolittle.Security;
using Machine.Specifications;
using It = Machine.Specifications.It;

namespace Dolittle.Commands.Security.Specs.for_CommandSecurityExtensions
{
    public class when_specifying_namespace
    {
        static CommandSecurityTarget target;
        static NamespaceSecurable securable;

        Establish context = () => target = new CommandSecurityTarget();

        Because of = () => target.InNamespace("MyNamespace", n => securable = n);

        It should_add_a_namespace_securable = () => target.Securables.First().ShouldBeOfExactType<NamespaceSecurable>();
        It should_set_the_name_of_the_namespace_on_the_securable = () => ((NamespaceSecurable)target.Securables.First()).Namespace.ShouldEqual("MyNamespace");
        It should_continue_the_fluent_interface_with_namespace_securable_builder = () => securable.ShouldNotBeNull();
    }
}
