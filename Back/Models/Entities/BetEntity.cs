namespace Back.Models;

public record BetEntity
{
    public int Id { get; set; }
    public required double Price { get; set; }
    public required int PictureId { get; set; }
    public required int UserId { get; set; }
    public required DateTime Date { get; set; }

    public PictureEntity? picture { get; set; }
    public UserEntity? user { get; set; }
    public List<NotificationEntity> notifications { get; set; } = new List<NotificationEntity>();
}