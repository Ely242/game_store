using System.ComponentModel.DataAnnotations;

public record CreateGameDto(
    [Required] string Title,
    [Required] int GenreId,
    [Range(0, double.MaxValue)] decimal Price,
    [Required] DateOnly ReleaseDate
);