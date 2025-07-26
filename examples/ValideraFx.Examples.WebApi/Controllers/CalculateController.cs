// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Mvc;
using ValideraFx.Core;

namespace ValideraFx.Examples.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CalculateController(IValidator validator) : ControllerBase
{
    [HttpGet("sum", Name = "SumNumbers")]
    public IActionResult Get([FromQuery] UntrustedValue<CalculationOptions> options)
    {
        try
        {
            var numbers = validator.Validate(options);
            return Ok(numbers.First + numbers.Second);
        }
        catch (ValidationException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}

public class CalculationOptions
{
    public int First { get; set; }
    public int Second { get; set; }
}