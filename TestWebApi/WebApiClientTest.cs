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
        public void Test11()
        {
            TestApiHelper test = new TestApiHelper();
            test.TestApi<NewRegisterInfoModel>("http://localhost:9263/BusinessAPI/NewPostPublishOrder_B", new NewRegisterInfoModel()
            {           
                 CommissionTypeId =1
            });
             
        }
        
       
    }
}
