// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Linq.Expressions;
using System.Reflection;
using ValideraFx.Core.Validators;

namespace ValideraFx.Core;

public static class Validation
{
    public static Validation<T> Of<T>() where T : notnull
    {
        return new Validation<T>();
    }
}

public class Validation<T> where T : notnull
{
    private readonly List<Validator<T>> validatorPipeline = [];
    private readonly List<ParameterExpression> parameterSelectors = [];
    private readonly List<MemberExpression> memberSelectors = [];

    public Validation<T> Apply<TProp>(Expression<Func<T, TProp>> selector,
        params IValidator<TProp>[] validators) where TProp : notnull
    {
        SaveSelectorForLater(selector);
        AddNewValidators(selector, validators);
        return this;
    }

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