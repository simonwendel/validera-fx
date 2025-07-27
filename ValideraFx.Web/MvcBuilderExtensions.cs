// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ValideraFx.Web.ModelBinding;

namespace ValideraFx.Web;

public static class MvcBuilderExtensions
{
    public static IMvcBuilder AddValideraFx(this IMvcBuilder builder)
    {
        builder.Services.Configure<MvcOptions>(options =>
        {
            options.ModelBinderProviders.Insert(0, new ModelBinderProvider());
            options.ModelValidatorProviders.Insert(0, new ModelValidatorProvider());
            options.ModelMetadataDetailsProviders.Add(new ValidationMetadataProvider());
        });

        return builder;
    }
}