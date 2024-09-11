namespace RoomRent.Entities.Dto;

public class BookingDto
{
  public int RoomId { get; set; }
  
  public DateOnly Date { get; set; }
  
  public TimeOnly StartTime { get; set; }
  
  public TimeOnly Duration { get; set; }

  public List<int> Services { get; set; } = [];
}
