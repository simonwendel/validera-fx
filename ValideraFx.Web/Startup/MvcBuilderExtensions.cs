// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using ValideraFx.Web.ModelBinding;

namespace ValideraFx.Web.Startup;

/// <summary>
/// Provides extension methods for configuring ValideraFx integration with ASP.NET Core.
/// </summary>
public static class MvcBuilderExtensions
{
    /// <summary>
    /// Adds ValideraFx validation services and configuration to the ASP.NET Core pipeline.
    /// </summary>
    /// <param name="builder">The MVC builder to configure.</param>
    /// <param name="configure">
    /// An optional delegate to configure <see cref="ValidationOptions"/> at startup.
    /// </param>
    /// <returns>
    /// The same <see cref="IMvcBuilder"/> instance so that multiple calls can be chained.
    /// </returns>
    public static IMvcBuilder AddValideraFx(this IMvcBuilder builder, Action<ValidationOptions>? configure = null)
    {
        var options = ApplyConfiguration(builder, configure);
        AddGlobalValidationForMvc(builder, options);
        AddStartupFilters(builder, options);
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

    private static void AddStartupFilters(IMvcBuilder builder, ValidationOptions options)
    {
        builder.Services.AddSingleton<IStartupFilter, ValidatorRegistryLoaderStartupFilter>();
        
        if (!options.EnforceValidatedTypes)
        {
            return;
        }

        builder.Services.AddSingleton<IMvcControllerScanner, MvcControllerScanner>();
        builder.Services.AddSingleton<IStartupFilter, ControllerScannerStartupFilter>();
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