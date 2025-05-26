namespace Back.Models;

public record PictureEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Author { get; set; }
    public required int Year { get; set; }
    public required DateTime DateOfStart { get; set; }
    public required DateTime DateOfEnd { get; set; }
    public required double StartPrice { get; set; }
    public  required int UserId { get; set; }
    public required string PictureId { get; set; }

    public UserEntity? user { get; set; }
    public List<BetEntity> bets { get; set; } = new List<BetEntity>();
}