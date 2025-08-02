// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Mvc;
using ValideraFx.Core;

namespace ValideraFx.Examples.WebApi.DontRenderValues.Controllers;

[ApiController]
[Route("[controller]")]
public class TwiceAsMuchController : ControllerBase
{
    [HttpGet]
    public IActionResult Get([FromQuery] TrustedValue<int> number)
    {
        return Ok(number.Value * 2);
    }
}