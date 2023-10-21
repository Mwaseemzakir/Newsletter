using DelegatingHandlerDemo.Models;

namespace DelegatingHandlerDemo.Services;
public sealed class DogsService : IDogsService
{
	private readonly IDogsAPI _dogsAPI;

	public DogsService(IDogsAPI dogsAPI)
	{
		_dogsAPI = dogsAPI;
	}
	public Task<GetDogFactsResponse> GetDogFactsAsync(CancellationToken cancellationToken = default)
	{
		return _dogsAPI.GetDogFactsAsync(cancellationToken);
	}
}
