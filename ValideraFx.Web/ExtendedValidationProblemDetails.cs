// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ValideraFx.Web;

internal class ExtendedValidationProblemDetails(ModelStateDictionary modelState) : ValidationProblemDetails(modelState)
{
    [JsonPropertyOrder(int.MaxValue)]
    public string? TraceId { get; set; }
}