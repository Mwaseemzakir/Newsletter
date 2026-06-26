using GprcClient;
using GprcClient.Helpers;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcClient.Helpers;

namespace GrpcClient.Requests;
internal static class PrimeRequestHandler
{
    public static async Task<List<PrimeNumberResponseDTO>> Send(
            int numberOfRequests, 
            string serverAddress)
    {
        List<PrimeNumberResponseDTO> responseList = new();
        var channel = GrpcChannel.ForAddress(serverAddress);
        var client = new PrimeNumbers.PrimeNumbersClient(channel);
        List<AsyncUnaryCall<PrimeNumberResponseDTO>> requestResponse = new();

        //Create N requests
        for (int start = 1; start < numberOfRequests; start++)
        {
            var request = new PrimeNumberRequestDTO
            {
                Id = start,
                Number = RandomNumberProvider.GetRandomInRange(),
                TimeStamp = DateTimeProvider.GetUnixTimeStamp()
            };
            var primeNumberTask = client.GetPrimeNumbersAsync(request);
            requestResponse.Add(primeNumberTask);
        }
        //Execute requests
        var list = await Task.WhenAll(requestResponse.Select(x => x.ResponseAsync));
        responseList.AddRange(list);
        return responseList;
    }
}
