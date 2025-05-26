using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Back.Dtos;
using Back.Models;
using Back.Services;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers;

[ApiController]
[Route("bet")]
public class BetController : ControllerBase
{
    private readonly BetService betService;
    private readonly PicrureService picrureService;
    private readonly WebSocketService webSocketService;
    private readonly NotificationService notificationService;

    public BetController(BetService betService, PicrureService picrureService, WebSocketService webSocketService, NotificationService notificationService)
    {
        this.betService = betService;
        this.picrureService = picrureService;
        this.webSocketService = webSocketService;
        this.notificationService = notificationService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] PostBetDTO postBetDTO)
    {
        int UserId = int.Parse(User.FindFirstValue("userId")!);
        BetEntity bet = await betService.Create(postBetDTO, UserId);
        var participants = await picrureService.GetParticipants(bet.PictureId);
        participants.Remove(bet.UserId);
        string message = "новая ставка";
        ShareNotificationDTO share = new()
        {
            BetId = bet.Id,
            Text = message
        };
        await webSocketService.BroadcastToUsersAsync(participants, share);

        foreach (var participantId in participants)
        {
            await notificationService.Create(participantId, new PostNotificationDTO()
            {
                Text = message,
                BetId = bet.Id,
            });
        }
        return Ok();
    }

    [HttpGet("getbets")]
    public async Task<IActionResult> Get()
    {
        int UserId = int.Parse(User.FindFirstValue("userId")!);
        return Ok(await betService.GetAll(UserId));
    }
}