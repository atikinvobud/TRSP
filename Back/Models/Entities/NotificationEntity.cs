namespace Back.Models;

public record NotificationEntity
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public required int BetId { get; set; }
    public required int UserId { get; set; }

    public BetEntity? bet { get; set; }
    public UserEntity? user { get; set; }
}