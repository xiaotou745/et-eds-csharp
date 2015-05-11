using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ETS.Util
{
    public class XmlParser
    {
        private static Regex regex = new Regex("<(\\w+?)[ >]", RegexOptions.Compiled);
        /// <summary>
        /// 编码
        /// </summary>
        private static Regex encodingRegex = new Regex(@"(?:encoding=""(?<coding>[^""]*)"")", RegexOptions.IgnoreCase);
        private static Dictionary<string, XmlSerializer> parsers = new Dictionary<string, XmlSerializer>();
        public T Parse<T>(string body) where T : class
        {
            Type typeFromHandle = typeof(T);
            string rootElement = this.GetRootElement(body);
            string text = typeFromHandle.FullName;
            //if ("error_response".Equals(rootElement))
            //{
            //    text += "_error_response";
            //}
            XmlSerializer xmlSerializer = null;
            bool flag = XmlParser.parsers.TryGetValue(text, out xmlSerializer);
            if (!flag || xmlSerializer == null)
            {
                XmlAttributes xmlAttributes = new XmlAttributes();
                xmlAttributes.XmlRoot = new XmlRootAttribute(rootElement);
                XmlAttributeOverrides xmlAttributeOverrides = new XmlAttributeOverrides();
                xmlAttributeOverrides.Add(typeFromHandle, xmlAttributes);
                xmlSerializer = new XmlSerializer(typeFromHandle, xmlAttributeOverrides);
                XmlParser.parsers[text] = xmlSerializer;
            }
            object obj = null;
            var encoding = GetEncoding(body);
            using (Stream stream = new MemoryStream(encoding.GetBytes(body)))
            {
                obj = xmlSerializer.Deserialize(stream);
            }
            T t = (T)((object)obj);
            //if (t != null)
            //{
            //    t.Body = body;
            //}
            return t;
        }
        private string GetRootElement(string body)
        {
            Match match = XmlParser.regex.Match(body);
            if (match.Success)
            {
                return match.Groups[1].ToString();
            }
            throw new XmlParserException("Invalid XML response format!");
        }

        private Encoding GetEncoding(string body)
        {
            Match m = encodingRegex.Match(body);
            if (m.Success)
            {
                return Encoding.GetEncoding(m.Groups["coding"].Value);
            }
            else
            {
                return Encoding.Default;
            }
        }
    }

    public class XmlParserException : Exception
    {
        public XmlParserException(string message)
            : base(message)
        { }
    }
}
