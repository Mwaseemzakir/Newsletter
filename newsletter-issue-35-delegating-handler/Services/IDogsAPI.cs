using DelegatingHandler.Models;
using Refit;

namespace DelegatingHandler.Services;
public interface IDogsAPI
{
	[Get("/facts")]
	Task<GetDogFactsResponse> GetDogFactsAsync(CancellationToken cancellationToken = default);
}