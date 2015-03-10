using System;
using System.Collections.Generic;
using System.Linq;

namespace ETS.Extension
{
    public static class
        StringExtension
    {
        public static string DeleteLastChar(this string source)
        {
            return source.Substring(0, source.Length - 1);
        }

        #region Format

        public static string FormatArgs(this string source, object arg0)
        {
            return FormatArgs(source, new[] {arg0});
        }

        public static string FormatArgs(this string source, object arg0, object arg1)
        {
            return FormatArgs(source, new[] {arg0, arg1});
        }

        public static string FormatArgs(this string source, object arg0, object arg1, object arg2)
        {
            return FormatArgs(source, new[] {arg0, arg1, arg2});
        }

        public static string FormatArgs(this string source, params object[] args)
        {
            return string.Format(source, args);
        }

        #endregion

        #region ToList

        public static List<string> ToList(this string text)
        {
            return ToList(text, ',');
        }

        public static List<int> ToNumberList(this string text)
        {
            return ToList(text, ',').ToNumberList();
        }

        public static List<string> ToList(this string text, char c)
        {
            if (!string.IsNullOrEmpty(text))
                return text.Split(new[] {c},StringSplitOptions.RemoveEmptyEntries).ToList();
            else
                return new List<string>();
        }

        #endregion
    }
}