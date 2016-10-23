using Orcus.Core.Cryptography.Abstract;

namespace Orcus.Core.Cryptography.Abstract
{
    public abstract class ICryphtographerAbstractFactory
    {
        public abstract IAsimetricCryphtographer GetAsimetricCryphtographer();
        public abstract ISimetricCryphtographer GetSimetricCryphtographer();
    }
}
