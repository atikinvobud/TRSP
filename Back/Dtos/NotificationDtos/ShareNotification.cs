namespace Back.Dtos;

public record ShareNotificationDTO
{
    public int BetId { get; set; }
    public string Text { get; set; } = null!;
}