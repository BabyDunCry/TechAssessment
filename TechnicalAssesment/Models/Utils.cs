using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TechnicalAssesment.Models
{
    public class Utils
    {
        public static string AesEncryption(string toEncrypt, string key)
        {
            var KeyBytes = Convert.FromBase64String(key);
            byte[] keyArray = KeyBytes;
            byte[] ivArray = new byte[] { 0, 2, 0, 3, 0, 4, 0, 5, 0, 6, 0, 7, 0, 8, 0, 9 };
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string AesDecryption(string plainText, string key)
        {
            try
            {
                RijndaelManaged aes = new RijndaelManaged();
                var KeyBytes = Convert.FromBase64String(key);
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                aes.Key = KeyBytes;
                aes.IV = new byte[] { 0, 2, 0, 3, 0, 4, 0, 5, 0, 6, 0, 7, 0, 8, 0, 9 };
                ICryptoTransform AESDecrypt = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] buffer = Convert.FromBase64String(plainText);
                var newKey = Convert.ToBase64String(AESDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
                return newKey;
            }
            catch (Exception e)
            {
                throw new Exception("Error decrypting: " + e.Message);
            }
        }

    }
}