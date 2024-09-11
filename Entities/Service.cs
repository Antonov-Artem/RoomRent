using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomRent.Entities;

public class Service
{
  public int Id { get; set; }
  
  [Column(TypeName = "VARCHAR")]
  [StringLength(50)]
  public string Title { get; set; } = string.Empty;
  
  public decimal Cost { get; set; }
  
  public List<Room> Rooms { get; set; } = [];

  public List<Booking> Bookings { get; set; } = [];
}
