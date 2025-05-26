using Back.Models;

namespace Back.Extensions;

public static class BetExtensions
{
    public static BetEntity ToEntity(this PostBetDTO postBetDTO, int Id)
    {
        return new()
        {
            Price = postBetDTO.Price,
            Date = postBetDTO.Date,
            PictureId = postBetDTO.PictureId,
            UserId = Id
        };
    }
}