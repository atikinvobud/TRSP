using System.Reflection.Metadata.Ecma335;
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

    public BetController(BetService betService)
    {
        this.betService = betService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] PostBetDTO postBetDTO)
    {
        int UserId = int.Parse(User.FindFirstValue("userId")!);
        int id = await betService.Create(postBetDTO, UserId);
        return Ok(new { id });
    }

    [HttpGet("getbets")]
    public async Task<IActionResult> Get()
    {
        int UserId = int.Parse(User.FindFirstValue("userId")!);
        return Ok(await betService.GetAll(UserId));
    }
}