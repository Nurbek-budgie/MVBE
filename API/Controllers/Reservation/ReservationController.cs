using BLL.Interfaces.Reservation;
using BLL.Services.Reservation;
using DTO.Reservation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Reservation;

[ApiController]
public class ReservationController : ControllerBase
{

    private readonly IReservationService _service;
    public ReservationController(IReservationService service)
    {
        _service = service;
    }
    
    [AllowAnonymous]
    [HttpPost]
    [Route("/reserve")]
    public async Task<ReservationDto.Read> CreateReservation(Guid userId,ReservationDto.ReserveSeat reservation)
    {
        var reserve = await _service.CreateReservation(userId, reservation);
        return reserve;
    }
    
    
    [HttpDelete]
    [Route("/api/reservation/cancel")]
    public async Task<IActionResult> CancelReservation()
    {
        throw new NotImplementedException();
    }
}