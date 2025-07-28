using Microsoft.Extensions.Options;

namespace ValideraFx.Web;

public class DefaultValidationOptionsSetup : IConfigureOptions<ValidationOptions>
{
    public void Configure(ValidationOptions options)
    {
        options.EnforceValidModelState = true;
    }
}