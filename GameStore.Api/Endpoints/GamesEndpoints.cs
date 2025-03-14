using System;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameSummaryDto> games = [
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

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
            .WithParameterValidation(); // nuget package MinimalApis.Extensions;

       // GET /games
        group.MapGet("/", async (GameStoreContext dbContext) => 
            await dbContext.Games
                .Include(game => game.Genre)
                .Select(game => game.ToGameSummaryDto())
                .AsNoTracking()
                .ToListAsync());

        // GET /games/1
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
            {
                Game? game = await dbContext.Games.FindAsync(id);

                return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
            })
                .WithName(GetGameEndpointName);


        // POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) => 
            {

                Game game = newGame.ToEntity();
                game.Genre = dbContext.Genres.Find(newGame.GenreId);

                
                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();


                return Results.CreatedAtRoute(
                    GetGameEndpointName,
                    new { id = game.Id},
                    game.ToGameDetailsDto());
            })
            ;

        //PUT /games
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
            {
                var existingGame = await dbContext.Games.FindAsync(id);

                if (existingGame is null)
                {
                    return Results.NotFound();
                }


                dbContext.Entry(existingGame)
                    .CurrentValues
                    .SetValues(updatedGame.ToEntity(id));

                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            });

        // DELETE/games/1
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) => 
            {
                await dbContext.Games
                    .Where(game => game.Id == id)
                    .ExecuteDeleteAsync();

                return Results.NoContent();
            }); 

        return group;
    }
}
