// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.Extensions.Options;

namespace ValideraFx.Web.Startup;

public class DefaultValidationOptionsSetup : IConfigureOptions<ValidationOptions>
{
    public void Configure(ValidationOptions options)
    {
        options.EnforceValidModelState = true;
    }
}