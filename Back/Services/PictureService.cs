using Back.Dtos;
using Back.Extensions;
using Back.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Back.Services;

public class PicrureService
{
    private readonly Context context;
    public PicrureService(Context context)
    {
        this.context = context;
    }

    public async Task<int> Create(PostPictureDTO postPictureDTO, int Id)
    {
        PictureEntity entity = postPictureDTO.ToEntity(Id);
        await context.Pictures.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity.Id;
    }
    public async Task<List<GetAllPicturesDTO>> GetAll()
    {
        List<GetAllPicturesDTO> entities = await context.Pictures.Include(p => p.bets).Include(p => p.user)
        .Select(p => new GetAllPicturesDTO()
        {
            Id =p.Id,
            Name = p.Name,
            Author = p.Author,
            Year = p.Year,
            Seller = p.user!.NickName,
            AmountOfBids = p.bets.Count,
            CurrentPrice = p.bets.Any() ? p.bets.Max(b => b.Price) : p.StartPrice,
            Leader = p.bets.OrderByDescending(b => b.Price).FirstOrDefault()!.user!.NickName,
            DateOfStart = p.DateOfStart,
            DateOfEnd = p.DateOfEnd,
            PictureId =p.PictureId
        }).ToListAsync();
        return entities;
    }

    public async Task<List<GetMyPicturesDTO>> GetMy(int Id)
    {
        List<GetMyPicturesDTO> entities = await context.Pictures.Where(p => p.UserId == Id).Include(p => p.bets).Include(p => p.user)
        .Select(p => new GetMyPicturesDTO()
        {
            Id =p.Id,
            Name = p.Name,
            Author = p.Author,
            Year = p.Year,
            AmountOfBids = p.bets.Count,
            CurrentPrice = p.bets.Any() ? p.bets.Max(b => b.Price) : p.StartPrice,
            Leader = p.bets.OrderByDescending(b => b.Price).FirstOrDefault()!.user!.NickName,
            DateOfEnd = p.DateOfEnd,
            DateOfStart = p.DateOfStart,
            PictureId = p.PictureId
        }).ToListAsync();
        return entities;
    }
    public async Task<GetFullDTO> GetFull(int Id)
    {
        List<GetFullDTO> entities = await context.Pictures.Where(p => p.Id == Id).Include(p => p.bets).Include(p => p.user)
        .Select(p => new GetFullDTO()
        {
            Id =p.Id,
            Name = p.Name,
            Author = p.Author,
            Year = p.Year,
            Seller = p.user!.NickName,
            StartPrice = p.StartPrice,
            DateOfStart = p.DateOfStart,
            DateOfEnd = p.DateOfEnd,
            PictureId =p.PictureId,
            bids = p.bets.Select(b => new GetShortIndoDTO()
            {
                Date = b.Date,
                User = b.user!.NickName,
                Price = b.Price
            }).ToList()
        }).ToListAsync();
        if (entities.Count == 0) return null!;
        return entities[0];
    }

    public async Task<List<int>> GetParticipants(int Id)
    {
        var userIds = await context.Pictures.Where(p => p.Id == Id)
        .SelectMany(p => p.bets).Select(b => b.UserId)
        .Distinct().ToListAsync();
        return userIds;
    }
}