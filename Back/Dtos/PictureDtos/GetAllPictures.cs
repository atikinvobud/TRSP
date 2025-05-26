namespace Back.Dtos;

public record GetAllPicturesDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Author { get; set; } = null!;
    public int Year { get; set; }
    public string Seller { get; set; } = null!;
    public int AmountOfBids { get; set; }
    public string Leader { get; set; } = null!;
    public DateTime DateOfStart { get; set; }
    public DateTime DateOfEnd { get; set; }
    public string PictureId { get; set; } = null!;
    public double CurrentPrice { get; set; }
    
}