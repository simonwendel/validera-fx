// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ValideraFx.Web;

public class MvcControllerScanner : IMvcControllerScanner
{
    public Dictionary<string, List<(string Name, Type Type)>> Scan()
    {
        var manager = new ApplicationPartManager();
        manager.FeatureProviders.Add(new ControllerFeatureProvider());

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()
                     .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location)))
        {
            manager.ApplicationParts.Add(new AssemblyPart(assembly));
        }

        var feature = new ControllerFeature();
        manager.PopulateFeature(feature);

        var result = new Dictionary<string, List<(string Name, Type Type)>>();

        foreach (var controllerType in feature.Controllers.Select(t => t.AsType()))
        {
            var controllerName = controllerType.Name;
            var methods = controllerType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m =>
                    !m.IsSpecialName &&
                    !m.IsDefined(typeof(NonActionAttribute)));

            foreach (var method in methods)
            {
                var key = $"{controllerName}.{method.Name}";
                var parameters = method.GetParameters()
                    .Select(p => (p.Name ?? "<unknown>", p.ParameterType))
                    .ToList();

                result[key] = parameters;
            }
        }

        return result;
    }
}