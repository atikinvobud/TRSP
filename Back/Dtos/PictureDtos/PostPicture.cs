namespace Back.Dtos;

public record PostPictureDTO
{
    public string Name { get; set; } = null!;
    public string Author { get; set; } = null!;
    public  int Year { get; set; }
    public DateTime DateOfStart { get; set; }
    public DateTime DateOfEnd { get; set; }
    public double StartPrice { get; set; }
    public string PictureId { get; set; } = null!;
}