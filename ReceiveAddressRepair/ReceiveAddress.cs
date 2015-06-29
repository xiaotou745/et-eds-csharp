using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;

namespace ReceiveAddressRepair
{
    public class ReceiveAddress : DaoBase
    {
        public DataTable GetNoReceiveAddress(DateTime startTime)
        {
            string querySql = @"select top 200 Id,ReceviceLongitude,ReceviceLatitude from dbo.[order] (nolock) where PubDate > @PubDate and ReceviceAddress is null or ReceviceAddress =''";

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@PubDate", SqlDbType.DateTime).Value = startTime;

            return DbHelper.ExecuteDataTable(SuperMan_Read, querySql, parm);
        }

        public string CreateAddressSql(int orderId,string address)
        { 
            StringBuilder updateSql = new StringBuilder();

            updateSql.AppendFormat(@"update dbo.[order] set ReceviceAddress = '{0}' where id ={1};", address, orderId);

            return updateSql.ToString();
        }

        public string GetAddress(string lng, string lat)
        {
            //lng = "116.322987";
            //lat = "39.983424";
            try
            {
                string url = string.Format(
                    "http://api.map.baidu.com/geocoder/v2/?ak=dAeaG6HwIFGlkbqtyKkyFGEC&callback=renderReverse&location={0},{1}&output=xml&pois=1",
                    lat, lng);

                WebRequest request = WebRequest.Create(url);
                request.Method = "POST";
                XmlDocument xmlDoc = new XmlDocument();
                string sendData = xmlDoc.InnerXml;
                byte[] byteArray = Encoding.Default.GetBytes(sendData);

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream, System.Text.Encoding.GetEncoding("utf-8"));
                string responseXml = reader.ReadToEnd();

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(responseXml);
                string status = xml.DocumentElement.SelectSingleNode("status").InnerText;
                if (status == "0")
                {

                    XmlNodeList nodes = xml.DocumentElement.GetElementsByTagName("formatted_address");
                    if (nodes.Count > 0)
                    {
                        return nodes[0].InnerText;
                    }
                    else
                        return "0未获取到位置信息,错误码3";
                }
                else
                {
                    return "0,错误码1";
                }
            }
            catch (System.Exception ex)
            {
                return "0未获取到位置信息,错误码2";
            }
        }
    }
}
