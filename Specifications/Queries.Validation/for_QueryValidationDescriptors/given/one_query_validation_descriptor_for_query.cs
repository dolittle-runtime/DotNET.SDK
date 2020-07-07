﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Machine.Specifications;

namespace Dolittle.Queries.Validation.Specs.for_QueryValidationDescriptors.given
{
    public class one_query_validation_descriptor_for_query : all_dependencies
    {
        protected static QueryValidationDescriptors descriptors;
        protected static SimpleQueryDescriptor descriptor;

        Establish context = () =>
        {
            descriptor = new SimpleQueryDescriptor();
            type_finder.Setup(t => t.FindMultiple(typeof(QueryValidationDescriptorFor<>))).Returns(new Type[] { typeof(SimpleQueryDescriptor) });
            container.Setup(c => c.Get(typeof(SimpleQueryDescriptor))).Returns(descriptor);
            descriptors = new QueryValidationDescriptors(type_finder.Object, container.Object);
        };
    }
}
