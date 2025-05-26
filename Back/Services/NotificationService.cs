using Back.Dtos;
using Back.Models;
using Microsoft.EntityFrameworkCore;

namespace Back.Services;

public class NotificationService
{
    private readonly Context context;
    public NotificationService(Context context)
    {
        this.context = context;
    }

    public async Task<int> Create(int Id, PostNotificationDTO postNotificationDTO)
    {
        NotificationEntity entity = postNotificationDTO.ToEntity(Id);
        await context.notifications.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity.Id;
    }
    public async Task<List<GetNotificationDTO>> GetAll(int Id)
    {
        List<GetNotificationDTO> dtos = await context.notifications.Where(n => n.UserId == Id).Include(n => n.bet)
        .Select(n => new GetNotificationDTO()
        {
            Text = n.Text,
            Name = n.bet!.picture!.Name,
            User = n.bet!.user!.NickName,
            Price = n.bet!.Price,
        }).ToListAsync();
        return dtos;
    }
}