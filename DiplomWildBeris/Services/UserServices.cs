using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWildBeris.Services
{
    public interface IUserService
    {
        string Encrypt(string plaintext);

        string Decrypt(string plaintext);

    }



    public class UserServices(IConfiguration config) : IUserService
    {
        private readonly UserSetting _setting = config.GetRequiredSection("UserSetting").Get<UserSetting>()!;



        public string Decrypt(string cipherText)
        {
            try
            {
                using Aes aesAlg = Aes.Create();

                string[] parts = cipherText.Split(':');
                byte[] iv = Convert.FromBase64String(parts[0]);

                aesAlg.Key = Encoding.UTF8.GetBytes(_setting.Key);
                aesAlg.IV = iv;

                byte[] cipherBytes = Convert.FromBase64String(parts[1]);
                ICryptoTransform cryptoTransform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using MemoryStream ms = new(cipherBytes, 0, cipherBytes.Length);
                using CryptoStream csDecrypt = new(ms, cryptoTransform, CryptoStreamMode.Read);
                using StreamReader sr = new(csDecrypt);
                return sr.ReadToEnd();
            }
            catch { throw; }
        }

        public string Encrypt(string password)
        {
            try
            {
                using Aes alg = Aes.Create();
                // Генерация случайного IV
                byte[] iv = new byte[alg.BlockSize / 8];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(iv);
                }
                alg.Key = System.Text.Encoding.UTF8.GetBytes(_setting.Key);
                alg.IV = iv;

                ICryptoTransform cryptoTransform = alg.CreateEncryptor(alg.Key, alg.IV);

                using MemoryStream ms = new();
                using (CryptoStream cs = new(ms, cryptoTransform, CryptoStreamMode.Write))
                {
                    using StreamWriter streamWriter = new(cs);
                    streamWriter.Write(password);
                }
                // Возвращаем IV, зашифрованные данные и их длину в формате, который позволит нам разделить их при дешифровании
                return Convert.ToBase64String(iv) + ":" + Convert.ToBase64String(ms.ToArray());
            }
            catch
            {
                throw;
            }
        }

    }
}
