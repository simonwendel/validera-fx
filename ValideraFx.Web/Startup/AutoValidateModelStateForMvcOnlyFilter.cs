// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ValideraFx.Web.Startup;

public class AutoValidateModelStateForMvcOnlyFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
        {
            return;
        }

        if (context.ActionDescriptor is ControllerActionDescriptor cad &&
            cad.ControllerTypeInfo.IsDefined(typeof(ApiControllerAttribute), inherit: true))
        {
            return;
        }

        var problemDetails = new ExtendedValidationProblemDetails(context.ModelState)
        {
            Title = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        };

        var activity = Activity.Current;
        if (activity != null)
        {
            var traceParent = $"00-{activity.TraceId}-{activity.SpanId}-00";
            problemDetails.TraceId = traceParent;
        }

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = StatusCodes.Status400BadRequest,
            ContentTypes = { "application/problem+json", "application/problem+xml" }
        };
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}