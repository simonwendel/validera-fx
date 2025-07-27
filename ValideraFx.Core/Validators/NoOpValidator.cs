using System.Diagnostics;

namespace ValideraFx.Core.Validators;

internal class NoOpValidator<T> : Validator<T> where T : notnull
{
    private protected override bool Valid(T value, string? name) => true;

    /// <remarks>
    /// The <see cref="NoOpValidator{T}" /> will never mark a result as invalid, which means it is not
    /// reasonable to create an error message from it.
    /// </remarks>
    /// <exception cref="UnreachableException">This method should never be called.</exception>
    private protected override string GetPartialMessage() => throw new UnreachableException();
}