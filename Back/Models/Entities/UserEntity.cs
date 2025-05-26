namespace Back.Models;

public record UserEntity
{
    public int Id { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
    public required string NickName { get; set; }

    public List<PictureEntity> pictures { get; set; } = new List<PictureEntity>();
    public List<BetEntity> bets { get; set; } = new List<BetEntity>();
    public List<NotificationEntity> notifications { get; set;} = new List<NotificationEntity>();
}