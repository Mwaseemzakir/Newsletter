using System.Diagnostics;

namespace DelegatingHandlerDemo.DelegateHandlers;
public sealed class LoggingHandler : DelegatingHandler
{
	private readonly ILogger<LoggingHandler> _logger;

	public LoggingHandler(ILogger<LoggingHandler> logger)
	{
		_logger = logger;
	}

	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		Debugger.Break();

		_logger.LogInformation("Request: {Method} {RequestUri}", request.Method, request.RequestUri);

		var response = await base.SendAsync(request, cancellationToken);

		_logger.LogInformation("Response: {StatusCode}", response.StatusCode);

		return response;
	}
}
