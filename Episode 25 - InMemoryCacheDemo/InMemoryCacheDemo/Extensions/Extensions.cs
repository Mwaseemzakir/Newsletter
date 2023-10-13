namespace InMemoryCacheDemo.Extensions;
public static class Extensions
{
    public static bool IsPrime(this long number)
    {
        if (number <= 1)
            return false;

        if (number <= 3)
            return true;

        if (number % 2 == 0 || number % 3 == 0)
            return false;

        int sqrt = (int)Math.Sqrt(number);
        for (int i = 5; i <= sqrt; i += 6)
        {
            if (number % i == 0 || number % (i + 2) == 0)
                return false;
        }

        return true;
    }
}
