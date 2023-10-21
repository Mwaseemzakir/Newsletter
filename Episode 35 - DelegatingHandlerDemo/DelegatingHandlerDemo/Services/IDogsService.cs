using DelegatingHandlerDemo.Models;

namespace DelegatingHandlerDemo.Services;
public interface IDogsService
{
	Task<GetDogFactsResponse> GetDogFactsAsync(CancellationToken cancellationToken = default);
}