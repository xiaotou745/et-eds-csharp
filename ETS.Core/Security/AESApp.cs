using ETS.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Security
{
    public class AESApp
    {
        const string AES_KEY = "k;)*(+nmjdsf$#@d";

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <returns>加密后字符串</returns>
        public static string AesEncrypt(string str)
        {
            try
            {
                string key = AES_KEY;
                //分组加密算法
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(str);//得到需要加密的字节数组 
                //设置密钥及密钥向量
                aes.Key = Encoding.UTF8.GetBytes(key);
                //aes.IV = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                byte[] cipherBytes = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        cipherBytes = ms.ToArray();//得到加密后的字节数组
                        cs.Close();
                        ms.Close();
                    }
                }
                return Convert.ToBase64String(cipherBytes);
            }
            catch { }
            return str;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="str">待解密字符串</param>
        /// <returns>解密后字符串</returns>
        public static string AesDecrypt(string str)
        {
            try
            {
                string key = AES_KEY;
                byte[] cipherText = Convert.FromBase64String(str);
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                aes.Key = Encoding.UTF8.GetBytes(key);
                //aes.IV = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                byte[] decryptBytes = new byte[cipherText.Length];
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        cs.Read(decryptBytes, 0, decryptBytes.Length);
                        cs.Close();
                        ms.Close();
                    }
                }
                return Encoding.UTF8.GetString(decryptBytes).Replace("\0", "");   //将字符串后尾的'\0'去掉
            }
            catch { }
            return str;
        }

        /// <summary>
        /// 通过加密键验证是否允许操作数据库
        /// </summary>
        /// <param name="key">去掉1 3 6内容再MD5</param>
        /// <param name="val">客户端传过来key去掉1 3 6的值</param>
        /// <returns></returns>
        public static bool CheckAES(string key, string val)
        {
            key = key.Length >= 6 ? key.Remove(6) : key;
            key = key.Length >= 3 ? key.Remove(3) : key;
            key = key.Length >= 1 ? key.Remove(1) : key;
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(val))
            {
                return false;
            }
            if (MD5Helper.MD5(key) == val)
            {
                return true;
            }
            return false;
        }
    }
}
