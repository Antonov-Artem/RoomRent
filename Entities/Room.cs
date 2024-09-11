using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomRent.Entities;

public class Room
{
  public int Id { get; set; }
  
  [Column(TypeName = "VARCHAR")]
  [StringLength(50)]
  public string Title { get; set; } = string.Empty;
  
  public int Capacity { get; set; }
  
  public decimal Rent { get; set; }
  
  public List<Service> Services { get; set; } = [];
  
  public List<Booking> Bookings { get; set; } = [];
}
