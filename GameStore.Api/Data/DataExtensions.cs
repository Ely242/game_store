using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    // MigrateDatabase is an extension method for WebApplication that applies any pending migrations to the database.
    public static void MigrateDatabase(this WebApplication app)
    {
        // Create a scope to get the required services for database migration
        using var scope = app.Services.CreateScope();

        // Get the GameStoreContext from the service provider
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

        // Apply any pending migrations to the database
        dbContext.Database.Migrate();
    }

    public static void AddGameStoreDbContext(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddSqlite<GameStoreContext>(
            connectionString,
            optionsAction: options => options.UseSeeding((context, _) =>
            {
                // Seed the database with initial data
                if (!context.Set<Genre>().Any())
                {
                    context.Set<Genre>().AddRange(
                        new Genre { Name = "Action" },
                        new Genre { Name = "Adventure" },
                        new Genre { Name = "RPG" },
                        new Genre { Name = "Strategy" }
                    );

                    context.SaveChanges();
                }
            })

        ); // Register DbContext with SQLite provider
    }
}