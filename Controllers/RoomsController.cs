using RoomRent.Repositories;
using RoomRent.Entities.Dto;
using Microsoft.AspNetCore.Mvc;
using RoomRent.Data;

namespace RoomRent.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController(
  RoomRepository roomRepository,
  ServiceRepository serviceRepository,
  BookingRepository bookingRepository) : ControllerBase
{
  [HttpPost]
  public IActionResult CreateRoom([FromBody] RoomDto roomDto)
  {
    var room = roomRepository.Create(roomDto);
    
    return Created(string.Empty, new { id = room.Id });
  }

  [HttpPut("{roomId:int}")]
  public IActionResult UpdateRoom(int roomId, [FromBody] RoomDto roomDto)
  {
    var room = roomRepository.Update(roomId, roomDto);

    return Ok(new
    {
      id = room?.Id,
      title = room?.Title,
      capacity = room?.Capacity,
      rent = room?.Rent,
      services = room?.Services.Select(s => s.Id),
      bookings = room?.Bookings.Select(b => b.Id)
    });
  }

  [HttpDelete("{roomId:int}")]
  public IActionResult DeleteRoom(int roomId)
  {
    var room = roomRepository.Delete(roomId);
    
    return Ok(room);
  }

  [HttpGet]
  public IActionResult FindRooms(
    [FromQuery] DateOnly date,
    [FromQuery] TimeOnly startTime,
    [FromQuery] int capacity)
  {
    var freeRooms = roomRepository.GetAllFree(date, startTime, capacity);
    
    var fullRooms = freeRooms.Select(room => new
    {
      id = room.Id,
      title = room.Title,
      capacity = room.Capacity,
      rent = room.Rent,
      services = serviceRepository.GetServiceIds(room),
      bookings = bookingRepository.GetBookingIds(room)
    });
    
    return Ok(fullRooms);
  }
}
