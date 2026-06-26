using DelegatingHandler.Models;

namespace DelegatingHandler.Services;
public interface IDogsService
{
	Task<GetDogFactsResponse> GetDogFactsAsync(CancellationToken cancellationToken = default);
}