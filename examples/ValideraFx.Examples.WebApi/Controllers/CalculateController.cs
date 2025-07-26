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
    public IActionResult Get([FromQuery] UntrustedValue<int> first, [FromQuery] UntrustedValue<int> second)
    {
        try
        {
            return Ok(validator.Validate(first) + validator.Validate(second));
        }
        catch (ValidationException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}