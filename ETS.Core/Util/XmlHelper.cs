using System;
using System.IO;
using System.Xml.Serialization;

namespace ETS.Util
{
    public class XmlHelper
    {
        public static void ToXml(string fileName, object obj)
        {

            using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                //得到被序列化的类型
                Type type = obj.GetType();
                XmlSerializer sz = new XmlSerializer(type);
                //开始序列化
                sz.Serialize(stream, obj);
            }
        }

        public static T ToObject<T>(string fileName) where T : class
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                var sr = new System.IO.StreamReader(fileStream);
                string xmlString = sr.ReadToEnd();
                XmlSerializer sz = new XmlSerializer(typeof(T));
                sr.Close();
                return sz.Deserialize(new StringReader(xmlString)) as T;
            }
        }
    }
}
