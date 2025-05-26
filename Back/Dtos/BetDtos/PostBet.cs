namespace Back.Models;

public record PostBetDTO
{
    public double Price { get; set; }
    public DateTime Date { get; set; }
    public int PictureId { get; set; }
}