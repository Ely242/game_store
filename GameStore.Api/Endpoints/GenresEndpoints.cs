using GameStore.Api.Data;

namespace GameStore.Api.Endpoints;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

public static class GenresEndpoints
{
    public static void MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres").WithTags("Genres");

        // GET /genres
        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Genres
                .Select(genre => new GenreDto(
                    genre.Id,
                    genre.Name
                ))
                .AsNoTracking()
                .ToListAsync()
        );

        // GET /genres/{id}
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var genre = await dbContext.Genres
                .FirstOrDefaultAsync(g => g.Id == id);

            if (genre is null)
            {
                return Results.NotFound();
            }

            GenreDto genreDto = new (
                genre.Id,
                genre.Name
            );

            return Results.Ok(genreDto);
        }).WithName("GetGenreById");

        // POST /genres
        group.MapPost("/", async (CreateGenreDto newGenre, GameStoreContext dbContext) =>
        {
            var genre = new Genre
            {
                Name = newGenre.Name
            };

            dbContext.Genres.Add(genre);
            await dbContext.SaveChangesAsync();

            GenreDto genreDto = new (
                genre.Id,
                genre.Name
            );

            return Results.CreatedAtRoute("GetGenreById", new { id = genre.Id }, genreDto);
        });

        // PUT /genres/{id}
        group.MapPut("/{id}", async (int id, CreateGenreDto updatedGenre, GameStoreContext dbContext) =>
        {
            var genre = await dbContext.Genres.FindAsync(id);

            if (genre is null)
            {
                return Results.NotFound();
            }

            genre.Name = updatedGenre.Name;

            await dbContext.SaveChangesAsync();

            GenreDto genreDto = new (
                genre.Id,
                genre.Name
            );

            return Results.Ok(genreDto);
        });

        // DELETE /genres/{id}
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Genres
                .Where(g => g.Id == id)
                .ExecuteDeleteAsync();

            return Results.NoContent();
        });
    }
}