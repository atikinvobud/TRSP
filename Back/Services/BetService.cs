using Back.Dtos;
using Back.Extensions;
using Back.Models;
using Microsoft.EntityFrameworkCore;

namespace Back.Services;

public class BetService
{
    private readonly Context context;
    public BetService(Context context)
    {
        this.context = context;
    }

    public async Task<BetEntity> Create(PostBetDTO postBetDTO, int Id)
    {
        BetEntity entity = postBetDTO.ToEntity(Id);
        await context.Bets.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }
    public async Task<List<GetBetsDTO>> GetAll(int Id)
    {
        List<GetBetsDTO> entities = await context.Bets.Where(b => b.UserId == Id).Include(b => b.picture).Include(b => b.user)
        .Select(b => new GetBetsDTO()
        {
            SlotId =b.picture!.Id,
            Name = b.picture!.Name,
            Author = b.picture!.Author,
            Year = b.picture!.Year,
            Seller = b.picture!.user!.NickName,
            AmountOfBids = b.picture.bets.Count,
            Leader = b.picture.bets.OrderByDescending(b => b.Price).FirstOrDefault()!.user!.NickName,
            DateOfEnd=b.picture!.DateOfEnd,
            DateOfStart=b.picture!.DateOfStart,
            PictureId =b.picture!.PictureId,
            CurrentPrice = b.picture.bets.Any() ? b.picture.bets.Max(b => b.Price) : b.picture.StartPrice
        }).Distinct().ToListAsync();
        return entities;
    }
}