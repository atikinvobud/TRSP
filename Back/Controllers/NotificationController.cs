using System.Security.Claims;
using Back.Dtos;
using Back.Services;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers;

[ApiController]
[Route("notifications")]
public class NotificationController : ControllerBase
{
    private readonly NotificationService notificationService;
    public NotificationController(NotificationService notificationService)
    {
        this.notificationService = notificationService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] PostNotificationDTO postNotificationDTO)
    {
        int UserId = int.Parse(User.FindFirstValue("userId")!);
        int id = await notificationService.Create(UserId, postNotificationDTO);
        return Ok(new { id });
    }
}