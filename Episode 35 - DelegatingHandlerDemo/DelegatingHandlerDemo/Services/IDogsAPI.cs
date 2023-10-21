using DelegatingHandlerDemo.Models;
using Refit;

namespace DelegatingHandlerDemo.Services;
public interface IDogsAPI
{
	[Get("/facts")]
	Task<GetDogFactsResponse> GetDogFactsAsync(CancellationToken cancellationToken = default);
}