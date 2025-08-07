// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Linq.Expressions;
using System.Reflection;
using ValideraFx.Core.Validators;

namespace ValideraFx.Core;

/// <summary>
/// Provides entry points for building custom validation pipelines.
/// </summary>
public static class Validation
{
    /// <summary>
    /// Starts a new validation builder for the specified type.
    /// </summary>
    /// <typeparam name="T">The type of value to validate.</typeparam>
    /// <returns>A new <see cref="Validation{T}"/> instance for composing validation rules.</returns>
    public static Validation<T> Of<T>() where T : notnull
    {
        return new Validation<T>();
    }
}

/// <summary>
/// A fluent builder for composing validators that apply to specific properties or fields of a type.
/// </summary>
/// <typeparam name="T">The type of value to validate.</typeparam>
public class Validation<T> where T : notnull
{
    private readonly List<Validator<T>> validatorPipeline = [];
    private readonly List<ParameterExpression> parameterSelectors = [];
    private readonly List<MemberExpression> memberSelectors = [];

    /// <summary>
    /// Applies one or more validators to a property or field of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="TProp">The type of the property being validated.</typeparam>
    /// <param name="selector">An expression that selects the property or field.</param>
    /// <param name="validators">One or more validators to apply to the selected member.</param>
    /// <returns>The same <see cref="Validation{T}"/> instance for chaining.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the selector is not a simple member access expression or parameter expression.
    /// </exception>
    public Validation<T> Apply<TProp>(Expression<Func<T, TProp>> selector,
        params IValidator<TProp>[] validators) where TProp : notnull
    {
        SaveSelectorForLater(selector);
        AddNewValidators(selector, validators);
        return this;
    }

    /// <summary>
    /// Builds a composed <see cref="IValidator{T}"/> from the configured pipeline.
    /// </summary>
    /// <param name="allowMissing">
    /// If <c>false</c>, the builder will enforce that all public properties and fields of <typeparamref name="T"/>
    /// are covered by validation. Set to <c>true</c> to allow partial validation. Default is <c>false</c>.
    /// </param>
    /// <returns>A composed <see cref="IValidator{T}"/> instance.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if no validators have been added, or if required members are not validated and
    /// <paramref name="allowMissing"/> is <c>false</c>.
    /// </exception>
    public IValidator<T> Build(bool allowMissing = false)
    {
        if (validatorPipeline.Count == 0)
        {
            throw new InvalidOperationException("No validators have been added to the validation builder.");
        }

        if (!allowMissing)
        {
            EnsureTypeIsProperlyValidated();
        }

        return new Pipeline<T>(validatorPipeline.ToArray());
    }

    private void SaveSelectorForLater<TProp>(Expression<Func<T, TProp>> selector)
    {
        var body = selector.Body;
        if (body is UnaryExpression u)
        {
            body = u.Operand;
        }

        switch (body)
        {
            case ParameterExpression parameterExpression:
                parameterSelectors.Add(parameterExpression);
                return;
            case MemberExpression memberExpression:
                memberSelectors.Add(memberExpression);
                return;
        }
        
        var bodyText = body.ToString();
        var expressionText = bodyText.StartsWith('(') && bodyText.EndsWith(')')
            ? bodyText[1..^1]
            : bodyText;
        throw new ArgumentException(
            $"The selector must be a member access or a bare parameter, but the expression is '{expressionText}'.");
    }

    private void AddNewValidators<TProp>(Expression<Func<T, TProp>> selector, IValidator<TProp>[] validators)
        where TProp : notnull
    {
        var newValidators = validators.Select(v => new ObjectValidator<T, TProp>(selector, v));
        validatorPipeline.AddRange(newValidators);
    }

    private void EnsureTypeIsProperlyValidated()
    {
        var validationForTypeItself = parameterSelectors.Any(x => x.Type == typeof(T));
        var missingProperties = GetMissingMembers();
        if (!validationForTypeItself && missingProperties.Count > 0)
        {
            throw new InvalidOperationException(
                $"Not all properties or fields of {typeof(T).Name} are validated. Validation is missing for '{string.Join(", ", missingProperties)}'.");
        }
    }

    private List<string> GetMissingMembers()
    {
        var selectedNames = memberSelectors.Select(x => x.Member.Name).ToHashSet();

        const BindingFlags attributes = BindingFlags.Instance | BindingFlags.Public;
        var allProps = typeof(T).GetProperties(attributes).Select(x => x.Name);
        var allFields = typeof(T).GetFields(attributes).Select(x => x.Name);

        var allMembers = allProps.Concat(allFields);
        return allMembers.Where(x => !selectedNames.Contains(x)).OrderBy(x => x).ToList();
    }
}