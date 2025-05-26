using Back.Dtos;
using Back.Models;

namespace Back.Extensions;

public static class UserExtensions
{
    public static UserEntity ToEntity(this PostUserDTO postUserDTO)
    {
        return new()
        {
            Login = postUserDTO.Login,
            Password = BCrypt.Net.BCrypt.HashPassword(postUserDTO.Password),
            NickName = postUserDTO.NickName
        };
    }
}

