// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using ValideraFx.Core;

namespace ValideraFx.Web.Startup;

public class ControllerScannerStartupFilter(IMvcControllerScanner controllerScanner) : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        ArgumentNullException.ThrowIfNull(next);
        return app =>
        {
            var offendingParameters = GetOffendingActionsAndParameters();
            if (offendingParameters.Length != 0)
            {
                var message = BuildErrorMessage(offendingParameters);
                throw new InvalidOperationException(message);
            }

            next(app);
        };
    }

    private KeyValuePair<string, List<(string Name, Type Type)>>[] GetOffendingActionsAndParameters() =>
        controllerScanner.Scan().Where(x => x.Value.Any(IsOffendingParameter)).ToArray();

    private static bool IsOffendingParameter((string, Type) element)
    {
        var (_, type) = element;
        if (!type.IsGenericType)
        {
            return true;
        }

        return type.GetGenericTypeDefinition() != typeof(UntrustedValue<>) &&
               type.GetGenericTypeDefinition() != typeof(TrustedValue<>);
    }

    private static string BuildErrorMessage(KeyValuePair<string, List<(string Name, Type Type)>>[] offendingParameters)
    {
        var message = new StringBuilder();
        message.AppendLine("The following controller actions have parameters that are not handled by ValideraFx:");
        message.AppendLine();
        foreach (var (action, parameters) in offendingParameters)
        {
            message.AppendLine($"Action: {action}");
            foreach (var (name, type) in parameters)
            {
                message.Append($"  Parameter: {name} ({type.FullName})");
                if (IsOffendingParameter((name, type)))
                {
                    message.Append(" <--");
                }

                message.AppendLine();
            }

            message.AppendLine();
        }

        return message.ToString();
    }
}