using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GrpcClient.Helpers;

internal static class HostProvider
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        IHostBuilder buildHost = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, configuration) =>
            {
                configuration.Sources.Clear();
                configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            });
        return buildHost;
    }
}
