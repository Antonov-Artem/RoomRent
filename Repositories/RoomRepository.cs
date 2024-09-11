using RoomRent.Data;
using RoomRent.Entities;
using RoomRent.Entities.Dto;
using Microsoft.EntityFrameworkCore;

namespace RoomRent.Repositories;

/// <summary>
/// Provides methods for interaction with <c>Room</c> entity
/// </summary>
public class RoomRepository(Context context, ServiceRepository serviceRepository)
{
  /// <summary>
  /// Returns room with specified ID
  /// </summary>
  /// <param name="roomId">room ID</param>
  /// <returns>If a room with the specified ID exists returns <c>Room</c> entity, otherwise returns <c>NULL</c></returns>
  public Room? GetById(int roomId)
  {
    return context.Room.SingleOrDefault(room => room.Id == roomId);
  }
  
  /// <summary>
  /// Creates new <c>Room</c> entity in database
  /// </summary>
  /// <param name="roomDto">title, capacity, rent and available services</param>
  /// <returns>Created <c>Room</c> entity</returns>
  public Room Create(RoomDto roomDto)
  {
    var room = new Room
    {
      Title = roomDto.Title,
      Rent = roomDto.Rent,
      Capacity = roomDto.Capacity,
      Services = serviceRepository.GetByIds(roomDto.Services)
    };

    context.Room.Add(room);
    
    context.SaveChanges();
    
    return room;
  }

  /// <summary>
  /// Update <c>Room</c> entity with specified ID in database
  /// </summary>
  /// <param name="roomId">room ID</param>
  /// <param name="roomDto">title, capacity, rent and available services</param>
  /// <returns>Updated <c>Room</c> entity</returns>
  public Room? Update(int roomId, RoomDto roomDto)
  {
    var room = context.Room
      .Include(s => s.Services)
      .FirstOrDefault(room => room.Id == roomId);

    if (room is null) return null;
    
    room.Title = roomDto.Title;
    room.Rent = roomDto.Rent;
    room.Capacity = roomDto.Capacity;
    room.Services.Clear();
    room.Services.AddRange(serviceRepository.GetByIds(roomDto.Services));

    context.SaveChanges();

    return room;
  }
  
  /// <summary>
  /// Deletes <c>Room</c> entity with given ID in database
  /// </summary>
  /// <param name="roomId">room ID</param>
  /// <returns>If a room with the specified ID exists returns deleted <c>Room</c> entity, otherwise return <c>NULL</c></returns>
  public Room? Delete(int roomId)
  {
    var room = GetById(roomId);

    if (room == null) return null;
    
    context.Room.Remove(room);

    context.SaveChanges();

    return room;
  }
  
  /// <summary>
  /// Returns list of <c>Room</c> entities that matches specified values
  /// </summary>
  /// <param name="date">date of meeting</param>
  /// <param name="startTime">time when meeting starts</param>
  /// <param name="capacity">max amount of people allowed</param>
  /// <returns>List of <c>Room</c> entities</returns>
  public List<Room> GetAllFree(
    DateOnly date,
    TimeOnly startTime,
    int capacity)
  {
    var freeRooms =
      from room in context.Room
      from booking in context.Booking
      where booking.Date == date &&
            (booking.StartTime > startTime ||
            booking.StartTime.Add(booking.Duration.ToTimeSpan()) < startTime) &&
            room.Capacity == capacity
      select room;
    
    return freeRooms.ToList();
  }
}
