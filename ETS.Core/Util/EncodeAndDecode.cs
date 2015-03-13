using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace ETS.Util
{
    public class EncodeAndDecode
    {
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="codeName">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string EncodeBase64(Encoding encode, string source)
        {
            string enstring = "";
            byte[] bytes = encode.GetBytes(source);
            try
            {
                enstring = Convert.ToBase64String(bytes);
            }
            catch
            {
                enstring = source;
            }
            return enstring;
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeBase64(string source)
        {
            return EncodeBase64(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }


        /// <summary>
        ///     加密
        /// </summary>
        /// <param name="aStrString">加密字符串</param>
        /// <param name="aStrKey">密匙</param>
        /// <returns>返回加密后字符串</returns>
        public static string Encrypt3Des(string aStrString, string aStrKey)
        {
            var des = new TripleDESCryptoServiceProvider();
            var provider2 = new MD5CryptoServiceProvider();
            des.Key = provider2.ComputeHash(Encoding.ASCII.GetBytes(aStrKey));
            des.Mode = CipherMode.ECB;
            var desEncrypt = des.CreateEncryptor();
            var buffer = Encoding.ASCII.GetBytes(aStrString);
            return Convert.ToBase64String(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        /// <summary>
        ///     3DES解密
        /// </summary>
        /// <param name="strString">加密后的字符串</param>
        /// <param name="strKey">解密key</param>
        /// <returns>返回明文密码</returns>
        public static string Decrypt3Des(string strString, string strKey)
        {
            var provider1 = new TripleDESCryptoServiceProvider();
            var provider2 = new MD5CryptoServiceProvider();
            provider1.Key = provider2.ComputeHash(Encoding.ASCII.GetBytes(strKey));
            provider1.Mode = CipherMode.ECB;
            var transform1 = provider1.CreateDecryptor();
            var buffer1 = Convert.FromBase64String(strString);
            var text1 = Encoding.ASCII.GetString(transform1.TransformFinalBlock(buffer1, 0, buffer1.Length));
            return text1;
        }

    }
}
