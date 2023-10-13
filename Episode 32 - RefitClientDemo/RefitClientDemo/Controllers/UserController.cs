using DTOs;
using Microsoft.AspNetCore.Mvc;

namespace RefitClientDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private static List<UserDTO> users = new()
    {
        new UserDTO {
            Id = Guid.NewGuid().ToString(),
            Name = "Muhammmad Waseem"
        },
        new UserDTO {
            Id = Guid.NewGuid().ToString(),
            Name = "Muhammmad Ali"
        },
        new UserDTO {
            Id = Guid.NewGuid().ToString(),
            Name = "Muhammmad Qayum"
        },
        new UserDTO {
            Id = Guid.NewGuid().ToString(),
            Name = "Muhammmad Hashaam"
        },
    };

    [HttpGet("/api/user/")]
    public List<UserDTO> Get()
    {
        return users;
    }

    [HttpPost("/api/user/")]
    public bool Add(UserDTO user)
    {
        users.Add(user);
        return true;
    }
}