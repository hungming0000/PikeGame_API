using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.Extension.StringEncrypt
{
    public static class _3DESCrypto
    {
        //先隨便輸入，後面要設置公司專用
        public static string AesKey = "1234567890ABCDEF"; //密鑰16碼
        public static string AesIv = "12345678"; //密鑰向量8碼

        //3DES的cbc加密[24位密鑰對應192位加密]
        public static string TripleDesEncryptorCBC(string text)
        {
            var tripleDESCipher = new TripleDESCryptoServiceProvider();
            tripleDESCipher.Mode = CipherMode.CBC;
            tripleDESCipher.Padding = PaddingMode.PKCS7;
            byte[] pwdBytes = Encoding.UTF8.GetBytes(AesKey);
            byte[] keyBytes = new byte[24];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
                len = keyBytes.Length;
            System.Array.Copy(pwdBytes, keyBytes, len);
            tripleDESCipher.Key = keyBytes;
            tripleDESCipher.IV = Encoding.ASCII.GetBytes(AesIv);

            ICryptoTransform transform = tripleDESCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(text);
            byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);
            return Convert.ToBase64String(cipherBytes);
        }

        //3DES的cbc解密
        public static string TripleDesDecryptorCBC(string text)
        {
            var tripleDESCipher = new TripleDESCryptoServiceProvider();
            tripleDESCipher.Mode = CipherMode.CBC;
            tripleDESCipher.Padding = PaddingMode.PKCS7;

            byte[] encryptedData = Convert.FromBase64String(text);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(AesKey);
            byte[] keyBytes = new byte[24];            
            byte[] ivBytes = Encoding.UTF8.GetBytes(AesIv);
            //byte[] ivBytes = Encoding.ASCII.GetBytes(AesIv);
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
                len = keyBytes.Length;
            //System.Array.Copy(pwdBytes, keyBytes, len);         
            tripleDESCipher.Key = pwdBytes;
            tripleDESCipher.IV = ivBytes;
            ICryptoTransform transform = tripleDESCipher.CreateDecryptor();
            byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(plainText);
        }

    }
}
