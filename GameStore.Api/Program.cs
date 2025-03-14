using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// physical path to your db file
var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);

var app = builder.Build();
// https://www.nuget.org/packages/dotnet-ef/8.0.2
//  https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/8.0.2

// dotnet ef database update
app.MapGamesEndpoints();
app.MapGenresEndpoints();

await app.MigrateDbAsync();

app.Run();
