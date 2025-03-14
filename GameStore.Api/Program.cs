using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
    new (
        1,
        "Marvel Rivals",
        "Hero Shooter",
        00.00M,
        new DateOnly(2024, 12, 5)),
    new (
        2,
        "Baldur's Gate III",
        "RPG",
        60.00M,
        new DateOnly(2023, 8, 3)),
    new (
        3,
        "Hollow Knight",
        "Metroidvania",
        15.00M,
        new DateOnly(2017, 2, 24))

];

// GET /games
app.MapGet("games", () => games);

// GET /games/1
app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id))
    .WithName(GetGameEndpointName);


// POST /games
app.MapPost("games", (CreateGameDto newGame) => 
{
    GameDto game = new (
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate);

    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id}, game);
});

//PUT /games
app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame) =>
{
    var index = games.FindIndex(game => game.Id == id);

    games[index] = new GameDto(
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );

    return Results.NoContent();
});

app.Run();
