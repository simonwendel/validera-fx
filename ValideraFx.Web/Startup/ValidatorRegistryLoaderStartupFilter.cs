// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace ValideraFx.Web.Startup;

public class ValidatorRegistryLoaderStartupFilter(IValidatorRegistry validatorRegistry) : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        ArgumentNullException.ThrowIfNull(next);
        return app =>
        {
            validatorRegistry.Load();
            next(app);
        };
    }
}