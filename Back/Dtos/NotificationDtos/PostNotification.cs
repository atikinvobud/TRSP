namespace Back.Dtos;

public record PostNotificationDTO
{
    public string Text { get; set; } = null!;
    public int BetId { get; set; }
}