using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Order;
using Ets.Service.IProvider.Clienter;
using Ets.Service.Provider.Clienter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Ets.Model.Common;
using ETS.Enums;
using Ets.Model.ParameterModel.Order;
using Ets.Service.Provider.Common;
using Ets.Service.IProvider.Common;
using Ets.Model.DomainModel.Group;
using ETS.Security;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using ETS.Util;
using System.Configuration;
using ETS.Const;
using Ets.Service.Provider.OpenApi;
using Ets.Model.DataModel.Order;
using Ets.Service.IProvider.OpenApi;

namespace OpenApi.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public ResultModel<object> Index()
        {  //签名信息
            List<string> @params = new List<string>() { 
            "timestamp="+TimeHelper.GetTimeStamp(false) ,
            "app_area_code=99999",
            "min_price=9",
            "area=[{\"x\":39941199,\"y\":116385384}, {\"x\":39926983,\"y\":116361694},{\"x\":39921586,\"y\":116398430}]",
            "app_id=33"
            };
            @params.Sort();
            string url = "http://test.waimaiopen.meituan.com/api/v1/third_shipping/save?";
            string waimd5 = url + string.Join("&", @params) + "01c33711a7c2e6cf2cc27d838e83006e"; //consumer_secret
            string sig = ETS.Security.MD5.Encrypt(waimd5).ToLower();
            string paras = string.Join("&", @params) + "&sig=" + sig;
            string json = HTTPHelper.HttpPost(url, paras, accept: "application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            return null;
        }

        [HttpGet]
        public ResultModel<object> Index1()
        {  //签名信息
            List<string> @params = new List<string>() { 
            "timestamp="+TimeHelper.GetTimeStamp(false) ,
            "app_poi_code=24",
            "app_id=33"
            };
            @params.Sort();
            string url = "http://test.waimaiopen.meituan.com/api/v1/order/getneworders?";
            string waimd5 = url + string.Join("&", @params) + "01c33711a7c2e6cf2cc27d838e83006e"; //consumer_secret
            string sig = ETS.Security.MD5.Encrypt(waimd5).ToLower();
            string paras = string.Join("&", @params) + "&sig=" + sig;


            string jso1n = new HttpClient().GetStringAsync(url + paras).Result;

            string json = HTTPHelper.HttpPost(url + string.Join("&", @params) + "&sig=" + sig, "", accept: "application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            return null;
        }
    }
}