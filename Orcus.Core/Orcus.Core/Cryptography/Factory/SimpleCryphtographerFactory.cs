using Orcus.Core.Cryptography.Abstract;
using Orcus.Core.Cryptography.Asimetric;
using Orcus.Core.Cryptography.ParameterDTO;
using Orcus.Core.Cryptography.Simetric;

namespace Orcus.Core.Cryptography.Factory
{
    //Abstract factory'nin uygulanışı gereği
    //İçeriğinde simetrik ve asimetrik şifreci dönen ICryphtographerAbstractFactory den inherit
    //alan bir fabrika classı yazılır.
    //AES ve MD5 algoritmaları çözülmesi daha kolay olduğundan Bu gruba koyduk.
    //Siz de kendi gruplarınızı oluşturmak için ICryphtographerAbstractFactory sınıfından 
    //inherit alan classlar yazabilirsiniz
    public class SimpleCryphtographerFactory : ICryphtographerAbstractFactory
    {
        #region Private Members

        private int keySize;
        private string key;
        private CryphtographerParameter parameter;

        #endregion

        #region Yapıcılar

        public SimpleCryphtographerFactory(CryphtographerParameter parameter)
        {
            this.parameter = parameter;
        }
        public SimpleCryphtographerFactory(int keySize, string key)
        {
            this.key = key;
            this.keySize = keySize;
        }

        #endregion

        public override IAsimetricCryphtographer GetAsimetricCryphtographer()
        {
            return new MD5Cryphtographer();
        }

        public override ISimetricCryphtographer GetSimetricCryphtographer()
        {
            if (parameter != null)
                return new AESCryptographer(parameter);
            else
                return new AESCryptographer(keySize, key);
        }
    }
}
