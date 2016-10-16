namespace Orcus.Cryptography
{
    //Abstract Factory Tasarım Deseni Kullanılmıştır.
    //Şifreleme tipleri Simetrik ve Asimetrik olmak üzere
    //iki şifreci grubu olarak düşünülmüştür.
    public abstract class ICryphtographerAbstractFactory
    {
        public abstract IAsimetricCryphtographer GetAsimetricCryphtographer();
        public abstract ISimetricCryphtographer GetSimetricCryphtographer();
    }
}
