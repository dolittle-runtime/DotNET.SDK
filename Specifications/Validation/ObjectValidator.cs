﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Validation;
using FluentValidation;

namespace Dolittle.FluentValidation
{
    public class ObjectValidator : BusinessValidator<object>
    {
        public ObjectValidator()
        {
            ModelRule()
                .NotNull();
        }
    }
}
