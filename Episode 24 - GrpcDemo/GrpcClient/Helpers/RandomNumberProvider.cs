namespace GrpcClient.Helpers;
internal static class RandomNumberProvider
{
    public static long GetRandomInRange(int start = 1, int end = 1000)
    {
        Random random = new Random();
        return random.Next(start, end);
    }
}
