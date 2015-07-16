using ETS.Expand;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Extension
{
    public static class EnumExtenstion
    {
        public static IEnumItem[] GetEnumItems<T>()
        {
            return GetEnumItems(typeof(T));
        }
        public static string GetDisplayText(this Enum item)
        {
            if (item == null)
            {
                return string.Empty;
            }
            var enumItem = GetEnumItem(item.GetType(), item);
            return enumItem.Text;
        }

        public static IEnumItem[] GetEnumItems(Type enumType)
        {
            var items = Enum.GetValues(enumType);
            var result = new List<IEnumItem>();
            foreach (var item in items)
            {
                var enumItem = GetEnumItem(enumType, item as Enum);
                result.Add(enumItem);
            }
            return result.ToArray();
        }

        public static IEnumItem GetEnumItem(Type enumType, Enum item)
        {
            var name = item.ToString();
            var text = name;
            var mi = enumType.GetMember(name).FirstOrDefault();
            if (mi!=null)
            {
                var dtAttribute = mi.GetCustomAttributes(typeof(DisplayTextAttribute), false).FirstOrDefault();
                if (dtAttribute != null)
                {
                    text = (dtAttribute as DisplayTextAttribute).DisplayText;
                }
            }
            var enumItem = new EnumItem { Value = Convert.ToInt32(item), Name = item.ToString(), Text = text };
            return enumItem;
        }

        public static bool OwnElement<TEnum>(object element) where TEnum : struct
        {
            return Enum.IsDefined(typeof(TEnum), element);
        }
    }
}
