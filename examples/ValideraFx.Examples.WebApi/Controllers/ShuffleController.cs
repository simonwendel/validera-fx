// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Mvc;
using ValideraFx.Core;

namespace ValideraFx.Examples.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ShuffleController : ControllerBase
{
    [HttpGet(Name = "GetShuffleList")]
    public IActionResult Get([FromQuery] TrustedValue<ShuffleRequest> options)
    {
        return Ok(options.Value.Items.OrderBy(_ => Guid.NewGuid()).ToList());
    }
}

public class ShuffleRequest
{
    public IEnumerable<string> Items { get; set; }
}