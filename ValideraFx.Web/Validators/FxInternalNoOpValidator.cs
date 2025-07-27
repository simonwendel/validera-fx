using System.Diagnostics;
using ValideraFx.Core.Validators;

namespace ValideraFx.Web.Validators;

/// <remarks>This type should <b>never</b> be used outside the framework.</remarks>
internal class FxInternalNoOpValidator<T> : Validator<T> where T : notnull
{
    protected override bool Valid(T value, string? name) => true;

    /// <remarks>
    /// The <see cref="FxInternalNoOpValidator{T}" /> will never mark a result as invalid, which means it is not
    /// reasonable to create an error message from it.
    /// </remarks>
    /// <exception cref="UnreachableException">This method should never be called.</exception>
    protected override string GetPartialMessage() => throw new UnreachableException();
}