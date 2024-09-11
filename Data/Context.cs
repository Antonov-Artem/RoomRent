using RoomRent.Entities;
using Microsoft.EntityFrameworkCore;

namespace RoomRent.Data;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
  public DbSet<Room> Room { get; init; }
  public DbSet<Service> Service { get; init; }
  public DbSet<Booking> Booking { get; init; }
}
