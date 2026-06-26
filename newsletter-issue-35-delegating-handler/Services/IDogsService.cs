using DelegatingHandlerSample.Models;

namespace DelegatingHandlerSample.Services;
public interface IDogsService
{
	Task<GetDogFactsResponse> GetDogFactsAsync(CancellationToken cancellationToken = default);
}