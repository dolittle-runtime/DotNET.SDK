﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Commands;

namespace Dolittle.FluentValidation.MetaData.for_ValidationMetaDataGenerator
{
    public class CommandWithConcept : ICommand
    {
        public ConceptAsString StringConcept { get; set; }

        public ConceptAsLong LongConcept { get; set; }

        public object NonConceptObject { get; set; }
    }
}
