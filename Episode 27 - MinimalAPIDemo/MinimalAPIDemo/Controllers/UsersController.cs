using Microsoft.AspNetCore.Mvc;

namespace MinimalAPIDemo.Controllers;

[ApiController]
[Route("Users")]
public class UsersController : ControllerBase
{

    [HttpGet]
    public string Get()
    {
        return $"Users retrieved";
    }

    [HttpPost]
    public string Create(int rollNo, string userName)
    {
        return $"User with RollNo : {rollNo} and UserName : {userName} created";
    }

    [HttpPut]
    public string Update(int rollNo, string userName)
    {
        return $"User with RollNo : {rollNo} and UserName : {userName} updated";
    }

    [HttpDelete]
    public string Delete(int rollNo)
    {
        return $"User with RollNo : {rollNo} deleted";
    }
}
