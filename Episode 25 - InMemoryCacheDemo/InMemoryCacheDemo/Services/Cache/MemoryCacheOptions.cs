using InMemoryCacheDemo.Abstractions.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCacheDemo.Services.Cache
{
    public class MemoryCacheOptions : IMemoryCacheOptions
    {
        public MemoryCacheEntryOptions? GetMemoryOptions(
           int slidingExpirationTime,
           int absoluteExpirationTime,
           int cacheEntryValue,
           CacheItemPriority priority = CacheItemPriority.Normal)
        {

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetSlidingExpiration(TimeSpan.FromSeconds(slidingExpirationTime))
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(absoluteExpirationTime))
                  .SetPriority(priority)
                  .SetSize(cacheEntryValue);

            return cacheEntryOptions;
        }
    }
}
