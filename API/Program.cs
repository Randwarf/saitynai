using API.Data;

var builder = WebApplication.CreateBuilder(args);

// Microsoft.EntityFrameworkCore.SqlServer
// dotnet tool install --global dotnet-ef

builder.Services.AddControllers();

builder.Services.AddDbContext<GameDbContext>();

var app = builder.Build();

app.UseRouting();

app.MapControllers();

app.Run();
