using Ets.Service.IProvider.OpenApi;
using ETS.Const;
using ETS.Enums;
using ETS.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.OpenApi
{
    public class JuWangKeGroup : IGroupProviderOpenApi
    {
        public string app_key = ConfigSettings.Instance.JuWangKeAppkey;
        public const string v = "1.0";
        public string app_secret = ConfigSettings.Instance.JuWangKeAppsecret;


        public ETS.Enums.OrderApiStatusType AsyncStatus(Model.Common.ParaModel<Model.ParameterModel.Order.AsyncStatusPM_OpenApi> paramodel)
        {
            int status = 0; //第三方订单状态物流状态，1代表已发货，2代表已签收
            switch (paramodel.fields.status)
            {
                case OrderConst.OrderStatus1:
                    status = 1; //已完成
                    break;
                case OrderConst.OrderStatus2:
                    status = 2; //已接单
                    break;
                case OrderConst.OrderStatus3:
                     status = 3; //已取消
                     break;
                default:
                    return OrderApiStatusType.Success;
            }
            string url = ConfigurationManager.AppSettings["JuWangKeOrderAsyncStatus"];
            if (url == null)
                return OrderApiStatusType.SystemError;
            ///order_id	int	Y	订单ID ，根据订单ID改变对应的订单物流状态，一个订单只能修改一次，修改过再修改会报错。
            /// status	int	Y	物流状态，1代表已发货，2代表已签收
            ///send_phone	string	Y/N	配送员电话，物流状态传参是（ststus=1）的时候，配送员电话必须写，如果为（ststus=2）的时候可以不写。
            ///send_name	string	Y/N	配送员姓名，物流状态传参是（ststus=1）的时候，配送员姓名必须写，如果为（ststus=2）的时候可以不写。
             
            //{"OrderNumber":1040649505,"OrderStatus":2,"Phone":"13712345678","KnightName":"骑士"}

            JuWangKeOrderModel jkom = new JuWangKeOrderModel(); 
            jkom.Phone = paramodel.fields.ClienterPhoneNo;
            jkom.OrderStatus = status;
            jkom.OrderNumber = paramodel.fields.OriginalOrderNo;
            if (!string.IsNullOrWhiteSpace(paramodel.fields.ClienterTrueName))
            {
                jkom.KnightName = paramodel.fields.ClienterTrueName;
            }else if (!string.IsNullOrWhiteSpace(paramodel.fields.BusinessName)){
                jkom.KnightName = paramodel.fields.BusinessName;
            }else{
                jkom.KnightName = "";
            }

            string json = HTTPHelper.HttpPost(url, "OrderNumber=" + jkom.OrderNumber + "&OrderStatus=" + jkom.OrderStatus + "&Phone=" + jkom.Phone + "&KnightName=" + jkom.KnightName);

            LogHelper.LogWriter("调用聚网客接口：", new { model = json });
            if (string.IsNullOrWhiteSpace(json))
                return OrderApiStatusType.ParaError;
            else if (json == "null")
                return OrderApiStatusType.SystemError;
            JObject jobject = JObject.Parse(json);
            int x = jobject.Value<int>("code"); //接口调用状态 区分大小写
            return x == 1 ? OrderApiStatusType.Success : OrderApiStatusType.SystemError;
        }
         
        public Model.ParameterModel.Order.CreatePM_OpenApi SetCcmmissonInfo(Model.ParameterModel.Order.CreatePM_OpenApi paramodel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 回调聚网客接口Model
        /// </summary>
        public class JuWangKeOrderModel
        {
            public string OrderNumber { get; set; }
            public int OrderStatus { get; set; }
            public string Phone { get; set; }
            public string KnightName { get; set; }
        }
    }
}
