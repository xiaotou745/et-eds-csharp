using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Validator
{
    public class CommonValidator
    {
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }
            if (!phoneNumber.StartsWith("1") || phoneNumber.Length != 11)
            {
                return false;
            }
            return true;
            //return Regex.IsMatch(phoneNumber, @"^(13|14|15|16|18|19)\d{9}$");
        }

        public static bool IsValidEMail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
