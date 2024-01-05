using ApiKeyAuthenticationDemo.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ApiKeyAuthenticationDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentControlller : ControllerBase
{
 
    [ApiKeyAuthorizationFilter]
    [HttpGet]
    public IActionResult Get()  
    {
        return Ok("Hello from StudentsControlller");
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        return Ok($"Hello from StudentsControlller with id {id}");
    }

    [HttpPost]
    public IActionResult Post()
    {
        return Ok("Hello from StudentsControlller");
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id)
    {
        return Ok($"Hello from StudentsControlller with id {id}");
    }
}
