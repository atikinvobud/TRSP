namespace Back.Dtos;

public record GetAllPicturesDTO
{
    public string Name { get; set; } = null!;
    public string Author { get; set; } = null!;
    public int Year { get; set; }
    public string Seller { get; set; } = null!;
    public int AmountOfBids { get; set; }
    public string Leader { get; set; } = null!;
    public TimeSpan TimeLeft { get; set; }
    public double CurrentPrice { get; set; }
    
}