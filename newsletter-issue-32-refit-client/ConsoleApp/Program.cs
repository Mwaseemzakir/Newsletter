using ConsoleApp.Client.Interfaces;
using Refit;

public class Program
{
    static async Task Main(string[] args)
    {
        string baseAddress = "https://localhost:7018";

        var userAPI = RestService.For<IUser>(baseAddress);

        var users = await userAPI.GetUsersAsync();

        foreach (var user in users)
        {
            Console.WriteLine($"{user.Id}  : {user.Name}");
        }
    }
}
