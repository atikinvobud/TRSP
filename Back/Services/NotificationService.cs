using Back.Dtos;
using Back.Models;

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
}