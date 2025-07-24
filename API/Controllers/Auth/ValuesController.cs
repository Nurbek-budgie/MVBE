using API.Configurations;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class ValuesController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult PublicEndpoint()
    {
        return Ok("Anyone can access this");
    }

    [AuthorizeRole(ERoles.Admin, ERoles.Audience)]
    [HttpGet("secure")]
    public IActionResult SecureEndpoint()
    {
        return Ok($"Hello {User.Identity.Name}, this is protected.");
    }
}