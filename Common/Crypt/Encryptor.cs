using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crypt
{
    public abstract class Encryptor
    {
        public virtual byte[] GenerateKey() { return null; }

        public virtual byte[] Encrypt(byte[] source, byte[] key) { return null; }

        public string Encrypt(string source, byte[] key)
        {
            return UTF8Encoding.UTF8.GetString(Encrypt(UTF8Encoding.UTF8.GetBytes(source), key));
        }

        public virtual byte[] Decrypt(byte[] encrypted, byte[] key) { return null; }

        public string Decrypt(string encrypted, byte[] key)
        {
            return UTF8Encoding.UTF8.GetString(Decrypt(UTF8Encoding.UTF8.GetBytes(encrypted), key));
        }
    }
}