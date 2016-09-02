using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace NHS111.Utils.Cache
{
    //public class CacheManager : ICacheManager
    //{
    //    private CacheItemPolicy _cacheItemPolicy;

    //    public CacheManager()
    //    {
    //        _cacheItemPolicy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(6000.0) };

    //    }

    //    public async void Set(string key, string value)
    //    {
    //        var item = new CacheItem(key, value);
    //        await Task.Run(() => MemoryCache.Default.Set(item, _cacheItemPolicy));
    //    }
    //    public Task<string> Read(string key)
    //    {
    //        return Task.Run(() => MemoryCache.Default.Get(key).ToString());
    //    }
    //}
}