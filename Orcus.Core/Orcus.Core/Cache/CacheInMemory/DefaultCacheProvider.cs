using Orcus.Core.Cache.CacheKeys;
using Orcus.Core.Cache.Interface;
using System;
using System.Linq;
using System.Runtime.Caching;

namespace Orcus.Core.Cache.CacheInMemory
{
    public class DefaultCacheProvider : CacheProvider
    {
        private ObjectCache _cache = null;
        private CacheItemPolicy _policy
        {
            get
            {
                return new CacheItemPolicy
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(CacheDuration)),
                    RemovedCallback = new CacheEntryRemovedCallback(CacheRemovedCallback)
                };
            }
        }

        public DefaultCacheProvider()
        {
            _cache = MemoryCache.Default;
        }

        private static void CacheRemovedCallback(CacheEntryRemovedArguments arguments)
        {
            System.Diagnostics.Trace.WriteLine("----------Cache Expire Oldu----------");
            System.Diagnostics.Trace.WriteLine("Key : " + arguments.CacheItem.Key);
            System.Diagnostics.Trace.WriteLine("Value : " + arguments.CacheItem.Value.ToString());
            System.Diagnostics.Trace.WriteLine("RemovedReason : " + arguments.RemovedReason);
            System.Diagnostics.Trace.WriteLine("-------------------------------------");
        }

        public override object Get(CacheKey key)
        {
            object retVal = null;

            try
            {
                retVal = _cache.Get(key.ToString());
            }
            catch (Exception e)
            {
                throw new Exception("Cache Get sırasında bir hata oluştu!", e);
            }

            return retVal;
        }

        public override void Set(CacheKey key, object value)
        {
            try
            {
                _cache.Set(key.ToString(), value, _policy);
            }
            catch (Exception e)
            {
                throw new Exception("Cache Set sırasında bir hata oluştu!", e);
            }
        }

        public override void Set(CacheKey key, object value, DateTimeOffset absoluteExpiration)
        {
            try
            {
                _cache.Set(key.ToString(), value, absoluteExpiration);
            }
            catch (Exception e)
            {
                throw new Exception("Cache Set sırasında bir hata oluştu!", e);
            }
        }

        public override bool IsExist(CacheKey key)
        {
            return _cache.Any(q => q.Key == key.ToString());
        }

        public override void Remove(CacheKey key)
        {
            _cache.Remove(key.ToString());
        }
    }
}
