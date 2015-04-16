using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Common
{
    /// <summary>
    /// 百度帮助类 
    /// </summary>
    public static class BaiDuHelper
    {
       public const string baiduAddress = " http://api.map.baidu.com/geocoder?address={0}&&output=json";  //百度地址
        /// <summary>
        /// 调用百度api 获取经纬度信息
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static Tuple<decimal, decimal> GeoCoder(string address)
       {
           string json = new HttpClient().GetStringAsync(string.Format(baiduAddress,address)).Result;
           JObject jsonObject = JObject.Parse(json); //将百度接口返回的json转成 json对象
           if (jsonObject.Value<string>("status") == "OK")
           {
               JObject locationJObject = jsonObject.Value<JObject>("result").Value<JObject>("location");  //获取
               decimal longitude = locationJObject.Value<decimal>("lng");  //精度
               decimal latitude = locationJObject.Value<decimal>("lat"); //纬度
                return new Tuple<decimal,decimal>(longitude,latitude);
           }else
               return new Tuple<decimal, decimal>(0,0);
        }
    }
}
