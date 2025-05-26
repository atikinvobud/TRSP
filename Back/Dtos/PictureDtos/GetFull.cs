using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Back.Dtos;

public record GetFullDTO
{
     public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Author { get; set; } = null!;
    public int Year { get; set; }
    public List<GetShortIndoDTO> bids { get; set; } = new List<GetShortIndoDTO>();
    public string Seller { get; set; } = null!;
    public double StartPrice { get; set; }
    public DateTime DateOfStart { get; set; }
    public DateTime DateOfEnd { get; set; }
    public string PictureId { get; set; } = null!;
}