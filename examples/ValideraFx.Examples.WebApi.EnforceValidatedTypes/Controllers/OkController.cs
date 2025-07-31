using Microsoft.AspNetCore.Mvc;

namespace ValideraFx.Examples.WebApi.EnforceValidatedTypes.Controllers;

[ApiController]
[Route("[controller]")]
public class OkController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(int i) => Ok(i);
}