using System.Net;

namespace IpAddressSafeListAPI.Middlewares;
public class WhitelistIpMiddleware : IMiddleware
{
	private readonly string _safelist;
	private readonly ILogger _logger;

	public WhitelistIpMiddleware(string safelist, ILogger logger)
	{
		_safelist = safelist;
		_logger = logger;
	}
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		var remoteIp = context.Connection.RemoteIpAddress;

		if (remoteIp == null)
		{
			_logger.LogWarning("Forbidden Request from IP: {RemoteIp}", remoteIp);

			context.Response.StatusCode = StatusCodes.Status403Forbidden;

			return;
		}
		var ip = _safelist.Split(';');

		var badIp = true;

		if (remoteIp.IsIPv4MappedToIPv6)
		{
			remoteIp = remoteIp.MapToIPv4();
		}

		foreach (var address in ip)
		{
			var testIp = IPAddress.Parse(address);

			if (testIp.Equals(remoteIp))
			{
				badIp = false;
				break;
			}
		}

		if (badIp)
		{
			_logger.LogWarning("Forbidden Request from IP: {RemoteIp}", remoteIp);

			context.Response.StatusCode = StatusCodes.Status403Forbidden;

			return;
		}
		await next.Invoke(context);
	}

}
