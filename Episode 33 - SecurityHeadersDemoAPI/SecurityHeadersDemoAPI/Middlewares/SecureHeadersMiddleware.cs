namespace SecurityHeadersDemoAPI.Middlewares;
public class SecureHeadersMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Response.Headers.Add("X-Frame-Options", "DENY");
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
        context.Response.Headers.Add("Referrer-Policy", "no-referrer");
        context.Response.Headers.Add("Content-Security-Policy", "default-src 'self';");
        context.Response.Headers.Add("Permissions-Policy", "geolocation=(self), microphone=(), camera=()");
        context.Response.Headers.Add("Feature-Policy", "geolocation 'self'; microphone 'none'; camera 'none';");
        context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");
        await next(context);
    }

}
