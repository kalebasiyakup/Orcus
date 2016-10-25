using Orcus.Core.Cache.CacheKeys;
using System;

namespace Orcus.Core.Cache.Interface
{
    public abstract class CacheProvider
    {
        public static int CacheDuration = 3600;
        public static CacheProvider Instance { get; set; }
        public abstract object Get(CacheKey key);
        public abstract void Set(CacheKey key, object value);
        public abstract void Set(CacheKey key, object value, DateTimeOffset absoluteExpiration);
        public abstract bool IsExist(CacheKey key);
        public abstract void Remove(CacheKey key);
    }
}
