using Microsoft.Extensions.Options;

namespace ValideraFx.Web.Startup;

public class DefaultValidationOptionsSetup : IConfigureOptions<ValidationOptions>
{
    public void Configure(ValidationOptions options)
    {
        options.EnforceValidModelState = true;
        options.EagerLoadValidators = false;
    }
}