namespace RoomRent.Entities.Dto;

public class RoomDto
{
  public string Title { get; set; } = string.Empty;

  public int Capacity { get; set; }
  
  public decimal Rent { get; set; }

  public List<int> Services { get; set; } = [];
}
