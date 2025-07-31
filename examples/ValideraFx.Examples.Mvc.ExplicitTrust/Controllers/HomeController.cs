// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Mvc;
using ValideraFx.Core;

namespace ValideraFx.Examples.Mvc.ExplicitTrust.Controllers;

public class HomeController : Controller
{
    [Route("/Message/{message}")]
    public IActionResult Message(TrustedValue<string> message)
    {
        return View(model: message.Value);
    }
}