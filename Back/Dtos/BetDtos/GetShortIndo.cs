namespace Back.Dtos;

public record GetShortIndoDTO
{
    public DateTime Date { get; set; }
    public string User { get; set; } = null!;
    public double Price { get; set; }
}