using DelegatingHandlerDemo.Models;
using DelegatingHandlerDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace DelegatingHandlerDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class DogsController
{
	private readonly IDogsService _dogsService;

	public DogsController(IDogsService dogsService)
	{
		_dogsService = dogsService;
	}

	[HttpGet(Name = "GetDogFacts")]
	public async Task<GetDogFactsResponse> GetDogFactsAsync(CancellationToken cancellationToken = default)
	{
		return await _dogsService.GetDogFactsAsync(cancellationToken);
	}
}
