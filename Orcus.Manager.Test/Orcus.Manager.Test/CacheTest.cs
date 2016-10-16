using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Diagnostics;

namespace Orcus.Manager.Test
{
    [TestClass]
    public class CacheTest
    {
        [TestMethod]
        public void EklevVeVeyaGunlemmeBasariliMi()
        {
            Assert.IsTrue(CacheManager.Instance.AddOrUpdate("Context","deneme"));
            Assert.IsNotNull(CacheManager.Instance.Get("Context"));

            Assert.IsTrue(CacheManager.Instance.AddOrUpdate("Context","deneme2"));
            Assert.IsNotNull(CacheManager.Instance.Get("Context"));
        }

        [TestMethod]
        public void HazfizadanGetirBasariliMi()
        {
            Assert.IsTrue(CacheManager.Instance.AddOrUpdate("Deneme", "Aloooo", new TimeSpan(0, 0, 10)));
            Assert.IsNotNull(CacheManager.Instance.Get("Deneme"));
            Trace.WriteLine(DateTime.Now.ToString());
            Assert.IsNull(CacheManager.Instance.Get("Deneme"));
            
        }

        [TestMethod]
        public void HazfizadanGetirBasariliMi2()
        {
            Assert.IsTrue(CacheManager.Instance.AddOrUpdate("Deneme", "Aloooo", new TimeSpan(0, 0, 10)));
            Assert.IsNotNull(CacheManager.Instance.Get("Deneme"));
            Trace.WriteLine(DateTime.Now.ToString());
            Assert.IsNull(CacheManager.Instance.Get("Deneme"));

        }
    }
}
