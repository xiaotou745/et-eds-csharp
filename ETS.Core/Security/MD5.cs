using System.Security.Cryptography;
using System.Text;
using System;

namespace ETS.Security
{
	/// <summary>
	/// MD5加密
	/// </summary>
	public class MD5
	{
		/// <summary>
		/// 字符串MD5加密，返回大写字母
		/// </summary>
		/// <param name="plainText">明文</param>
		/// <returns>密文（大写字母）</returns>
        private static readonly string saltVal = "";
        // Hash an input string and return the hash as
        // a 32 character hexadecimal string.

        public static string GetMD5Hash(string input)
        {
            input = input + saltVal;
            System.Security.Cryptography.MD5 md5Hasher = System.Security.Cryptography.MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// 将输入字符串用md5加密并返回
        /// </summary>
        /// <param name="Str">输入字符串</param>
        /// <returns>返回字符串</returns>
        public static string GetMd5String(string Str)
        {
            //32位md5加密算法
            byte[] byteString = null;
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byteString = md5.ComputeHash(Encoding.Unicode.GetBytes(Str));
            md5.Clear();

            string NewStr = Convert.ToBase64String(byteString);

            return NewStr;
        }

        // Verify a hash against a string.
        public static bool VerifyMD5Hash(string input, string hash)
        {
            string hashOfInput = GetMD5Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);
        }

        public static string Encrypt(string plainText)
		{
			var md = new MD5CryptoServiceProvider();
			byte[] b = md.ComputeHash(Encoding.Default.GetBytes(plainText));
			string s = string.Empty;
			for (int i = 0; i < b.Length; i++)
			{
				s += (b[i].ToString("x2"));
			}
			return s.ToUpper();
		}
	}
}