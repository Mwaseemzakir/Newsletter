using GprcServer.Abstractions;
using GprcServer.DTOs;
using GprcServer.Extensions;

namespace GprcServer.Repositories;
public class PrimeNumbersRepository : IPrimeNumbersRepository
{
    private List<long> PrimeNumbers = new();
    private List<long> NonPrimeNumbers = new();
    public int GetTotalRequestsCount()
    {
        if (PrimeNumbers.TryGetNonEnumeratedCount(out int primeNumbersCount))
        {
            return primeNumbersCount;
        }
        return 0;
    }
    public int GetPrimeNumberRequestsCount()
    {
        int totalRequests = 0;
        if (PrimeNumbers.TryGetNonEnumeratedCount(out int primeNumbersCount))
        {
            totalRequests = totalRequests + primeNumbersCount;
        }
        if (NonPrimeNumbers.TryGetNonEnumeratedCount(out int nonPrimerNumbersCount))
        {
            totalRequests = totalRequests + nonPrimerNumbersCount;
        }
        return totalRequests;
    }
    public void AddNumber(long number, bool isPrime)
    {
        if (isPrime)
        {
            PrimeNumbers.Add(number);
        }
        else
        {
            NonPrimeNumbers.Add(number);
        }
    }

    public IEnumerable<long> GetPrimeNumbers()
    {
        return PrimeNumbers;
    }

    public string GetTopRated()
    {
        if (PrimeNumbers.Any())
        {
            var groupedPrimeNumbers = PrimeNumbers
                .GroupBy(n => n)
                .Select(group => new Occurance
                 {
                    Number = group.Key,
                    Count = group.TryGetNonEnumeratedCount(out int count) ? count : 0
                 })
                .OrderByDescending(group => group.Count)
                .Take(10);

            var topRated = groupedPrimeNumbers
                   .Select(x => x.Number)
                   .ToList();

            if (topRated.Any())
            {
                return topRated.ToCommaSeperatedArray();
            }
        }
        return string.Empty;
    }
}
