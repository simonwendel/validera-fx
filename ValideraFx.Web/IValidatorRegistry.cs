// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Web;

internal interface IValidatorRegistry
{
    void Load();
    
    object? GetValidatorFor(Type type, IServiceProvider serviceProvider, ValidationOptions options);
}