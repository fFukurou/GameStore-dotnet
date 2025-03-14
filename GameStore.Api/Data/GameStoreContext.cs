using System;
using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;
// nuget package: https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/8.0.2 

// Gonna Map our objects/entities into proper database tables
public class GameStoreContext(DbContextOptions<GameStoreContext> options)
    : DbContext(options)
{
    // DbSet is gonna query and save instances in this case, of Game
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new {Id = 1, Name = "Fighting"},
            new {Id = 2, Name = "Roleplaying"},
            new {Id = 3, Name = "Sports"},
            new {Id = 4, Name = "Racing"},
            new {Id = 5, Name = "Kids and Family"}
        );
    }
}
