using Microsoft.AspNetCore.Authorization;

namespace MinimalAPIDemo.EndPoints;
public static class UserEndPoint
{
    public static void RegisterUserAPIs(this IEndpointRouteBuilder app)
    {
        app.MapGet("/GetUser", () =>
        {
            return $"Users retrieved";
        });

        app.MapPost("/CreateUser", [AllowAnonymous] async (int RollNo, string UserName) =>
        {
            string response = $"User with RollNo : {RollNo} and UserName : {UserName} created";
            return await Task.FromResult(response);
        });

        app.MapDelete("/DeleteUser", (int RollNo) =>
        {
            return $"User with RollNo : {RollNo} deleted";
        });

        app.MapPut("/UpdateUser", (int RollNo, string UserName) =>
        {
            return $"User with RollNo : {RollNo} and UserName : {UserName} updated";
        });

        app.MapGet("/GetUsers", (int page, int pageSize, IConfiguration configuration) =>
        {
            string userName = configuration.GetValue<string>("UserName");
            return $"Paginated Users retrieved for userName : {userName}";
        });
    }
}
