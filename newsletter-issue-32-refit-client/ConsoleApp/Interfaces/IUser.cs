using DTOs;
using Refit;

namespace ConsoleApp.Client.Interfaces;

[Headers("Accept: application/json")]
public interface IUser
{
    [Get("/api/user/")]
    Task<List<UserDTO>> GetUsersAsync();

    [Post("/api/user")]
    Task PostAsync(UserDTO userDTO);
}
