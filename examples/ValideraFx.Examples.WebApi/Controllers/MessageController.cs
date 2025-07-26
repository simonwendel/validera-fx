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
    public IActionResult Get([FromQuery] UntrustedValue<MessageOptions> options)
    {
        try
        {
            var validatedOptions = validator.Validate(options);
            var message = validatedOptions.Message;
            var numberOfTimes = validatedOptions.NumberOfTimes;
            return Ok(string.Join('\n', Enumerable.Range(0, numberOfTimes).Select(_ => message)));
        }
        catch (ValidationException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}

public class MessageOptions
{
    public required string Message { get; set; }
    public int NumberOfTimes { get; set; }
}