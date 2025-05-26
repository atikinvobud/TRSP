using Back.Dtos;
using Back.Models;

namespace Back.Extensions;

public static class PictureExtensions
{
    public static PictureEntity ToEntity(this PostPictureDTO postPictureDTO, int Id)
{
    return new()
    {
        Name = postPictureDTO.Name,
        Author = postPictureDTO.Author,
        Year = postPictureDTO.Year,
        DateOfStart = DateTime.SpecifyKind(postPictureDTO.DateOfStart, DateTimeKind.Utc),
        DateOfEnd = DateTime.SpecifyKind(postPictureDTO.DateOfEnd, DateTimeKind.Utc),
        StartPrice = postPictureDTO.StartPrice,
        PictureId = postPictureDTO.PictureId,
        UserId = Id
    };
}

}