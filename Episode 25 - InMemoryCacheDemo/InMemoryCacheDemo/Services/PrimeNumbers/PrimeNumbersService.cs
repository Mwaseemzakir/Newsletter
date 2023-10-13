using InMemoryCacheDemo.Abstractions.Cache;
using InMemoryCacheDemo.Abstractions.PrimeNumbers;
using InMemoryCacheDemo.DTOs;
using InMemoryCacheDemo.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCacheDemo.Services.PrimeNumbers;
public class PrimeNumbersService : IPrimeNumbersService
{
    const int slidingExpirationTime = 60;
    const int absoluteExpirationTime = 3600;
    const int cacheEntryValue = 1024;
    CacheItemPriority cachePriority = CacheItemPriority.Normal;

    private readonly IMemoryCache _cache;
    private readonly IMemoryCacheOptions _cacheOptions;
    private readonly ILogger<PrimeNumbersService> _logger;
    private readonly SemaphoreSlim semaphore = new(1, 10000);
    public PrimeNumbersService(ILogger<PrimeNumbersService> logger,
                               IMemoryCache cache,
                               IMemoryCacheOptions cacheOptions)
    {
        _logger = logger;
        _cache = cache;
        _cacheOptions = cacheOptions;
    }
    public async Task<bool> VerifyPrimeNumber(long number)
    {
        try
        {
            string key = $"{number}";
            await semaphore.WaitAsync();

            if (_cache.TryGetValue(key, out PrimeCacheResult primeNumber) && primeNumber != null)
            {
                _logger.Log(LogLevel.Information, "Prime number found in cache");
                return primeNumber.isPrime;
            }
            else
            {
                _logger.Log(LogLevel.Information, "Prime number not found in cache");

                bool isCurrentNumberPrime = number.IsPrime();
                PrimeCacheResult result = new PrimeCacheResult(number, isCurrentNumberPrime);

                var cacheEntryOptions = _cacheOptions.GetMemoryOptions(
                        slidingExpirationTime,
                        absoluteExpirationTime,
                        cacheEntryValue,
                        cachePriority);

                _cache.Set(key, result, cacheEntryOptions);
                return isCurrentNumberPrime;
            }
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            semaphore.Release();
        }
    }
}
