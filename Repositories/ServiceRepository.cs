using RoomRent.Data;
using RoomRent.Entities;

namespace RoomRent.Repositories;

/// <summary>
/// Provides methods for interaction with <c>Service</c> entity
/// </summary>
public class ServiceRepository(Context context)
{
  /// <summary>
  /// Takes list of service ID's and returns list of appropriate services
  /// </summary>
  /// <param name="serviceIds">list of service ID's</param>
  /// <returns>List of services with given ID's</returns>
  public List<Service> GetByIds(List<int> serviceIds)
  {
    return context.Service.Where(service => serviceIds.Contains(service.Id)).ToList();
  }

  public List<int> GetServiceIds(Room room)
  {
    return context.Service
      .Where(s => s.Rooms.Contains(room))
      .Select(s => s.Id)
      .ToList();
  }
}
