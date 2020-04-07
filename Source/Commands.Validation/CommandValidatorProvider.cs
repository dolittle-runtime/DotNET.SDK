﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dolittle.Collections;
using Dolittle.DependencyInversion;
using Dolittle.Lifecycle;
using Dolittle.Logging;
using Dolittle.Reflection;
using Dolittle.Types;
using Dolittle.Validation;
using FluentValidation;

namespace Dolittle.Commands.Validation
{
    /// <summary>
    /// Represents an instance of an <see cref="ICommandValidatorProvider">ICommandValidatorProvider.</see>.
    /// </summary>
    [Singleton]
    public class CommandValidatorProvider : ICommandValidatorProvider
    {
        static readonly Type _commandInputValidatorType = typeof(ICommandInputValidator);
        static readonly Type _commandBusinessValidatorType = typeof(ICommandBusinessValidator);
        static readonly Type _validatesType = typeof(ICanValidate<>);

        readonly ITypeFinder _typeFinder;
        readonly IContainer _container;
        readonly ILogger _logger;

        Dictionary<Type, Type> _inputCommandValidators;
        Dictionary<Type, Type> _businessCommandValidators;
        Dictionary<Type, List<Type>> _dynamicallyDiscoveredInputValidators = new Dictionary<Type, List<Type>>();
        Dictionary<Type, List<Type>> _dynamicallyDiscoveredBusinessValidators = new Dictionary<Type, List<Type>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandValidatorProvider"/> class.
        /// </summary>
        /// <param name="typeFinder">
        /// An instance of <see cref="ITypeFinder"/> to help identify and register <see cref="ICommandInputValidator"/> implementations
        /// and  <see cref="ICommandBusinessValidator"/> implementations.
        /// </param>
        /// <param name="container">An instance of <see cref="IContainer"/> to manage instances of any <see cref="ICommandInputValidator"/>.</param>
        /// <param name="logger">A <see cref="ILogger"/> for logging.</param>
        public CommandValidatorProvider(
            ITypeFinder typeFinder,
            IContainer container,
            ILogger logger)
        {
            _typeFinder = typeFinder;
            _container = container;
            _logger = logger;

            InitializeCommandValidators();
            InitializeDynamicValidators();
        }

        /// <summary>
        /// Gets a list of registered input validator types.
        /// </summary>
        public IEnumerable<Type> RegisteredInputCommandValidators => _inputCommandValidators.Values;

        /// <summary>
        ///  Gets a list of registered business validator types.
        /// </summary>
        public IEnumerable<Type> RegisteredBusinessCommandValidators => _businessCommandValidators.Values;

        /// <inheritdoc/>
        public ICommandInputValidator GetInputValidatorFor(ICommand command)
        {
            return GetInputValidatorFor(command.GetType());
        }

        /// <inheritdoc/>
        public ICommandBusinessValidator GetBusinessValidatorFor(ICommand command)
        {
            return GetBusinessValidatorFor(command.GetType());
        }

        /// <inheritdoc/>
        public ICommandBusinessValidator GetBusinessValidatorFor(Type commandType)
        {
            _logger.Debug($"Get business validator for : {commandType.AssemblyQualifiedName}");

            if (!typeof(ICommand).GetTypeInfo().IsAssignableFrom(commandType))
                return null;

            _businessCommandValidators.TryGetValue(commandType, out Type registeredBusinessValidatorType);
            var typesAndDiscoveredValidators = GetValidatorsFor(commandType, _dynamicallyDiscoveredBusinessValidators);
            var hasCrossCuttingValidators = typesAndDiscoveredValidators.Count > 0;

            if (registeredBusinessValidatorType != null)
            {
                _logger.Trace($"Validator for {commandType.AssemblyQualifiedName} found");
                var validator = _container.Get(registeredBusinessValidatorType) as ICommandBusinessValidator;

                if (hasCrossCuttingValidators && validator is IEnumerable<IValidationRule> enumerable)
                {
                    var dynamicValidator = BuildDynamicallyDiscoveredBusinessValidator(commandType, typesAndDiscoveredValidators);
                    var addRuleMethod = dynamicValidator.GetType().GetTypeInfo().GetMethod("AddRule", BindingFlags.Public | BindingFlags.Instance);
                    if (addRuleMethod != null)
                    {
                        enumerable.ForEach(rule =>
                        {
                            _logger.Trace($"Adding rule with ruleset '{rule.RuleSet}'");
                            addRuleMethod.Invoke(dynamicValidator, new[] { rule });
                        });
                    }

                    return dynamicValidator;
                }

                return validator;
            }

            _logger.Trace($"Building dynamic validator for {commandType.AssemblyQualifiedName}");
            return BuildDynamicallyDiscoveredBusinessValidator(commandType, typesAndDiscoveredValidators);
        }

        /// <inheritdoc/>
        public ICommandInputValidator GetInputValidatorFor(Type commandType)
        {
            _logger.Debug($"Get input validator for : {commandType.AssemblyQualifiedName}");

            if (!typeof(ICommand).GetTypeInfo().IsAssignableFrom(commandType))
                return null;

            _inputCommandValidators.TryGetValue(commandType, out Type registeredInputValidatorType);

            var typesAndDiscoveredValidators = GetValidatorsFor(commandType, _dynamicallyDiscoveredInputValidators);
            var hasCrossCuttingValidators = typesAndDiscoveredValidators.Count > 0;

            if (registeredInputValidatorType != null)
            {
                _logger.Trace($"Validator for {commandType.AssemblyQualifiedName} found");
                var validator = _container.Get(registeredInputValidatorType) as ICommandInputValidator;
                if (hasCrossCuttingValidators && validator is IEnumerable<IValidationRule> enumerable)
                {
                    var dynamicValidator = BuildDynamicallyDiscoveredInputValidator(commandType, typesAndDiscoveredValidators);
                    var addRuleMethod = dynamicValidator.GetType().GetTypeInfo().GetMethod("AddRule", BindingFlags.Public | BindingFlags.Instance);
                    if (addRuleMethod != null)
                    {
                        enumerable.ForEach(rule =>
                        {
                            _logger.Trace($"Adding rule with ruleset '{rule.RuleSet}'");
                            addRuleMethod.Invoke(dynamicValidator, new[] { rule });
                        });
                    }

                    return dynamicValidator;
                }

                return validator;
            }

            _logger.Trace($"Building dynamic validator for {commandType.AssemblyQualifiedName}");
            return BuildDynamicallyDiscoveredInputValidator(commandType, typesAndDiscoveredValidators);
        }

        Dictionary<Type, IEnumerable<Type>> GetValidatorsFor(Type commandType, Dictionary<Type, List<Type>> registeredTypes)
        {
            var typesOnCommand = GetTypesFromCommand(commandType).ToList();
            var validatorTypes = new Dictionary<Type, IEnumerable<Type>>();
            foreach (var typeToBeValidated in typesOnCommand)
            {
                if (registeredTypes.ContainsKey(typeToBeValidated))
                    validatorTypes.Add(typeToBeValidated, registeredTypes[typeToBeValidated]);
            }

            return validatorTypes;
        }

        IEnumerable<Type> GetTypesFromCommand(Type commandType)
        {
            var commandPropertyTypes = commandType.GetTypeInfo().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                                            .Where(p => !p.PropertyType.IsAPrimitiveType()).Select(p => p.PropertyType).Distinct();
            return commandPropertyTypes;
        }

        ICommandInputValidator BuildDynamicallyDiscoveredInputValidator(Type commandType, IDictionary<Type, IEnumerable<Type>> typeAndAssociatedValidatorTypes)
        {
            _logger.Trace($"Build dynamic input validator for {commandType.AssemblyQualifiedName}");
            Type[] typeArgs = { commandType };
            var closedValidatorType = typeof(ComposedCommandInputValidatorFor<>).MakeGenericType(typeArgs);

            var propertyTypeAndValidatorInstances = new Dictionary<Type, IEnumerable<IValidator>>();
            foreach (var key in typeAndAssociatedValidatorTypes.Keys)
            {
                var validatorTypes = typeAndAssociatedValidatorTypes[key];
                if (validatorTypes.Any())
                {
                    propertyTypeAndValidatorInstances.Add(key, validatorTypes.Select(v =>
                    {
                        _logger.Trace($"Adding validator {v.AssemblyQualifiedName}");
                        var validator = _container.Get(v) as IValidator;
                        return validator;
                    }).ToArray());
                }
            }

            _logger.Trace($"Create instance dynamic composed validator of type {closedValidatorType.AssemblyQualifiedName}");
            return Activator.CreateInstance(closedValidatorType, propertyTypeAndValidatorInstances) as ICommandInputValidator;
        }

        ICommandBusinessValidator BuildDynamicallyDiscoveredBusinessValidator(Type commandType, IDictionary<Type, IEnumerable<Type>> typeAndAssociatedValidatorTypes)
        {
            Type[] typeArgs = { commandType };
            var closedValidatorType = typeof(ComposedCommandBusinessValidatorFor<>).MakeGenericType(typeArgs);

            var propertyTypeAndValidatorInstances = new Dictionary<Type, IEnumerable<IValidator>>();
            foreach (var key in typeAndAssociatedValidatorTypes.Keys)
            {
                var validatorTypes = typeAndAssociatedValidatorTypes[key];
                if (validatorTypes.Any())
                    propertyTypeAndValidatorInstances.Add(key, validatorTypes.Select(v => _container.Get(v) as IValidator).ToArray());
            }

            return Activator.CreateInstance(closedValidatorType, propertyTypeAndValidatorInstances) as ICommandBusinessValidator;
        }

        void InitializeCommandValidators()
        {
            _inputCommandValidators = new Dictionary<Type, Type>();
            _businessCommandValidators = new Dictionary<Type, Type>();

            var commandInputValidators = _typeFinder.FindMultiple(_commandInputValidatorType);
            var commandBusinessValidators = _typeFinder.FindMultiple(_commandBusinessValidatorType);

            commandInputValidators.ForEach(type => RegisterCommandValidator(type, _inputCommandValidators));
            commandBusinessValidators.ForEach(type => RegisterCommandValidator(type, _businessCommandValidators));
        }

        void InitializeDynamicValidators()
        {
            _dynamicallyDiscoveredBusinessValidators = new Dictionary<Type, List<Type>>();
            _dynamicallyDiscoveredInputValidators = new Dictionary<Type, List<Type>>();

            var inputValidators = _typeFinder.FindMultiple(typeof(IValidateInput<>))
                .Where(t => t != typeof(InputValidator<>) && t != typeof(ComposedCommandInputValidatorFor<>));
            var businessValidators = _typeFinder.FindMultiple(typeof(IValidateBusinessRules<>))
                .Where(t => t != typeof(BusinessValidator<>) && t != typeof(ComposedCommandBusinessValidatorFor<>));

            inputValidators.ForEach(type => RegisterValidator(type, _dynamicallyDiscoveredInputValidators));
            businessValidators.ForEach(type => RegisterValidator(type, _dynamicallyDiscoveredBusinessValidators));
        }

        void RegisterCommandValidator(Type typeToRegister, IDictionary<Type, Type> validatorRegistry)
        {
            var commandType = GetCommandType(typeToRegister);

            if (commandType?.GetTypeInfo().IsInterface != false ||
                validatorRegistry.ContainsKey(commandType))
            {
                return;
            }

            _logger.Trace($"Registering input validator {typeToRegister.AssemblyQualifiedName} for {commandType.AssemblyQualifiedName}");

            validatorRegistry.Add(commandType, typeToRegister);
        }

        void RegisterValidator(Type typeToRegister, IDictionary<Type, List<Type>> validatorRegistry)
        {
            var validatedType = GetValidatedType(typeToRegister);

            if (validatedType?.GetTypeInfo().IsInterface != false ||
                validatedType.IsAPrimitiveType())
            {
                return;
            }

            _logger.Trace($"Registering validator of type {typeToRegister.AssemblyQualifiedName} for validation of {validatedType.AssemblyQualifiedName}");

            if (validatorRegistry.ContainsKey(validatedType))
            {
                validatorRegistry[validatedType].Add(typeToRegister);
            }
            else
            {
                validatorRegistry.Add(validatedType, new List<Type>() { typeToRegister });
            }
        }

        Type GetCommandType(Type typeToRegister)
        {
            var types = from interfaceType in typeToRegister.GetTypeInfo()
                                    .GetInterfaces()
                        where interfaceType.GetTypeInfo()
                                    .IsGenericType
                        let baseInterface = interfaceType.GetGenericTypeDefinition()
                        where baseInterface == _validatesType
                        select interfaceType.GetTypeInfo()
                                    .GetGenericArguments()
                            .FirstOrDefault();

            return types.FirstOrDefault();
        }

        Type GetValidatedType(Type typeToRegister)
        {
            Type validatedType = GetGenericParameterType(typeToRegister, typeof(IValidateInput<>));
            return validatedType ?? GetGenericParameterType(typeToRegister, typeof(IValidateBusinessRules<>));
        }

        Type GetGenericParameterType(Type typeToQuery, Type genericInterfaceType)
        {
            return (from @interface in typeToQuery.GetTypeInfo().GetInterfaces()
                    where @interface.GetTypeInfo().IsGenericType && @interface.GetTypeInfo().GetGenericTypeDefinition() == genericInterfaceType
                    select @interface.GetTypeInfo().GetGenericArguments()[0]).FirstOrDefault();
        }
    }
}