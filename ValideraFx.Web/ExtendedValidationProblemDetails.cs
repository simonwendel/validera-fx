using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ValideraFx.Web;

internal class ExtendedValidationProblemDetails(ModelStateDictionary modelState) : ValidationProblemDetails(modelState)
{
    [JsonPropertyOrder(int.MaxValue)]
    public string? TraceId { get; set; }
}