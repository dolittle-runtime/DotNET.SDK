﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dolittle.Commands.Validation;
using Dolittle.Concepts;
using Dolittle.Strings;
using Dolittle.Types;
using FluentValidation;
using FluentValidation.Validators;

namespace Dolittle.Validation.MetaData
{
    /// <summary>
    /// Represents an implementation of <see cref="ICanGenerateValidationMetaData"/>.
    /// </summary>
    public class ValidationMetaDataGenerator : ICanGenerateValidationMetaData
    {
        readonly ICommandValidatorProvider _validatorProvider;
        readonly Dictionary<Type, ICanGenerateRule> _generatorsByType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationMetaDataGenerator"/> class.
        /// </summary>
        /// <param name="ruleGenerators">The known instances of generators.</param>
        /// <param name="validatorProvider">The provider of command input validators.</param>
        public ValidationMetaDataGenerator(IInstancesOf<ICanGenerateRule> ruleGenerators, ICommandValidatorProvider validatorProvider)
        {
            _validatorProvider = validatorProvider;
            _generatorsByType = Generators(ruleGenerators);
        }

        /// <inheritdoc/>
        public TypeMetaData GenerateFor(Type typeForValidation)
        {
            var metaData = new TypeMetaData();

            var validator = _validatorProvider.GetInputValidatorFor(typeForValidation);
            GenerateForValidator(validator, metaData, string.Empty);

            return metaData;
        }

        void GenerateForValidator(IValidator inputValidator, TypeMetaData metaData, string parentKey, bool isParentConcept = false, bool isParentModelRule = false)
        {
            var inputValidatorType = inputValidator.GetType();
            var genericArguments = inputValidatorType.GetTypeInfo().BaseType.GetTypeInfo().GetGenericArguments();

            var descriptor = inputValidator.CreateDescriptor();
            foreach (var member in descriptor.GetMembersWithValidators())
            {
                foreach (var rule in descriptor.GetRulesForMember(member.Key))
                {
                    foreach (var validator in rule.Validators)
                    {
                        var isModelRule = member.Key == ModelRule<string>.ModelRulePropertyName;
                        var currentKey = GetKeyForMember(parentKey, isParentConcept, isParentModelRule, member, isModelRule);

                        if (validator is ChildValidatorAdaptor)
                        {
                            GenerateForChildValidator(metaData, genericArguments, member, validator, isModelRule, currentKey);
                        }
                        else if (validator is IPropertyValidator)
                        {
                            GenerateFor(metaData, currentKey, validator);
                        }
                    }
                }
            }
        }

        string GetKeyForMember(string parentKey, bool isParentConcept, bool isParentModelRule, IGrouping<string, IPropertyValidator> member, bool isModelRule)
        {
            string currentKey;
            if (isParentConcept || isParentModelRule || isModelRule)
                currentKey = parentKey;
            else
                currentKey = string.IsNullOrEmpty(parentKey) ? member.Key : $"{parentKey}.{member.Key.ToCamelCase()}";

            if (currentKey.EndsWith(".", StringComparison.InvariantCulture)) currentKey = currentKey.Substring(0, currentKey.Length - 1);
            return currentKey;
        }

        void GenerateForChildValidator(TypeMetaData metaData, Type[] genericArguments, IGrouping<string, IPropertyValidator> member, IPropertyValidator validator, bool isModelRule, string currentKey)
        {
            var isConcept = false;

            if (genericArguments.Length == 1)
            {
                // TODO: This is probably needed because of a bug in FluentValidation.
                //       At least it seems like that. It seems there is some static cached state.
                //       It works fine in isolated tests - but running the whole battery
                //       makes this fail if we take out the "Value" name forcing
                var memberKey = member.Key;
                if (memberKey?.Length == 0) memberKey = "Value";

                var type = isModelRule ? genericArguments[0] : GetPropertyInfo(genericArguments[0], memberKey).PropertyType;
                isConcept = type.IsConcept();
            }

            var propertyValidatorContext = new PropertyValidatorContext(new ValidationContext(null), null, null);
            var childValidator = (validator as ChildValidatorAdaptor).GetValidator(propertyValidatorContext);
            GenerateForValidator(childValidator, metaData, currentKey, isConcept, isModelRule);
        }

        void GenerateFor(TypeMetaData metaData, string property, IPropertyValidator validator)
        {
            var validatorType = validator.GetType();
            var types = new List<Type>
            {
                validatorType
            };
            types.AddRange(validatorType.GetTypeInfo().GetInterfaces());
            foreach (var type in types)
            {
                if (_generatorsByType.ContainsKey(type))
                {
                    var propertyName = property.ToCamelCase();
                    var rule = _generatorsByType[type].GeneratorFrom(property, validator);
                    var ruleName = rule.GetType().Name.ToCamelCase();
                    metaData[propertyName][ruleName] = rule;
                }
            }
        }

        Dictionary<Type, ICanGenerateRule> Generators(IInstancesOf<ICanGenerateRule> ruleGenerators)
        {
            return (
                from generator in ruleGenerators
                from type in generator.From
                select new { generator, type })
                .ToDictionary(d => d.type, d => d.generator);
        }

        PropertyInfo GetPropertyInfo(Type type, string name)
        {
            return type.GetTypeInfo().GetProperty(name);
        }
    }
}
