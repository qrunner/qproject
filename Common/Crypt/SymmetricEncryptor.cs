using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Crypt
{
    public class SymmetricEncryptor : Encryptor
    {
        protected SymmetricAlgorithm sa = null;

        public SymmetricEncryptor(SymmetricAlgorythmType algType)
        {
            string algName = "Rijndael";
            switch(algType)
            {
                case SymmetricAlgorythmType.Rijndael:
                    algName = "Rijndael";
                    break;
                case SymmetricAlgorythmType.DES:
                    algName = "DES";
                    break;
                case SymmetricAlgorythmType.TripleDES:
                    algName = "TripleDES";
                    break;
                case SymmetricAlgorythmType.RC2:
                    algName = "RC2";
                    break;
            }
            SymmetricAlgorithm sa = SymmetricAlgorithm.Create(algName);
        }

        public SymmetricEncryptor(string algName)
        {
            SymmetricAlgorithm sa = SymmetricAlgorithm.Create(algName);
        }

        public SymmetricEncryptor()
        {
            SymmetricAlgorithm sa = SymmetricAlgorithm.Create();
        }

        public override byte[] GenerateKey()
        {
            sa.GenerateKey();
            return sa.Key;
        }

        public override byte[] Encrypt(byte[] source, byte[] key)
        {
            sa.Key = key;
            using (MemoryStream target = new MemoryStream())
            {
                sa.GenerateIV();
                target.Write(sa.IV, 0, sa.IV.Length);
                using (CryptoStream cs = new CryptoStream(target, sa.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(source, 0, source.Length);
                    cs.FlushFinalBlock();
                }
                return target.ToArray();
            }
        }

        public override byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            sa.Key = key;
            using (MemoryStream target = new MemoryStream())
            {
                byte[] iv = new byte[sa.IV.Length];
                Array.Copy(encrypted, iv, iv.Length);
                sa.IV = iv;
                int readStart = iv.Length;
                using (CryptoStream cs = new CryptoStream(target, sa.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(encrypted, readStart, encrypted.Length - readStart);
                    cs.FlushFinalBlock();
                    return target.ToArray();
                }
            }
        }
    }
}