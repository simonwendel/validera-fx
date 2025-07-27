// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Mvc;
using ValideraFx.Core;

namespace ValideraFx.Examples.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController(IValidator<Message> validator) : ControllerBase
{
    [HttpGet(Name = "GetMessage")]
    public IActionResult Get([FromQuery] UntrustedValue<Message> options)
    {
        try
        {
            var validatedOptions = validator.Validate(options);
            var message = validatedOptions.Text;
            var numberOfTimes = validatedOptions.Repeat;
            return Ok(string.Join('\n', Enumerable.Range(0, numberOfTimes).Select(_ => message)));
        }
        catch (ValidationException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}

public class Message
{
    public required string Text { get; set; }
    public int Repeat { get; set; }
}