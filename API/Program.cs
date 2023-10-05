using API.Data;
using API.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Microsoft.EntityFrameworkCore.SqlServer
// dotnet tool install --global dotnet-ef

builder.Services.AddControllers();

builder.Services.AddDbContext<GameDbContext>();
builder.Services.AddTransient<ISaveRepository, SaveRepository>();
builder.Services.AddTransient<IBuildingRepository, BuildingRepository>();
builder.Services.AddTransient<IWorkerRepository, WorkerRepository>();

var app = builder.Build();

app.UseRouting();

app.MapControllers();

app.Run();
