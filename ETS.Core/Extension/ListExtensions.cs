using System;
using System.Collections.Generic;
using System.Text;

namespace ETS.Extension
{
    public static class ListExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string SplitString<T>(this List<T> list, char c)
        {
            var result = new StringBuilder();

            foreach (var str in list)
            {
                result.Append(str.ToString() + c);
            }

            if (list.Count > 0)
                return result.ToString().TrimEnd(c);

            return result.ToString();
        }

        public static List<int> ToNumberList(this List<string> list)
        {
            var result = new List<int>();
            foreach (var item in list)
            {
                result.Add(Convert.ToInt32(item));
            }
            return result;
        }
    }
}