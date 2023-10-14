using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace IpAddressSafeListAPI.Filters.Action;
public class WhitelistIpActionFilter : ActionFilterAttribute
{
	private readonly string _safelist;
	private readonly ILogger _logger;

	public WhitelistIpActionFilter(string safelist, ILogger logger)
	{
		_logger = logger;
		_safelist = safelist;
	}
	public override void OnActionExecuting(ActionExecutingContext context)
	{
		var remoteIp = context.HttpContext.Connection.RemoteIpAddress;

		if (remoteIp == null)
		{
			_logger.LogWarning("Forbidden Request from IP: {RemoteIp}", remoteIp);

			context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);

			return;
		}
		_logger.LogDebug("Remote IpAddress: {RemoteIp}", remoteIp);

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

			context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);

			return;
		}

		base.OnActionExecuting(context);
	}

}
