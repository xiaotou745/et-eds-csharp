using System;
using System.Text.RegularExpressions;

namespace Letao.Util
{
    public class StringVerify
    {
        /// <summary>
        /// 乐淘支持的手机号码
        /// </summary>
        private static string strMobile = @"(^(\+86)?1[3,4,5,6,8](\d{9})$)";

        /// <summary>
        /// 乐淘支持的座机号码
        /// </summary>
        private static string strTelephone = @"(^(\((\d{2,5})\)|\d{2,5})?(\s*)(-?)(\s*)(\d{5,9})$)";

        /// <summary>
        /// 乐淘支持的邮件地址格式
        /// </summary>
        private static string strEmail = @"^([\w\.\-]+)@((([A-Za-z0-9\-]+)\.)+)[A-Za-z]{2,4}$";
        /// <summary>
        /// 判断一个输入字符串是否合法的商品ID，合法的商品ID是全数字或者“数字_数字”
        /// </summary>
        /// <param name="inStr">输入的字符串</param>
        /// <returns>true表示合法</returns>
        public static bool isValidProductID(string inStr)
        {
            if (string.IsNullOrWhiteSpace(inStr) || inStr.Length < 6)
                return false;

            if (inStr.IndexOf('_') == -1)
                return isAllNumber(inStr);
            else
                return (isAllNumber(inStr.Substring(0, inStr.IndexOf('_'))) && isAllNumber(inStr.Substring(inStr.IndexOf('_') + 1)));
        }

        /// <summary>
        /// 判断一个输入字符串是否合法的电话号码（只能含有数字，空格，括号，减号）
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isValidPhoneNumber(string inStr)
        {
            if (inStr == null || inStr.Trim().Length == 0)
                return false;

            for (int i = 0; i < inStr.Length; i++)
            {
                if (!IsNumber(inStr, i) && inStr[i] != ' ' && inStr[i] != '-' && inStr[i] != '(' && inStr[i] != ')' && inStr[i] != '（' && inStr[i] != '）')
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 判断一个输入字符串是否是一个合法的浮点数
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isValidDouble(string inStr)
        {
            if (inStr == null || inStr.Trim().Length == 0)
                return false;

            try
            {
                double ddd = double.Parse(inStr.Trim());
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 对于一些输入，将其中可能造成问题的字符替换为下横线，目前仅包括单引号
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        /// 
        private static string _SpaceMatcher(Match m)
        {
            GroupCollection gc = m.Groups;
            return null;
        }

        private static string[] spacePattern = new string[] { 
            null,   //默认的
            @"(\s{2,})"   //字符之间有多于两个的空格
        };


        private static string normalizeSpace(string inStr, int normalizeSpaceStatus)
        {
            string pattern = spacePattern[normalizeSpaceStatus] ?? null;
            if (pattern == null)
            {
                return inStr;
            }
            string outStr = Regex.Replace(inStr, pattern, " ");

            return outStr;
        }


        public static string normalizeString(string inStr)
        {
            return normalizeString(inStr, 0);
        }

        public static string normalizeString(string inStr, int normalizeSpaceStatus)
        {
            if (inStr == null || inStr.Trim().Length == 0)
                return inStr;

            inStr = normalizeSpace(inStr, normalizeSpaceStatus);

            string new_str = "";
            for (int i = 0; i < inStr.Length; i++)
            {
                if (inStr[i] == '\'')
                    new_str += '_';
                else
                    new_str += inStr[i];
            }

            return new_str;
        }

        /// <summary>
        /// 判断一个输入字符串是否完全是数字，null和空字符串会返回false
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>true表示合法</returns>
        public static bool isAllNumber(string inStr)
        {
            if (inStr == null || inStr.Length == 0)
                return false;

            for (int i = 0; i < inStr.Length; i++)
            {
                if (!IsNumber(inStr, i))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 判断一个输入字符串是否完全是数字或者字母，null和空字符串会返回false
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>true表示合法</returns>
        public static bool IsAllNumberOrLetter(string inStr)
        {
            if (inStr == null || inStr.Length == 0)
                return false;

            for (int i = 0; i < inStr.Length; i++)
            {
                if (!IsLetterOrNumber(inStr[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 判断一个字符串中的某个字符是不是数字
        /// </summary>
        /// <param name="s"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool IsNumber(string s, int i)
        {
            char c = s.ToCharArray()[i];
            return IsNumber(c);
        }

        /// <summary>
        /// 判断一个字符是否数字或者字符，字符只允许a-z和A-Z
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsLetterOrNumber(char c)
        {
            if (IsNumber(c) || IsLetter(c))
                return true;

            return false;
        }

        /// <summary>
        /// 判断是否是数字，跟char.isnumber不同，它不认为中文数字是合法数字
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsNumber(char c)
        {
            if (c < '0' || c > '9')
                return false;

            return true;
        }

        /// <summary>
        /// 判断是否a-Z的letter
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsLetter(char c)
        {
            if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                return true;

            return false;
        }

        /// <summary>
        /// 判断一个输入字符串是否是合法的Double类型
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>true表示合法</returns>
        public static bool isDouble(string inStr)
        {
            try
            {
                Double.Parse(inStr);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断一个输入字符串是否是合法的Int32类型
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>true表示合法</returns>
        public static bool isInt32(string inStr)
        {
            try
            {
                Int32.Parse(inStr);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断一个输入字符串是否是一个合法的邮编
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isValidPostalCode(string inStr)
        {
            if (inStr == null || inStr.Length == 0)
                return false;

            return ((inStr.Trim().Length == 6) && isAllNumber(inStr));
        }

        /// <summary>
        /// 判断一个输入字符串是否是合法的Int64类型
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>true表示合法</returns>
        public static bool isInt64(string inStr)
        {
            try
            {
                Int64.Parse(inStr);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断一个字符串是否是合法的用户账号
        /// </summary>
        /// <param name="inStr">输入字符串，用户账号</param>
        /// <returns>返回true表示合法</returns>
        public static bool isValidUserLogin(string inStr)
        {
            if (!isValidChar(inStr))
                return false;

            return (isValidEmail(inStr) || isValidMobile(inStr) || isAllNumber(inStr));
        }

        /// <summary>
        /// 判断一个字符串是否是合法的用户账号，上面的isValidUserLogin里加个isAllNumber是什么意思？
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isValidUserLogin_New(string inStr)
        {
            if (!isValidChar(inStr))
                return false;

            return (isValidEmail(inStr) || isValidMobile(inStr));
        }

        /// <summary>
        /// 判断一个输入是否合法的用户密码
        /// </summary>
        /// <param name="inStr">输入密码</param>
        /// <returns>true表示合法，false不合法</returns>
        public static bool isValidUserPassword(string inStr)
        {
            if (inStr == null)
                return false;

            if (inStr.Length > 32 || inStr.Length < 3)
                return false;

            return isValidChar(inStr);
        }

        /// <summary>
        /// 判断一个字符串是否合法的UnionID
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isValidUnionName(string inStr)
        {
            if (inStr == null || inStr.Trim() == "")
                return false;

            if (inStr.IndexOf("%") != -1 || inStr.IndexOf(" ") != -1)
                return false;

            return isValidInputString(inStr);
        }

        /// <summary>
        /// 一个更为安全的判断字符串是否合法的方法
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isSafeString(string inStr)
        {
            if (inStr == null || inStr.Trim() == "")
                return true;

            if (inStr.IndexOf("'") != -1)
                return false;

            return isValidInputString(inStr);
        }

        /// <summary>
        /// 判断一个输入串含有可疑的注入词，公有方法
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>true表示都合法，false表示有，不合法</returns>
        public static bool isValidInputString(string inStr)
        {
            if (inStr == null || inStr.Length == 0 || inStr.Trim() == "")
                return true;

            string Sql_1 = "exec|insert+|insert|select+|delete+|delete|update+|master+|master|truncate|declare|drop+|drop|drop+table|table";
            string[] sql_c = Sql_1.Split('|');

            foreach (string sl in sql_c)
            {
                if (inStr.ToLower().IndexOf(sl.Trim()) >= 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 判断一个输入串是否都是合法字符，私有方法
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>true表示都合法，false表示有不合法</returns>
        private static bool isValidChar(string inStr)
        {
            const string invalid_char = " |,|;|\"|'|[|]|{|}|<|>|(|)";

            if (inStr == null)
                return false;

            foreach (string str_t in invalid_char.Split('|'))
            {
                if (inStr.IndexOf(str_t) > -1)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 判断一个输入字符串是不是一个手机号
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>返回true表示合法</returns>
        public static bool isValidMobile(string inStr)
        {
            Regex rTel = new Regex(strMobile);
            Match mTel = rTel.Match(inStr);

            if (mTel.Success)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 判断一个输入字符串是不是一个座机号
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>返回true表示合法</returns>
        public static bool isValidTelephone(string inStr)
        {
            Regex rTel = new Regex(strTelephone);
            Match mTel = rTel.Match(inStr);

            if (mTel.Success)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 判断一个输入字符串是不是一个合法的邮件地址
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>返回true表示合法</returns>
        public static bool isValidEmail(string inStr)
        {
            Regex r = new Regex(strEmail);
            Match mEmail = r.Match(inStr);

            if (mEmail.Success)
                return true;
            else
                return false;
        }

        public static bool IsValidIpAddress(string ipString)
        {
            bool returnValue = true;
            if (string.IsNullOrWhiteSpace(ipString))
                return false;
            if (!isSafeString(ipString))
                return false;
            string[] ipList = ipString.Split(new char[] { '.' });
            if (ipList.Length == 4 || ipList.Length == 6)
            {
                foreach (string item in ipList)
                {
                    if (isAllNumber(item))
                    {
                        int i = Convert.ToInt32(item);
                        if (i >= 0 || i <= 255)
                            continue;
                        else
                        {
                            returnValue = false;
                            break;
                        }
                    }
                    else
                    {
                        returnValue = false;
                        break;
                    }
                }
            }
            else
            {
                return false;
            }
            return returnValue;
        }

        /// <summary>
        /// 验证网址
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsUrl(string source)
        {
            return Regex.IsMatch(source, @"^(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&amp;%_\./-~-]*)?$", RegexOptions.IgnoreCase);
        }


        /// <summary>
        /// 字符串中是否含有中文
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public static bool isLiveChinese(string words)
        {
            string TmmP;
            for (int i = 0; i < words.Length; i++)
            {
                TmmP = words.Substring(i, 1);
                byte[] sarr = System.Text.Encoding.GetEncoding("gb2312").GetBytes(TmmP);
                if (sarr.Length == 2)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 验证是否是合法的COID
        /// </summary>
        /// <param name="co_id"></param>
        /// <returns></returns>
        public static bool IsValidCOID(string co_id)
        {
            if (string.IsNullOrEmpty(co_id))
                return false;

            Regex reg = new Regex(@"^[1,2]\d{11}$");
            return reg.IsMatch(co_id);
        }

        /// <summary>
        /// 验证是否是合法的SOID
        /// </summary>
        /// <param name="so_id"></param>
        /// <returns></returns>
        public static bool IsValidSOID(string so_id)
        {
            if (string.IsNullOrEmpty(so_id))
                return false;

            Regex reg = new Regex(@"^3\d{11}$");
            return reg.IsMatch(so_id);
        }

        /// <summary>
        /// 验证是否是合法的鞋码，如41.5
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool IsShoeSize(string inStr)
        {
            if (string.IsNullOrEmpty(inStr))
                return false;

            Regex reg = new Regex(@"^[1-4]\d(\.5)?$");
            return reg.IsMatch(inStr);
        }

        /// <summary>
        /// 验证是否是合法的鞋码，多个鞋码用'/'分割
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool IsMoreShoeSize(string inStr)
        {
            if (string.IsNullOrEmpty(inStr))
                return false;

            Regex reg = new Regex(@"^([1-4]\d(\.5)?\/)*[1-4]\d(\.5)?$");
            return reg.IsMatch(inStr);
        }



        
    }

}
