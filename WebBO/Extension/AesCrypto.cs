using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.Extension.StringEncrypt
{
    public static class AesCrypto
    {
        //先隨便輸入，後面要設置公司專用
        public static string AesKey = "1234567890ABCDEF"; //密鑰
        public static string AesIv = "1234567890ABCDEF"; //密鑰向量

        //解密資料
        public static string DecryptStringAES(string cipherText)
        {
            var keybytes = Encoding.UTF8.GetBytes(AesKey);   //自行設定，但要與JavaScript端 一致
            var iv = Encoding.UTF8.GetBytes(AesIv); // 自行設定，但要與JavaScript端 一致
            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            string plaintext = null;
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;
                rijAlg.Key = key;
                rijAlg.IV = iv;
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                try
                {
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }
            return plaintext;
        }



        /// <summary>
        /// AES 加密字串
        /// </summary>
        /// <param name="original">原始字串</param>
        /// <param name="key">自訂金鑰</param>
        /// <param name="iv">自訂向量</param>
        /// <returns></returns>
        public static string AesEncrypt(string original, string key = null, string iv = null)
        {
            key = string.IsNullOrEmpty(key) ? AesKey : key;
            iv = string.IsNullOrEmpty(iv) ? AesIv : iv;

            string encrypt = "";
            try
            {
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                byte[] keyData = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
                byte[] ivData = md5.ComputeHash(Encoding.UTF8.GetBytes(iv));
                byte[] dataByteArray = Encoding.UTF8.GetBytes(original);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (
                        CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(keyData, ivData), CryptoStreamMode.Write)
                    )
                    {
                        cs.Write(dataByteArray, 0, dataByteArray.Length);
                        cs.FlushFinalBlock();
                        encrypt = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                //todo...
            }

            return encrypt;
        }

        /// <summary>
        /// AES 解密字串
        /// </summary>
        /// <param name="hexString">已加密字串</param>
        /// <param name="key">自訂金鑰</param>
        /// <param name="iv">自訂向量</param>
        /// <returns></returns>
        public static string AesDecrypt(string hexString, string key = null, string iv = null)
        {
            key = string.IsNullOrEmpty(key) ? AesKey : key;
            iv = string.IsNullOrEmpty(iv) ? AesIv : iv;

            string decrypt = hexString;
            try
            {
                SymmetricAlgorithm aes = new AesCryptoServiceProvider();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                byte[] keyData = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
                byte[] ivData = md5.ComputeHash(Encoding.UTF8.GetBytes(iv));
                byte[] dataByteArray = Convert.FromBase64String(hexString);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (
                        CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(keyData, ivData), CryptoStreamMode.Write)
                    )
                    {
                        cs.Write(dataByteArray, 0, dataByteArray.Length);
                        cs.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                //todo...
            }

            return decrypt;
        }

        public static bool TryAesDecrypt(string hexString, out string original, string key = null, string iv = null)
        {
            return hexString != (original = AesDecrypt(hexString, key, iv));
        }

        /// <summary>
        /// AES 加密檔案
        /// </summary>
        /// <param name="sourceFile">原始檔案路徑</param>
        /// <param name="encryptFile">加密後檔案路徑</param>
        /// <param name="key">自訂金鑰</param>
        /// <param name="iv">自訂向量</param>
        public static bool AesEncryptFile(string sourceFile, string encryptFile, string key = null, string iv = null)
        {
            key = string.IsNullOrEmpty(key) ? AesKey : key;
            iv = string.IsNullOrEmpty(iv) ? AesIv : iv;

            if (string.IsNullOrEmpty(sourceFile) || string.IsNullOrEmpty(encryptFile) || !File.Exists(sourceFile))
            {
                return false;
            }

            try
            {
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                byte[] keyData = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
                byte[] ivData = md5.ComputeHash(Encoding.UTF8.GetBytes(iv));
                aes.Key = keyData;
                aes.IV = ivData;

                using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                {
                    using (FileStream encryptStream = new FileStream(encryptFile, FileMode.Create, FileAccess.Write))
                    {
                        //檔案加密
                        byte[] dataByteArray = new byte[sourceStream.Length];
                        sourceStream.Read(dataByteArray, 0, dataByteArray.Length);

                        using (CryptoStream cs = new CryptoStream(encryptStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(dataByteArray, 0, dataByteArray.Length);
                            cs.FlushFinalBlock();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //todo...
                return false;
            }

            return true;
        }

        /// <summary>
        /// AES 解密檔案
        /// </summary>
        /// <param name="encryptFile"></param>
        /// <param name="decryptFile"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static bool AesDecryptFile(string encryptFile, string decryptFile, string key = null, string iv = null)
        {
            key = string.IsNullOrEmpty(key) ? AesKey : key;
            iv = string.IsNullOrEmpty(iv) ? AesIv : iv;

            if (string.IsNullOrEmpty(encryptFile) || string.IsNullOrEmpty(decryptFile) || !File.Exists(encryptFile))
            {
                return false;
            }

            try
            {
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                byte[] keyData = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
                byte[] ivData = md5.ComputeHash(Encoding.UTF8.GetBytes(iv));
                aes.Key = keyData;
                aes.IV = ivData;

                using (FileStream encryptStream = new FileStream(encryptFile, FileMode.Open, FileAccess.Read))
                {
                    using (FileStream decryptStream = new FileStream(decryptFile, FileMode.Create, FileAccess.Write))
                    {
                        byte[] dataByteArray = new byte[encryptStream.Length];
                        encryptStream.Read(dataByteArray, 0, dataByteArray.Length);
                        using (CryptoStream cs = new CryptoStream(decryptStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(dataByteArray, 0, dataByteArray.Length);
                            cs.FlushFinalBlock();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //todo...
                return false;
            }

            return true;
        }


    }
}
