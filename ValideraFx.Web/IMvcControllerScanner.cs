// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Web;

internal interface IMvcControllerScanner
{
    Dictionary<string, List<(string Name, Type Type)>> Scan();
}