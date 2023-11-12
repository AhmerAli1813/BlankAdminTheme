using DE.Infrastructure.Memory;
using System;
using System.Runtime.Caching;

namespace DE.Infrastructure.MemoryImplementation
{
    public class MemoryCacher : IMemoryCacher
    {
        readonly MemoryCache _memoryCache;
        public MemoryCacher()
        {
            _memoryCache = MemoryCache.Default;
        }
        public object GetValue(string key)
        {

            return _memoryCache.Get(key);
        }

        public T GetValue<T>(string key)
        {
            return (T)_memoryCache.Get(key);
        }

        public bool Add(string key, object value, int durationInMinutes)
        {
            var absExpiration = DateTimeOffset.Now.AddMinutes(durationInMinutes);
            return _memoryCache.Add(key, value, absExpiration);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
