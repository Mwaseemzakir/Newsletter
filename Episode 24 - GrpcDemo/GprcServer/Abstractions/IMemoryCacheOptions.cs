using Microsoft.Extensions.Caching.Memory;

namespace GprcServer.Abstractions;
public interface IMemoryCacheOptions
{
    public MemoryCacheEntryOptions? GetMemoryOptions(
          int slidingExpirationTime,
          int absoluteExpirationTime,
          int cacheEntryValue,
          CacheItemPriority priority = CacheItemPriority.Normal);
}
