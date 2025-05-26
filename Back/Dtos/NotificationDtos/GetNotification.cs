namespace Back.Dtos;

public record GetNotificationDTO
{
    public string Text { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string User { get; set; } = null!;
    public double Price { get; set; }
}