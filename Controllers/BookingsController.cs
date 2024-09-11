using RoomRent.Repositories;
using RoomRent.Entities.Dto;
using Microsoft.AspNetCore.Mvc;

namespace RoomRent.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController(BookingRepository bookingRepository) : ControllerBase
{
  [HttpPost]
  public IActionResult CreateBooking([FromBody] BookingDto bookingDto)
  {
    var booking = bookingRepository.Create(bookingDto);
    
    return Created(nameof(BookingsController), new { id = booking?.Id, cost = booking?.Cost });
  }
}
