using System;
using System.Web.Script.Serialization;

namespace Letao.Util
{
    public class JsonHelper
    {
        public static String ToJson(object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            return serializer.Serialize(obj);
        }

        public static T ToObject<T>(string json) where T : class
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            return serializer.Deserialize<T>(json);
        }
    }
}
