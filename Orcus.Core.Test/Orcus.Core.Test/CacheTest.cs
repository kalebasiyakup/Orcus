using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orcus.Core.Cache.CacheInMemory;
using Orcus.Core.Cache.CacheKeys;
using Orcus.Core.Cache.Interface;

namespace Orcus.Core.Test
{
    [TestClass]
    public class CacheTest
    {
        [TestMethod]
        public void Cache_Test()
        {
            CacheProvider.Instance = new DefaultCacheProvider();
            CacheKey key = CacheKey.New("Customers", 0, "yakupk");
            if (CacheProvider.Instance.IsExist(key))
            {
                var retVal = CacheProvider.Instance.Get(key);
            }
            else
            {
                CacheProvider.Instance.Set(key, "Ali Baba");
            }

            Assert.AreEqual(CacheProvider.Instance.Get(key), "Ali Baba");
        }
    }
}
