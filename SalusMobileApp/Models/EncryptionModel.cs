using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SalusMobileApp.Models
{
    public class EncryptionModel
    {
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }
        public EncryptionModel(byte[] key, byte[] iV)
        {
            Key = key;
            IV = iV;
        }

        public EncryptionModel() { }

        //private static byte[] DeriveKeyFromPassword(string password)
        //{
            //var emptySalt = Array.Empty<byte>();
            //var iterations = 1000;
            //var desiredKeyLength = 16; // 16 bytes equal 128 bits.
            //var hashMethod = HashAlgorithmName.SHA384;
            //return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password),
            //                                 emptySalt,
            //                                 iterations,
            //                                 hashMethod,
            //                                 desiredKeyLength);
            
        //}
        public static async Task<byte[]> EncryptAsync(string password)
        {
            using Aes aes = Aes.Create();
            aes.Padding = PaddingMode.PKCS7;
            var saveEncryption = new EncryptionModel(aes.Key, aes.IV);
            App.database.SaveEncryptionData(saveEncryption);
            using MemoryStream output = new();
            using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
            await cryptoStream.WriteAsync(Encoding.Unicode.GetBytes(password));
            await cryptoStream.FlushFinalBlockAsync();
            return output.ToArray();
        }
        public static async Task<string> DecryptAsync(byte[] password)
        {
            using Aes aes = Aes.Create();
            aes.Padding = PaddingMode.PKCS7;
            //aes.Key = DeriveKeyFromPassword(Encoding.Unicode.GetString(password));
            //aes.IV = IV;
            var encryptionData = App.database.GetEncryptionData();
            aes.Key = encryptionData.Key;
            aes.IV = encryptionData.IV;
            using MemoryStream input = new(password);
            using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using MemoryStream output = new();
            await cryptoStream.CopyToAsync(output);
            return Encoding.Unicode.GetString(output.ToArray());
        }
    }
}
