using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GrpcClient.Helpers
{
    internal static class ConfigurationProvider
    {
        public static string? GetValue(IHost host, string name)
        {
            IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
            return config.GetValue<string>(name);
        }
    }
}
