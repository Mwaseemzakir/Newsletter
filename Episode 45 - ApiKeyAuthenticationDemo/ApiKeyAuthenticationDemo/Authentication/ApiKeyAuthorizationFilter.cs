using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiKeyAuthenticationDemo.Authentication;

public sealed class ApiKeyAuthorizationFilter : Attribute, IAuthorizationFilter
{
 
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var request = context.HttpContext.Request;

        var providedApiKey = request.Headers["ApiKey"].FirstOrDefault();

        var configuration = context
                    .HttpContext
                    .RequestServices
                    .GetRequiredService<IConfiguration>();

        var apiKey = GetApiKey(configuration);

        if (string.IsNullOrWhiteSpace(providedApiKey) || providedApiKey != apiKey)
        {
            context.Result = new UnauthorizedResult();
        }
    }

  public string GetApiKey(IConfiguration configuration)
  {
        //This is just a demo, don't store your API key in appsettings.json

        var apiKey = configuration.GetValue<string>("ApiKey");

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new Exception("ApiKey is not configured");
        }

        return apiKey;
    }
}
