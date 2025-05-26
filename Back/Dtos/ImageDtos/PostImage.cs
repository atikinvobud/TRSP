namespace Back.Dtos;

public record PostImageDTO
{
    public IFormFile File { get; set; } = null!;
}