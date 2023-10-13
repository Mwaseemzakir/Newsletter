namespace InMemoryCacheDemo.Abstractions.PrimeNumbers;
public interface IPrimeNumbersService
{
    Task<bool> VerifyPrimeNumber(long number);
}