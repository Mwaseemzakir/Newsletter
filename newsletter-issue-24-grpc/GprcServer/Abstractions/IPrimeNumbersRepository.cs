using System.Collections.Generic;

namespace GprcServer.Abstractions;
public interface IPrimeNumbersRepository
{
    string? GetTopRated();
    int GetPrimeNumberRequestsCount();
    public int GetTotalRequestsCount();
    void AddNumber(long number, bool isPrime);
    public IEnumerable<long> GetPrimeNumbers();


}
