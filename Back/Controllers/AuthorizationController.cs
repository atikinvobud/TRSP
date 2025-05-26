using Back.Dtos;
using Back.Models;
using Back.Services;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers;

[ApiController]
[Route("auth")]

public class AuthorizationController : ControllerBase
{
    private readonly AuthorizationService authorizationService;
    private readonly JwtService jwtService;

    public AuthorizationController(AuthorizationService authorizationService, JwtService jwtService)
    {
        this.authorizationService = authorizationService;
        this.jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Registr([FromBody] PostUserDTO userDTO)
    {
        int ids  = await authorizationService.Create(userDTO);
        if (ids != 0) return Ok(new { id  = ids });
        return Conflict();
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login([FromQuery] string Login, [FromQuery] string Password)
    {
        UserEntity? user = await authorizationService.Login(Login, Password);
        if (user == null) return NotFound();

        var token = jwtService.GenerateToken(user);
        Response.Cookies.Append("jwt", token, new CookieOptions
        {
            //HttpOnly = true,
            //Secure = true,
            Expires = DateTime.UtcNow.AddHours(12)
        });
        return Ok(new
        {
            id = user.Id,
            nickName =user.NickName });
    }
}