using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Back.Dtos;
using Back.Services;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers;

[ApiController]
[Route("picture")]
public class PictureController : ControllerBase
{
    private readonly PicrureService picrureService;

    public PictureController(PicrureService picrureService)
    {
        this.picrureService = picrureService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] PostPictureDTO postPictureDTO)
    {
        int UserId = int.Parse(User.FindFirstValue("userId")!);
        int id = await picrureService.Create(postPictureDTO, UserId);
        return Ok(new { id });
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await picrureService.GetAll());
    }

    [HttpGet("getmy")]
    public async Task<IActionResult> GetMy()
    {
        int UserId = int.Parse(User.FindFirstValue("userId")!);
        return Ok(await picrureService.GetMy(UserId));
    }

    [HttpGet("getfull/{id}")]
    public async Task<IActionResult> GetFull([FromRoute] int id)
    {
        GetFullDTO? dto = await picrureService.GetFull(id);
        if (dto == null) return NotFound();
        return Ok(dto);
    }
}