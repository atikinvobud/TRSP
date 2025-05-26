using System.Reflection.Metadata;

namespace Back.Dtos;

public record LoginUserDTO
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}