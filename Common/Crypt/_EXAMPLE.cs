using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Crypt
{
    class _EXAMPLE
    {
        public string Do(string source)
        {
            // создаем класс для шифрования
            Encryptor enc = new SymmetricEncryptor(SymmetricAlgorythmType.Rijndael);
            // задаем ключ
            byte[] key = new byte[1] { 1 };
            // шифруем 
            string encrypted = enc.Encrypt(source, key);
            // дешифруем
            string twoWayEncrypted = enc.Decrypt(encrypted, key);

            Debug.Assert(twoWayEncrypted == source, "Строка после шифрования/дешифрования не равна исходной");

            // создаем класс хеша
            HashGenerator hash = new HashGenerator();
            // получаем хеш
            return hash.GetHash(source);
        }
    }
}