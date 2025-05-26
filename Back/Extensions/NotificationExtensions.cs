using Back.Models;

namespace Back.Dtos;

public static class NotificationExtensions
{
    public static NotificationEntity ToEntity(this PostNotificationDTO postNotificationDTO, int Id)
    {
        return new()
        {
            Text = postNotificationDTO.Text,
            BetId = postNotificationDTO.BetId,
            UserId = Id
        };
    }
}