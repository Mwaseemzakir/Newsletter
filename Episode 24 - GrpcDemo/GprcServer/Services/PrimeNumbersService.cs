using GprcServer.Abstractions;
using GprcServer.Extensions;
using GprcServer.Helpers;
using Grpc.Core;
using Microsoft.Extensions.Caching.Memory;

namespace GprcServer.Services;
public class PrimeNumbersService : PrimeNumbers.PrimeNumbersBase
{
    private IMemoryCache _cache;
    private readonly IMemoryCacheOptions _cacheOptions;
    private readonly ILogger<PrimeNumbersService> _logger;
    private readonly IPrimeNumbersRepository _primeNumbersRepository;

    private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 10000);
    public PrimeNumbersService(
        IMemoryCache cache,
        ILogger<PrimeNumbersService> logger,
        IMemoryCacheOptions cacheOptions,
        IPrimeNumbersRepository primeNumbersRepository
        )
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache)); ;
        _cacheOptions = cacheOptions ?? throw new ArgumentNullException(nameof(cacheOptions));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _primeNumbersRepository = primeNumbersRepository ?? throw new ArgumentNullException(nameof(primeNumbersRepository)); ;
    }
    public override async Task<PrimeNumberResponseDTO> GetPrimeNumbers(
        PrimeNumberRequestDTO request,
        ServerCallContext context)
    {
        TripTimeProvider.StartWatch();
        PrimeNumberResponseDTO response = new();
        string key = $"{request.Number}";
        try
        {
            await semaphore.WaitAsync();
            if (_cache.TryGetValue(key, out Result primeNumber) && primeNumber != null)
            {
                _logger.Log(LogLevel.Information, "Prime number found in cache");
                response = new PrimeNumberResponseDTO
                {
                    IsSuccessfull = true,
                    Result = new Result
                    {
                        IsPrime = primeNumber.IsPrime,
                        Number = request.Number
                    },
                    Error = new()
                };
                _primeNumbersRepository.AddNumber(request.Number, primeNumber.IsPrime);
            }
            else
            {
                _logger.Log(LogLevel.Information, "Prime number not found in cache");
                bool isCurrentNumberPrime = request.Number.IsPrime();
                response = new PrimeNumberResponseDTO
                {
                    IsSuccessfull = true,
                    Result = new Result
                    {
                        IsPrime = isCurrentNumberPrime,
                        Number = request.Number
                    },
                    Error = new()
                };
                _primeNumbersRepository.AddNumber(request.Number, isCurrentNumberPrime);
                var cacheEntryOptions = _cacheOptions.GetMemoryOptions(60, 3600, 1024, CacheItemPriority.Normal);
                
                //Add value in cache
                Result cacheResult = new()
                {
                    Number = request.Number,
                    IsPrime = isCurrentNumberPrime
                };
                _cache.Set(key, cacheResult, cacheEntryOptions);
            }
        }
        catch (Exception ex)
        {
            string message = ex.Message;
            response = new PrimeNumberResponseDTO
            {
                IsSuccessfull = false,
                Error = new Error
                {
                    Message = message,
                },
                Result = new(),
            };
            _logger.LogError($"Exception Details {message}");
        }
        finally
        {
            TripTimeProvider.StopWatch();
            double tripTimeMiliseconds = TripTimeProvider.RoundTripTime();
            response.RoundTripTime = tripTimeMiliseconds;
            semaphore.Release();
        }
        return await Task.FromResult(response);
    }
}
