using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Auth;

[ApiController]
public class ManagerController : ControllerBase
{
    
    
    // [Authorize(Roles = "Manager")]
    [HttpPost("screens")]
    public async Task<IActionResult> CreateScreen([FromBody] bool dto)
    {
        var theaterIdClaim = User.FindFirst("theaterId")?.Value;
        // if (theaterIdClaim == null || dto.TheaterId.ToString() != theaterIdClaim)
        //     return Forbid();

        // proceed to create screen
        return Ok(true);
    }
}