// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ValideraFx.Core;
using ValideraFx.Examples.Mvc.ExplicitTrust.Models;

namespace ValideraFx.Examples.Mvc.ExplicitTrust.Controllers;

public class HomeController : Controller
{
    [Route("/Message/{message}")]
    public IActionResult Message(TrustedValue<string> message)
    {
        return View(model: message.Value);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}