namespace Orcus.Core.Cryptography.ParameterDTO
{
    public class CryphtographerParameter
    {
        public int KeySize { get; set; }
        public string Key { get; set; }

        /// <summary>
        /// tavsiye 128 -> For AES
        /// </summary>
        public int BlockBitSize { get; set; }
        /// <summary>
        /// tavsiye 64 -> For AES
        /// </summary>
        public int SaltBitSize { get; set; }
        /// <summary>
        /// tavsiye 10000 -> For AES
        /// </summary>
        public int Iterations { get; set; }
        /// <summary>
        /// tavsiye 12 -> For AES
        /// </summary>
        public int MinPasswordLength { get; set; }
        /// <summary>
        /// 16 byte olmalı -> For rijndael
        /// </summary>
        public string InitVector { get; set; }
        /// <summary>
        /// Herhangi bir rakam olabilir. Ne kadar büyük, o kadar güvenli ama yavai. tavsiye (2-10) -> For Rijndael
        /// </summary>
        public int PasswordIterations { get; set; }
    }
}
