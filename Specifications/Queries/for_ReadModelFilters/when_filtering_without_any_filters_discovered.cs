﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Dolittle.ReadModels;
using Machine.Specifications;

namespace Dolittle.Queries.for_ReadModelFilters
{
    public class when_filtering_without_any_filters_discovered : given.read_model_filters_without_any_filters
    {
        static IEnumerable<ReadModelWithString> items = new[]
        {
            new ReadModelWithString { Content = "Hello" },
            new ReadModelWithString { Content = "World" },
        };

        static IEnumerable<IReadModel> result = null;

        Because of = () => result = filters.Filter(items);

        It should_return_the_same_as_input = () => result.ShouldEqual(items);
    }
}
