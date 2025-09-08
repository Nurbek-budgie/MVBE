using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Reservation;

[ApiController]
public class ReservationController : ControllerBase
{

    public ReservationController()
    {
        
    }
    [AllowAnonymous]
    [HttpPost]
    [Route("/api/reservation/create")]
    public async Task<IActionResult> CreateReservation()
    {
        throw new NotImplementedException();
    }
    
    
    [HttpDelete]
    [Route("/api/reservation/cancel")]
    public async Task<IActionResult> CancelReservation()
    {
        throw new NotImplementedException();
    }
}