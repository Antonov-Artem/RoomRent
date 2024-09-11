namespace RoomRent.Entities;

public class Booking
{
  public int Id { get; set; }
  
  public decimal Cost { get; set; }
  
  public DateOnly Date { get; set; }
  
  public TimeOnly StartTime { get; set; }
  
  public TimeOnly Duration { get; set; }
  
  public int RoomId { get; set; }

  public Room Room { get; set; } = null!;
  
  public List<Service> Services { get; set; } = [];
}
