using DelegatingHandlerSample.Models;
using Refit;

namespace DelegatingHandlerSample.Services;
public interface IDogsAPI
{
	[Get("/facts")]
	Task<GetDogFactsResponse> GetDogFactsAsync(CancellationToken cancellationToken = default);
}