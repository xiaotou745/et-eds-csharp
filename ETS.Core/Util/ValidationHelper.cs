using System.Text.RegularExpressions;

namespace Letao.Util
{
    public static class ValidationHelper
    {
        public static bool IsValidMobile(this string mobile)
        {
            string pattern = "^1[358]\\d{9}$";
            var match = Regex.Match(mobile, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsValidEmail(this string email)
        {
            string pattern = "^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$";
            var match = Regex.Match(email, pattern, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
