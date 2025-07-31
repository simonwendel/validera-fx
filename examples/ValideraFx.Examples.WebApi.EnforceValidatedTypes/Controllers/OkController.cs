// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Mvc;

namespace ValideraFx.Examples.WebApi.EnforceValidatedTypes.Controllers;

[ApiController]
[Route("[controller]")]
public class OkController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(int i) => Ok(i);
}