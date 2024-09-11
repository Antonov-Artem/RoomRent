using RoomRent.Data;
using RoomRent.Entities;
using RoomRent.Entities.Dto;

namespace RoomRent.Repositories;

/// <summary>
/// Provides methods for interaction with <c>Booking</c> entity
/// </summary>
public class BookingRepository(
  Context context,
  RoomRepository roomRepository,
  ServiceRepository serviceRepository)
{
  /// <summary>
  /// Calculates total cost of the booking
  /// </summary>
  /// <param name="booking"><c>Booking</c> entity</param>
  /// <param name="room"><c>Room</c> entity</param>
  /// <returns>Total cost of the booking</returns>
  private decimal CalculateCost(Booking booking, Room room)
  {
    var servicesCost = room.Services.Sum(service => service.Cost);
    
    var baseCost = room.Rent + servicesCost;

    var rentCost = booking.StartTime.Hour switch
    {
      > 9 and < 12 or > 14 and < 18 => baseCost,
      > 6 and < 9 => baseCost * (decimal)0.9,
      > 12 and < 14 => baseCost * (decimal)1.15,
      > 18 and < 23 => room.Rent * (decimal)0.8 + servicesCost,
      _ => 0
    };

    var totalCost = rentCost * booking.Duration.Hour;
    
    return totalCost;
  }
  
  /// <summary>
  /// Makes the booking of the room
  /// </summary>
  /// <param name="bookingDto">room ID, date, start time, duration and selected services</param>
  /// <returns>If a room with the specified ID exists returns created <c>Booking</c> entity, otherwise return <c>NULL</c></returns>
  public Booking? Create(BookingDto bookingDto)
  {
    var room = roomRepository.GetById(bookingDto.RoomId);
    
    if (room is null) return null;

    var freeRooms = roomRepository.GetAllFree(bookingDto.Date, bookingDto.StartTime, room.Capacity);
    
    if (freeRooms.Contains(room)) return null;
    
    var booking = new Booking
    {
      RoomId = bookingDto.RoomId,
      Date = bookingDto.Date,
      StartTime = bookingDto.StartTime,
      Duration = bookingDto.Duration,
      Services = serviceRepository.GetByIds(bookingDto.Services)
    };

    booking.Cost = CalculateCost(booking, room);

    context.Booking.Add(booking);
    
    context.SaveChanges();
    
    return booking;
  }

  /// <summary>
  /// Returns list of <c>Booking</c> entities with specified ID's
  /// </summary>
  /// <param name="room"><c>Room</c> entity</param>
  /// <returns>list of booking ID's of specified room</returns>
  public List<int> GetBookingIds(Room room)
  {
    return context.Booking
      .Where(b => b.Room.Id == room.Id)
      .Select(b => b.Id)
      .ToList();
  }
}
