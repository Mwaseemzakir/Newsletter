namespace GprcClient.Helpers;
public static class DateTimeProvider
{
    public static long GetUnixTimeStamp()
    {
        return DateTimeOffset
            .UtcNow
            .ToUnixTimeSeconds();
    }
}
