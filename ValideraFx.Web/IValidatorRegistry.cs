// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Web;

public interface IValidatorRegistry
{
    void PopulateRegistry();
    
    object? GetValidatorFor(Type type, IServiceProvider serviceProvider);
}