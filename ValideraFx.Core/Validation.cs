using ValideraFx.Core.Validators;

namespace ValideraFx.Core;

public static class Validation
{
    public static Validation<TValidated> Of<TValidated>() where TValidated : notnull
    {
        return new Validation<TValidated>();
    }
}

public class Validation<T> where T : notnull
{
    private readonly List<Validator<T>> validatorPipeline = [];
    
    public Validation<T> Apply<TProp>(System.Linq.Expressions.Expression<Func<T, TProp>> selector,
        params IValidator<TProp>[] validators) where TProp : notnull
    {
        var newValidators = validators.Select(v => new ObjectPropertyValidator<T, TProp>(selector, v));
        validatorPipeline.AddRange(newValidators);
        return this;
    }

    public IValidator<T> Build()
    {
        return new Pipeline<T>(validatorPipeline.ToArray());
    }
}