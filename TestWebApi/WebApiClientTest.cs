using Newtonsoft.Json;
using SuperManCommonModel.Models;
using SuperManCore;
using SuperManCore.Common;
using SuperManWebApi.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestWebApi
{
    public class WebApiClientTest
    {
        //[Fact]
        //public void WebApi_SiteList_Test()
        //{
        //    var requestJson = JsonConvert.SerializeObject(new { startId = 1, itemcount = 3 });

        //    HttpContent httpContent = new StringContent(requestJson);
        //    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //    var httpClient = new HttpClient();

        //    var responseJson = httpClient.PostAsync("http://localhost:7178/BusinessAPI/OrderCount_B/1", httpContent)
        //        .Result.Content.ReadAsStringAsync().Result;



        //    var sites = JsonConvert.DeserializeObject<IList<ResultModel<BusiOrderCountResultModel>>>(responseJson);

        //    sites.ToList().ForEach(x => Console.WriteLine(x.Message + "：" + x.Result + "：" + x.Status));
        //} 

        public void Test11()
        {
            
             
        }
        
       
    }
}
