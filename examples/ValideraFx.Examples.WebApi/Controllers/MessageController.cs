// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Mvc;
using ValideraFx.Core;

namespace ValideraFx.Examples.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController(IValidator<MessageOptions> validator) : ControllerBase
{
    [HttpGet(Name = "GetMessage")]
    public IActionResult Get([FromQuery]UntrustedValue<MessageOptions> options)
    {
        return Ok(validator.Validate(options).Message);
    }
}

public class MessageOptions
{
    public string Message { get; set; }
    public int NumberOfTimes { get; set; }  
}