using GprcServer.Abstractions;

namespace GprcServer.Jobs;
public class PrimeNumbersJob : BackgroundService
{
    private readonly TimeSpan _period = TimeSpan.FromSeconds(1);
    private readonly ILogger<PrimeNumbersJob> _logger;
    private readonly IPrimeNumbersRepository _primeNumbersRepository;

    public PrimeNumbersJob(
        IPrimeNumbersRepository primeNumbersRepository,
        ILogger<PrimeNumbersJob> logger)
    {
        _primeNumbersRepository = primeNumbersRepository;
        _logger = logger;
    }

    protected override async Task<Task> ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(_period);

        while (!stoppingToken.IsCancellationRequested &&
               await timer.WaitForNextTickAsync(stoppingToken))
        {
            long totalRequests = _primeNumbersRepository.GetTotalRequestsCount();
            string? topRated = _primeNumbersRepository.GetTopRated();

            _logger.LogInformation($"Total requests recieved : {totalRequests}");

            if (!String.IsNullOrEmpty(topRated))
            {
                _logger.LogInformation($"Top 10 requested prime numbers : {topRated}");
            }
        }

        return Task.CompletedTask;
    }

}