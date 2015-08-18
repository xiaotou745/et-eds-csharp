using System;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;

namespace ETS.Security
{
    /// <summary>
    /// 3DES加密/解密
    /// </summary>
    public class DES
    {
		//private const string DES_KEY = "FDAJjjiio&&(*777762&&^J::JK:JDFSAew4554";
        //private const string DES_KEY = "FDSFIojslsk;fjlk;)*(+nmjdsf$#@dsf54641#&*(()";
        private const string DES_KEY = "f83834a7bc7fb26ae7535c7e651554c5";


        private static readonly string EncryptionKey = "meydarin";
        private static readonly string EncryptionIV = "y3ee2h1a";
        private static readonly string MultipleEncryptionKey1 = "ukeivtzb";
        private static readonly string MultipleEncryptionKey2 = "olyckeyc";
        private static readonly string MultipleEncryptionKey3 = "pqmvidkx";

        private static readonly string MultipleEncryptionIV1 = "fjnmkola";
        private static readonly string MultipleEncryptionIV2 = "weicapmv";
        private static readonly string MultipleEncryptionIV3 = "cinepzgv";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encrypt(string text)
        {
            return Encrypt(text, EncryptionKey, EncryptionIV);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="encryptionIV"></param>
        /// <returns></returns>
        public static string Encrypt(string text, string encryptionKey, string encryptionIV)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            else
            {
                byte[] data = Encoding.UTF8.GetBytes(text);
                byte[] result = null;

                using (DESCryptoServiceProvider DES = new DESCryptoServiceProvider())
                {
                    DES.Key = Encoding.UTF8.GetBytes(encryptionKey);
                    DES.IV = Encoding.UTF8.GetBytes(encryptionIV);
                    ICryptoTransform desencrypt = DES.CreateEncryptor();
                    result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                }
                return BitConverter.ToString(result).Replace("-", "");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Decrypt(string text)
        {
            return Decrypt(text, EncryptionKey, EncryptionIV);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="encryptionIV"></param>
        /// <returns></returns>
        public static string Decrypt(string text, string encryptionKey, string encryptionIV)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            else
            {
                byte[] data = new byte[text.Length / 2];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = byte.Parse(text.Substring(2 * i, 2), NumberStyles.HexNumber);
                }

                byte[] result = null;
                using (DESCryptoServiceProvider DES = new DESCryptoServiceProvider())
                {
                    DES.Key = Encoding.UTF8.GetBytes(encryptionKey);
                    DES.IV = Encoding.UTF8.GetBytes(encryptionIV);
                    ICryptoTransform desencrypt = DES.CreateDecryptor();
                    result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                }
                return Encoding.UTF8.GetString(result);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string TripleEncrypt(string text)
        {
            string text2 = Encrypt(text, MultipleEncryptionKey1, MultipleEncryptionIV1);
            string text3 = Encrypt(text2, MultipleEncryptionKey2, MultipleEncryptionIV2);
            return Encrypt(text3, MultipleEncryptionKey3, MultipleEncryptionIV3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string TripleDecrypt(string text)
        {
            string text2 = Decrypt(text, MultipleEncryptionKey3, MultipleEncryptionIV3);
            string text1 = Decrypt(text2, MultipleEncryptionKey2, MultipleEncryptionIV2);
            return Decrypt(text1, MultipleEncryptionKey1, MultipleEncryptionIV1);
        }
        /// <summary>
        /// 3des加密字符串
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>加密后并经base63编码的字符串</returns>
        /// <remarks>重载，指定编码方式</remarks>
        public static string Encrypt3DES(string plainText, Encoding encoding)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;
            var DES = new
                TripleDESCryptoServiceProvider();
            var hashMD5 = new MD5CryptoServiceProvider();

            DES.Key = hashMD5.ComputeHash(encoding.GetBytes(DES_KEY));
            DES.Mode = CipherMode.ECB;

            ICryptoTransform DESEncrypt = DES.CreateEncryptor();

            byte[] Buffer = encoding.GetBytes(plainText);
            return Convert.ToBase64String(DESEncrypt.TransformFinalBlock
                                              (Buffer, 0, Buffer.Length));
        }

        /// <summary>
        /// 3des加密字符串
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>加密后并经base63编码的字符串</returns>
        public static string Encrypt3DES(string plainText)
        {
            return Encrypt3DES(plainText, Encoding.Default);
        }

        /// <summary>
        /// 3des解密字符串
        /// </summary>
        /// <param name="entryptText">密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt3DES(string entryptText)
        {
            return Decrypt3DES(entryptText, Encoding.Default);
        }

        /// <summary>
        /// 3des解密字符串
        /// </summary>
        /// <param name="entryptText">密文</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>解密后的字符串</returns>
        /// <remarks>静态方法，指定编码方式</remarks>
        public static string Decrypt3DES(string entryptText, Encoding encoding)
        {
            var DES = new
                TripleDESCryptoServiceProvider();
            var hashMD5 = new MD5CryptoServiceProvider();

            DES.Key = hashMD5.ComputeHash(encoding.GetBytes(DES_KEY));
            DES.Mode = CipherMode.ECB;

            ICryptoTransform DESDecrypt = DES.CreateDecryptor();

            string result;
            try
            {
                byte[] Buffer = Convert.FromBase64String(entryptText);
                result = encoding.GetString(DESDecrypt.TransformFinalBlock
                                                (Buffer, 0, Buffer.Length));
            }
            catch (Exception e)
            {
                throw (new Exception("Invalid Key or input string is not a valid base64 string", e));
            }
            return result;
        }
    }
}