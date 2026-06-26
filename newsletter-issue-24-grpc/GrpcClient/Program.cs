using GprcClient;
using GrpcClient.Helpers;
using GrpcClient.Requests;

namespace GrpcClient;
internal class Program
{
    const int maxRequests = 1500;
    static async Task Main(string[] args)
    {
        using var host = HostProvider.CreateHostBuilder(args).Build();

        string? serverAddress = ConfigurationProvider.GetValue(host, "Grpc:ServerAddress");

        List<PrimeNumberResponseDTO> reply = await PrimeRequestHandler.Send(
                                                          maxRequests, 
                                                          serverAddress);
        PrimeRequestResponseHandler.Display(reply);
    }
}