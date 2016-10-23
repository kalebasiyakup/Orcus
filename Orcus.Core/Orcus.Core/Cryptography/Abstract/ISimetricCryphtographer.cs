namespace Orcus.Core.Cryptography.Abstract
{
    public abstract class ISimetricCryphtographer
    {
        private int keySize; //her simetrik şifreci için bir anahtar boyutu olmalıdır.
        private string key; //her simetrik şifreleme bir anahtar aracılığıyla yapılır

        internal ISimetricCryphtographer(int keySize, string key)
        {
            this.keySize = keySize;
            this.key = key;
        }
        internal virtual void SetKey(int keySize, string key)
        {
            this.keySize = keySize;
            this.key = key;
        }
        internal virtual int GetKeySize()
        {
            return this.keySize;
        }
        internal virtual string GetKey()
        {
            return this.key;
        }

        public abstract string Encrypt(string plainText);
        public abstract string Decrypt(string encryptedText);
    }
}
