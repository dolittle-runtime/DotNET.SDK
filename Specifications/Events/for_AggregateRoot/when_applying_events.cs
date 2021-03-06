// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Events;
using Machine.Specifications;

namespace Dolittle.Domain.for_AggregateRoot
{
    public class when_applying_events : given.two_aggregate_roots
    {
        Because of = () =>
        {
            statefull_aggregate_root.Apply(event_one);
            statefull_aggregate_root.Apply(event_two);
            statefull_aggregate_root.Apply(event_three);
        };

        It should_be_at_version_three = () => statefull_aggregate_root.Version.ShouldEqual((AggregateRootVersion)3);
        It should_handle_simple_event_two_times = () => statefull_aggregate_root.SimpleEventOnCalled.ShouldEqual(2);
        It should_handle_another_event_one_time = () => statefull_aggregate_root.AnotherEventOnCalled.ShouldEqual(1);
        It should_have_no_broken_rules = () => statefull_aggregate_root.BrokenRules.ShouldBeEmpty();
        It should_have_no_rule_set_evaulations = () => statefull_aggregate_root.RuleSetEvaluations.ShouldBeEmpty();
    }
}
