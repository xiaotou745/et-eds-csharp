using Ets.Model.Common;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.OpenApi;
using Ets.Service.Provider.OpenApi;
using ETS.Const;
using ETS.Enums;
using ETS.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.OpenApi
{
    public class MeiTuanGroup : IGroupProviderOpenApi, IPullOrderInfoOpenApi
    {
        public string app_key = ConfigSettings.Instance.FulltimeAppkey;
        public const string v = "1.0";
        public string app_secret = ConfigSettings.Instance.FulltimeAppsecret;
        /// <summary>
        /// 回调万达接口同步订单状态  add by caoheyang 20150326
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        public OrderApiStatusType AsyncStatus(ParaModel<AsyncStatusPM_OpenApi> paramodel)
        {
            int status = 0; //第三方订单状态物流状态，1代表已发货，2代表已签收
            switch (paramodel.fields.status)
            {
                case OrderConst.OrderStatus1:
                    status = 2; //已签收
                    break;
                case OrderConst.OrderStatus2:
                    status = 1; //已发货
                    break;
                default:
                    return OrderApiStatusType.Success;
            }
            string ts = DateTime.Now.ToString();
            string url = ConfigurationManager.AppSettings["FulltimeAsyncStatus"];
            if (url == null)
                return OrderApiStatusType.SystemError;
            ///order_id	int	Y	订单ID ，根据订单ID改变对应的订单物流状态，一个订单只能修改一次，修改过再修改会报错。
            /// status	int	Y	物流状态，1代表已发货，2代表已签收
            ///send_phone	string	Y/N	配送员电话，物流状态传参是（ststus=1）的时候，配送员电话必须写，如果为（ststus=2）的时候可以不写。
            ///send_name	string	Y/N	配送员姓名，物流状态传参是（ststus=1）的时候，配送员姓名必须写，如果为（ststus=2）的时候可以不写。
            string json = HTTPHelper.HttpPost(url, "app_key=" + app_key + "&sign=" + GetSign(ts) + "&timestamp=" + ts + "&order_id=" + paramodel.fields.OriginalOrderNo + "&status=" + status + "&v=" + v + "&send_phone=" + paramodel.fields.ClienterPhoneNo
                + "&send_name=" + paramodel.fields.ClienterTrueName);
            if (string.IsNullOrWhiteSpace(json))
                return OrderApiStatusType.ParaError;
            else if (json == "null")
                return OrderApiStatusType.SystemError;
            JObject jobject = JObject.Parse(json);
            int x = jobject.Value<int>("status"); //接口调用状态 区分大小写
            return x == 1 ? OrderApiStatusType.Success : OrderApiStatusType.SystemError;
        }

        /// <summary>
        /// 获取当前集团请求时的sign信息  add by caoheyang 20150407
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        private string GetSign(string time)
        {
            string signStr = app_secret + "app_key" + app_key + "timestamp" + time + "v" + v + app_secret;
            return ETS.Security.MD5.Encrypt(signStr);
        }

        /// <summary>
        /// 新增商铺时根据集团id为店铺设置外送费，结算比例等财务相关信息 add by caoheyang 20150417
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        public CreatePM_OpenApi SetCcmmissonInfo(CreatePM_OpenApi paramodel)
        {
            paramodel.store_info.delivery_fee = 5;//全时目前外送费统一5
            paramodel.store_info.businesscommission = 0;//万达目前结算比例统一0
            return paramodel;
        }


        public void PullOrderInfo(string info)
        {

        }
    }



    #region 美团订单数据实体 add by caoheyang 20150420
    /// <summary>
    /// 美团抓取过来的订单明细对应的model实体
    /// </summary>
    public class MeiTuanOrdeModel
    {
        /// <summary>
        /// 美团订单ID
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// 收件人地址
        /// </summary>
        public string recipient_address { get; set; }

        /// <summary>
        /// 收件人电话
        /// </summary>
        public string recipient_phone { get; set; }
        /// <summary>
        /// 收件人姓名
        /// </summary>
        public string recipient_name { get; set; }
        /// <summary>
        /// 门店配送费
        /// </summary>
        public decimal shipping_fee { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal total { get; set; }
        /// <summary>
        /// 忌口或备注
        /// </summary>
        public string caution { get; set; }
        /// <summary>
        /// 送餐员电话
        /// </summary>
        public string shipper_phone { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 订单是否要发票
        /// </summary>
        public int has_invoiced { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>
        public string invoice_title { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime ctime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime utime { get; set; }
        /// <summary>
        /// 订单详细类目列表
        /// </summary>
        public MeiTuanOrdeDetailModel[] detail { get; set; }
        /// <summary>
        /// 订单活动类目列表
        /// </summary>
        public MeiTuanOrderExtrasModel[] extras { get; set; }
        /// <summary>
        /// 美团商家ID
        /// </summary>
        public int wm_poi_id { get; set; }
        /// <summary>
        /// 美团商家名称
        /// </summary>
        public string wm_poi_name { get; set; }
        /// <summary>
        /// 美团商家地址
        /// </summary>
        public string wm_poi_address { get; set; }
        /// <summary>
        /// 美团商家电话
        /// </summary>
        public string wm_poi_phone { get; set; }
        /// <summary>
        /// 是否为美团商家APP方配送
        /// </summary>
        public int is_third_shipping { get; set; }
        /// <summary>
        /// 支付类型。1：货到付款，2：在线支付
        /// </summary>
        public int pay_type { get; set; }
    }
    /// <summary>
    /// 美团订单详细类目列表 add by caoheyang 20150420
    /// </summary>
    public class MeiTuanOrdeDetailModel
    {
        /// <summary>
        /// APP方菜品id
        /// </summary>
        public int app_food_code { get; set; }
        /// <summary>
        /// APP方菜品名称
        /// </summary>
        public string food_name { get; set; }
        /// <summary>
        /// 菜品数量
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 菜品单价
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 打包餐盒数量
        /// </summary>
        public int box_num { get; set; }
        /// <summary>
        /// 打包餐盒单价
        /// </summary>

        public decimal box_price { get; set; }
        /// <summary>
        /// 菜品折扣，只是美团商家，APP方配送的门店才会设置，默认为1。折扣值不参与总价计算。
        /// </summary>
        public decimal food_discount { get; set; }

    }


    /// <summary>
    /// 美团订单详细类目列表 add by caoheyang 20150420
    /// </summary>
    public class MeiTuanOrderExtrasModel
    {
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal reduce_fee { get; set; }
        /// <summary>
        /// 优惠活动说明
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 优惠活动类型（1-免配送费；2-满赠；3-满减）
        /// </summary>
        public int type { get; set; }
    }
    #endregion

}
