using Back.Dtos;
using Back.Extensions;
using Back.Models;
using Microsoft.EntityFrameworkCore;

namespace Back.Services;

public class AuthorizationService
{
    private readonly Context context;
    public AuthorizationService(Context context)
    {
        this.context = context;
    }

    public async Task<int> Create(PostUserDTO postUserDTO)
    {

        List<UserEntity> users = await context.Users.ToListAsync();
        if(users.Where(u => u.Login == postUserDTO.Login || u.NickName==postUserDTO.NickName).ToList().Count == 0 )
        {
            UserEntity entity = postUserDTO.ToEntity();
            await context.Users.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity.Id;
        }
        return 0;
    }

    public async Task<UserEntity> Login(string login, string password)
    {
        UserEntity? user = await context.Users.FirstOrDefaultAsync(u => u.Login == login);
        if (user == null) return null!;
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
        if (!isPasswordValid) return null!;
        return user;
    }
}