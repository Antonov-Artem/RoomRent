using RoomRent.Data;
using RoomRent.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
  options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<RoomRepository>();
builder.Services.AddTransient<BookingRepository>();
builder.Services.AddTransient<ServiceRepository>();
builder.Services.AddDbContext<Context>(options =>
{
   options.UseNpgsql(builder.Configuration.GetConnectionString("RoomRentDB"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
