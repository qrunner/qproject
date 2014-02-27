using System.Security.Cryptography;
using System.Text;

namespace Crypt
{
    public class HashGenerator
    {
        HashAlgorithm ha = null;

        public HashGenerator()
        {
            ha = HashAlgorithm.Create();
        }

        public HashGenerator(string algName)
        {
            ha = HashAlgorithm.Create(algName);
        }

        public byte[] GetHash(byte[] source)
        {
            return ha.ComputeHash(source);
        }

        public string GetHash(string source, Encoding encoding)
        {
            return encoding.GetString(ha.ComputeHash(encoding.GetBytes(source)));
        }

        public string GetHash(string source)
        {
            return GetHash(source, Encoding.UTF8);
        }
    }
}
