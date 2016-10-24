using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orcus.Core.Cryptography.Manager;
using Orcus.Core.Cryptography.Enum;
using Orcus.Core.Cryptography.ParameterDTO;

namespace Orcus.Core.Test
{
    [TestClass]
    public class CryptographyTest
    {
        [TestMethod]
        public void SimpleAsimetricCalisiyorMu()
        {
            CryptographerManager manager = new CryptographerManager(CryptoAlgorithmType.Simple);
            string cipherText = manager.EncryptAsimetric("Yakup_Kalebasi");
            string cipherText2 = manager.EncryptAsimetric("Yakup_Kalebasi");
            Assert.AreEqual(cipherText, cipherText2);
        }

        [TestMethod]
        public void SimpleSimetricCalisiyorMu()
        {
            CryptographerManager manager = new CryptographerManager(CryptoAlgorithmType.Simple);
            string cipherText = manager.EncryptSimetric("Yakup_Kalebasi");
            string plainText = manager.DecryptSimetric(cipherText);
            Assert.AreEqual(plainText, "Yakup_Kalebasi");
        }

        [TestMethod]
        public void SimpleSimetricParametreliCalisiyorMu()
        {
            CryphtographerParameter parameter = new CryphtographerParameter
            {
                BlockBitSize = 128,
                Iterations = 40,
                MinPasswordLength = 12,
                SaltBitSize = 128,
                Key = "123456789012345678901234567890AE",
                KeySize = 256
            };
            CryptographerManager manager = new CryptographerManager(CryptoAlgorithmType.Simple, parameter);
            string cipherText = manager.EncryptSimetric("Yakup_Kalebasi");
            string plainText = manager.DecryptSimetric(cipherText);
            Assert.AreEqual(plainText, "Yakup_Kalebasi");
        }

        [TestMethod]
        public void ComplicateAsimetricCalisiyorMu()
        {
            CryptographerManager manager = new CryptographerManager(CryptoAlgorithmType.Complicate);
            string cipherText = manager.EncryptAsimetric("Yakup_Kalebasi");
            string cipherText2 = manager.EncryptAsimetric("Yakup_Kalebasi");
            Assert.AreEqual(cipherText, cipherText2);
        }

        [TestMethod]
        public void ComplicateSimetricCalisiyorMu()
        {
            CryptographerManager manager = new CryptographerManager(CryptoAlgorithmType.Complicate);
            string cipherText = manager.EncryptSimetric("Yakup_Kalebasi");
            string plainText = manager.DecryptSimetric(cipherText);
            Assert.AreEqual(plainText, "Yakup_Kalebasi");
        }

        [TestMethod]
        public void ComplicateSimetrikParametreliCalisiyorMu()
        {
            CryphtographerParameter parameter = new CryphtographerParameter();
            parameter.InitVector = "1234567890123456";
            parameter.PasswordIterations = 3;
            parameter.KeySize = 256;
            parameter.Key = "123456789012345678901234567890AE";

            //defaultta rijndal geliyor
            CryptographerManager manager = new CryptographerManager(CryptoAlgorithmType.Complicate, parameter);
            string cipherText = manager.EncryptSimetric("Yakup_Kalebasi");
            string plainText = manager.DecryptSimetric(cipherText);
            Assert.AreEqual(plainText, "Yakup_Kalebasi");
        }
    }
}
