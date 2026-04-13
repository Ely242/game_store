namespace GameStore.Api.Dtos;

public record UpdateGameDto(
    string Title,
    int GenreId,
    decimal Price,
    DateOnly ReleaseDate
);