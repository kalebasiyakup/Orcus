namespace Orcus.Cryptography
{
    //Geri dönüşü olmayan şifreleme tiplerinin
    //miras alacağı abstract class
    //siz de kendi geri dönüşü olmayan(asimetrik) şifrecilerinizi
    //bu class dan türeterek genişletebilirsiniz.
    public abstract class IAsimetricCryphtographer
    {
        public abstract string Encrypt(string plainText);
    }
}
