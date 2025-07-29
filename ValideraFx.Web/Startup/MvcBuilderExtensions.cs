// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using ValideraFx.Web.ModelBinding;

namespace ValideraFx.Web.Startup;

public static class MvcBuilderExtensions
{
    public static IMvcBuilder AddValideraFx(this IMvcBuilder builder, Action<ValidationOptions>? configure = null)
    {
        var options = ApplyConfiguration(builder, configure);
        AddGlobalValidationForMvc(builder, options);
        AddValidatorRegistry(builder, options);
        AddModelBinders(builder);
        return builder;
    }

    private static ValidationOptions ApplyConfiguration(IMvcBuilder builder, Action<ValidationOptions>? configure)
    {
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IConfigureOptions<ValidationOptions>, DefaultValidationOptionsSetup>());

        if (configure is not null)
        {
            builder.Services.Configure(configure);
        }

        return builder.Services.BuildServiceProvider().GetRequiredService<IOptions<ValidationOptions>>().Value;
    }

    private static void AddGlobalValidationForMvc(IMvcBuilder builder, ValidationOptions options)
    {
        if (options.EnforceValidModelState)
        {
            builder.Services.Configure<MvcOptions>(mvcOptions =>
                mvcOptions.Filters.Add(new AutoValidateModelStateForMvcOnlyFilter()));
        }
    }

    private static void AddValidatorRegistry(IMvcBuilder builder, ValidationOptions options)
    {
        builder.Services.AddSingleton<IValidatorRegistry>(new ValidatorRegistry(builder.Services));
    }

    private static void AddModelBinders(IMvcBuilder builder)
    {
        builder.Services.Configure<MvcOptions>(options =>
        {
            options.ModelBinderProviders.Insert(0, new ModelBinderProvider());
            options.ModelValidatorProviders.Insert(0, new ModelValidatorProvider());
            options.ModelMetadataDetailsProviders.Add(new ValidationMetadataProvider());
        });
    }
}