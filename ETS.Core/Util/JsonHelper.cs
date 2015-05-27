using System;
using System.Web.Script.Serialization;

namespace ETS.Util
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

        public static T JsonConvertToObject<T>(string json) where T : class
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static string JsonConvertToString(object o)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(o);
        }
    }
}
