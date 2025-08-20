using API.Configurations;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class ValuesController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("public")]
    public IActionResult PublicEndpoint()
    {
        return Ok("Anyone can access this");
    }

    [AuthorizeRole(ERoles.Admin)]
    [HttpGet("Admin")]
    public IActionResult SecureEndpoint()
    {
        return Ok($"Hello {User.Identity.Name}, this is protected.");
    }
    
    [AuthorizeRole(ERoles.Client)]
    [HttpGet("Client")]
    public IActionResult SecureEndpointClient()
    {
        return Ok($"Hello {User.Identity.Name}, this is protected.");
    }
    
    [AuthorizeRole(ERoles.Audience)]
    [HttpGet("Audience")]
    public IActionResult SecureEndpointClientAudience()
    {
        return Ok($"Hello {User.Identity.Name}, this is protected.");
    }
    
    [AuthorizeRole(ERoles.Admin, ERoles.Audience, ERoles.Client)]
    [HttpGet("ANYROLEsecure")]
    public IActionResult SecureEndpointANYROLE()
    {
        return Ok($"Hello {User.Identity.Name}, this is protected.");
    }
}