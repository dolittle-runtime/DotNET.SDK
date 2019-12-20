﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Dolittle.Strings;
using Dolittle.Validation;
using Dolittle.Validation.MetaData;
using Machine.Specifications;

namespace Dolittle.FluentValidation.MetaData.for_ValidationMetaDataGenerator
{
    [Subject(typeof(ValidationMetaDataGenerator))]
    public class when_generating_for_a_command_with_concept_on_it_and_a_model_rule : given.a_validation_meta_data_generator_with_common_rules
    {
        static TypeMetaData result;

        Establish context = () =>
        {
            command_validator_provider_mock
                .Setup(m => m.GetInputValidatorFor(typeof(CommandWithConcept)))
                .Returns(new CommandWithConceptValidator());
        };

        Because of = () => result = generator.GenerateFor(typeof(CommandWithConcept));

        It should_have_required_for_string_concept = () => result["stringConcept"]["required"].ShouldNotBeNull();
        It should_have_required_for_long_concept = () => result["longConcept"]["required"].ShouldNotBeNull();
        It should_have_required_for_non_concept = () => result["nonConceptObject"]["notNull"].ShouldNotBeNull();
        It should_not_have_any_model_rules = () => result.Properties.Keys.Any(k => k.Contains(ModelRule<string>.ModelRulePropertyName.ToCamelCase())).ShouldBeFalse();
    }
}
