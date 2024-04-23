using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace FugleNET.Keyrings
{
    internal class ArgonAESEncryption
    {
        public const int DefaultTimeCost = 15;
        public const int DefaultMemoryCost = 1 << 16;
        public const int DefaultParallelism = 2;

        public int Parallelism { get; set; } = DefaultParallelism;

        public int TimeCost { get; set; } = DefaultTimeCost;

        public int MemoryCost { get; set; } = DefaultMemoryCost;


        public AesGcm CreateCipher(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = Parallelism,
                MemorySize = MemoryCost,
                Iterations = TimeCost
            };

            return new AesGcm(argon2.GetBytes(16));
        }
    }
}
