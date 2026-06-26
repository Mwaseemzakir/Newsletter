using GprcClient;

namespace GrpcClient.Requests;
internal class PrimeRequestResponseHandler
{
    public static void Display(List<PrimeNumberResponseDTO> responseList)
    {
        foreach (var item in responseList)
        {
            if (item.IsSuccessfull)
            {
                Console.WriteLine($"{item.Result.Number} is {(item.Result.IsPrime ? "prime" : "not prime")}");
                Console.WriteLine($"Round Trip Time is {item.RoundTripTime}\n");
            }
            else
            {
                Console.WriteLine(item.Error.Message);
                Console.WriteLine($"Round Trip Time is {item.RoundTripTime}\n");
            }
        }
    }
}
