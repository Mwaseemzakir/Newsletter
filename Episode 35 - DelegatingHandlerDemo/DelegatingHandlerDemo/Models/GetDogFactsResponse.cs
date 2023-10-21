namespace DelegatingHandlerDemo.Models;
public sealed class GetDogFactsResponse
{
	public List<string> Facts { get; set; } = new();
	public bool Success { get; set; }
}
