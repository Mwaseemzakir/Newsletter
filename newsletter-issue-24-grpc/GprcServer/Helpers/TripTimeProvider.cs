using System.Diagnostics;

namespace GprcServer.Helpers;
internal static class TripTimeProvider
{
    private static Stopwatch stopwatch;
    static TripTimeProvider()
    {
        stopwatch = new();
    }
    public static void StartWatch()
    {
        stopwatch.Reset();
        stopwatch.Start();
    }
    public static void StopWatch()
    {
        stopwatch.Stop();
    }
    public static double RoundTripTime()
    {
        TimeSpan rtt = stopwatch.Elapsed;
        return rtt.TotalMilliseconds;
    }
}
