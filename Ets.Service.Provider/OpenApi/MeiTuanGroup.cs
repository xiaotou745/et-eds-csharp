using Ets.Model.Common;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.OpenApi;
using Ets.Service.Provider.OpenApi;
using ETS.Const;
using ETS.Enums;
using ETS.Util;
using Letao.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.OpenApi
{
    /// <summary>
    /// 美团相关业务类 add by caoheyang 20150420
    /// </summary>
    public class MeiTuanGroup : IGroupProviderOpenApi
    {
        private string app_id = ConfigSettings.Instance.MeiTuanAppkey;
        private string consumer_secret = ConfigSettings.Instance.MeiTuanAppsecret;

        /// <summary>
        /// Accept HTTP 标头的值
        /// </summary>
        private string accept = "application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

        #region 回调美团接口同步订单状态
        /// <summary>
        /// 回调美团接口同步订单状态  add by caoheyang 20150420
        /// </summary>
        /// <param name="paramodel">参数实体</param>
        /// <returns></returns>
        public OrderApiStatusType AsyncStatus(ParaModel<AsyncStatusPM_OpenApi> paramodel)
        {
            switch (paramodel.fields.status)
            {
                case OrderConst.OrderStatus1: //已完成
                    return ArrivedAsyncStatus(paramodel.fields);
                case OrderConst.OrderStatus2: //已接单
                    return DeliveringAsyncStatus(paramodel.fields);
                case OrderConst.OrderStatus3: //已取消
                    return CancelAsyncStatus(paramodel.fields);
                case OrderConst.OrderStatus0: //待确认
                    return ConfirmAsyncStatus(paramodel.fields);
                default:
                    return OrderApiStatusType.ParaError;
            }
        }

        /// <summary>
        ///  确认订单美团回调接口地址  add by caoheyang 20150420
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected OrderApiStatusType ConfirmAsyncStatus(AsyncStatusPM_OpenApi model)
        {
            //参数信息
            List<string> @params = new List<string>() { 
            "timestamp="+TimeHelper.GetTimeStamp(false) ,
            "order_id="+model.order_no, //订单号
            "app_id="+app_id
            };
            @params.Sort();
            string url = ConfigSettings.Instance.MeiTuanConfirmAsyncStatus;
            string sig = ETS.Security.MD5.Encrypt(url + string.Join("&", @params) + consumer_secret).ToLower();
            string paras = string.Join("&", @params) + "&sig=" + sig;
            return GetDoAsyncStatus(url, paras);
        }

        /// <summary>
        /// 取消订单美团回调接口地址  add by caoheyang 20150420
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected OrderApiStatusType CancelAsyncStatus(AsyncStatusPM_OpenApi model)
        {
            //参数信息
            List<string> @params = new List<string>() { 
            "timestamp="+TimeHelper.GetTimeStamp(false) ,
            "order_id="+model.order_no, //订单号
            "reason=12", //取消原因
            "reason_code=12", //规范化取消原因code
            "app_id="+app_id
            };
            @params.Sort();
            string url = ConfigSettings.Instance.MeiTuanCancelAsyncStatus;
            string sig = ETS.Security.MD5.Encrypt(url + string.Join("&", @params) + consumer_secret).ToLower();
            string paras = string.Join("&", @params) + "&sig=" + sig;
            string postdata = "";
            return PostDoAsyncStatus(url, postdata);
        }
        /// <summary>
        ///  订单配送中美团回调接口地址  add by caoheyang 20150420
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected OrderApiStatusType DeliveringAsyncStatus(AsyncStatusPM_OpenApi model)
        {
            //参数信息
            List<string> @params = new List<string>() { 
            "timestamp="+TimeHelper.GetTimeStamp(false) ,
            "order_id="+model.order_no, //订单号
            "courier_name="+model.ClienterTrueName, //配送员姓名
            "courier_phone="+model.ClienterPhoneNo, //配送电话
            "app_id="+app_id
            };
            @params.Sort();
            string url = ConfigSettings.Instance.MeiTuanDeliveringAsyncStatus;
            string sig = ETS.Security.MD5.Encrypt(url + string.Join("&", @params) + consumer_secret).ToLower();
            string paras = string.Join("&", @params) + "&sig=" + sig;
            return PostDoAsyncStatus(url, paras);
        }
        /// <summary>
        /// 订单已送达美团（订单完成）回调接口地址  add by caoheyang 20150420
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected OrderApiStatusType ArrivedAsyncStatus(AsyncStatusPM_OpenApi model)
        {
            //参数信息
            List<string> @params = new List<string>() { 
            "timestamp="+TimeHelper.GetTimeStamp(false) ,
            "order_id="+model.order_no, //订单号
            "app_id="+app_id
            };
            @params.Sort();
            string url = ConfigSettings.Instance.MeiTuanArrivedAsyncStatus;
            string sig = ETS.Security.MD5.Encrypt(url + string.Join("&", @params) + consumer_secret).ToLower();
            string paras = string.Join("&", @params) + "&sig=" + sig;
            return PostDoAsyncStatus(url, paras);
        }

        /// <summary>
        /// GET调用美团第三方接口 add by caoheyang 20150420
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postdata">参数数据</param>
        /// <returns></returns>
        protected OrderApiStatusType GetDoAsyncStatus(string url, string postdata)
        {
            string json = new HttpClient().GetStringAsync(url + postdata).Result;
            return AsyncStatusInfo(json);
        }

        /// <summary>
        /// 对同步美团订单状态的第三方接口返回值做处理 add by caoheyang 20150420
        /// </summary>
        /// <param name="json">请求得到的数据</param>
        /// <returns></returns>
        protected OrderApiStatusType AsyncStatusInfo(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return OrderApiStatusType.ParaError;
            JObject jobject = JObject.Parse(json);
            string status = jobject.Value<string>("data"); //接口调用状态 区分大小写
            return status == "ok" ? OrderApiStatusType.Success : OrderApiStatusType.OtherError;
        }

        /// <summary>
        /// POST调用美团第三方接口 add by caoheyang 20150420
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postdata">参数数据</param>
        /// <returns></returns>
        protected OrderApiStatusType PostDoAsyncStatus(string url, string postdata)
        {
            string json = HTTPHelper.HttpPost(url, postdata, accept: accept);
            return AsyncStatusInfo(json);
        }
        #endregion

        /// <summary>
        /// 新增商铺时根据集团id为店铺设置外送费，结算比例等财务相关信息 add by caoheyang 20150420
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        public CreatePM_OpenApi SetCcmmissonInfo(CreatePM_OpenApi paramodel)
        {
            paramodel.store_info.delivery_fee = 5;//全时目前外送费统一5
            paramodel.store_info.businesscommission = 0;//万达目前结算比例统一0
            return paramodel;
        }


        /// <summary>
        /// 验证美团推送订单的签名是否正确  add by caoheyang 20150421
        /// </summary>
        /// <param nCame="fromModel">美团数据实体</param>
        public bool ValiditeSig(MeiTuanOrdeModel fromModel)
        {
            IList<string> infos = new List<string>();
            PropertyInfo[] props = fromModel.GetType().GetProperties();
            props = props.Where(item => item.Name != "sig").OrderBy(item => item.Name).ToArray();
            string paras = "";
            for (int i = 0; i < props.Length; i++)
            {
                if (i != props.Length - 1)
                {
                    if ((props[i].Name == "detail" || props[i].Name == "extras") && props[i].GetValue(fromModel) != null)
                        paras = paras + props[i].Name + "=" + JsonHelper.JsonConvertToString(props[i].GetValue(fromModel));
                    else
                        paras = paras + props[i].Name + "=" + props[i].GetValue(fromModel) + "&";
                }
                else
                    paras = paras + props[i].Name + "=" + props[i].GetValue(fromModel) + "&";
            }
            string url = "http://test.waimaiopen.meituan.com/api/v1/third_shipping/save?";  //TODO
            string waimd5 = url + paras + consumer_secret; //consumer_secret
            string sig = ETS.Security.MD5.Encrypt(waimd5).ToLower();
            return sig == fromModel.sig;
        }

        /// <summary>
        /// 美团的订单数据转成通用的openapi接入订单数据实体类型 20150421
        /// </summary>
        /// <param name="fromModel">美团数据实体</param>
        public CreatePM_OpenApi TranslateModel(MeiTuanOrdeModel fromModel)
        {
            CreatePM_OpenApi model = new CreatePM_OpenApi();
            model.order_id = fromModel.order_id; //订单ID
            ///店铺信息
            Store store = new Store();
            store.store_id = fromModel.app_poi_code;//对接方店铺id
            store.store_name = fromModel.wm_poi_name;//店铺名称
            store.address = fromModel.wm_poi_address;//店铺地址
            store.phone = fromModel.wm_poi_address;//店铺电话

            ///订单地址信息
            Address address = new Address();
            address.address = fromModel.recipient_address;//用户收货地址
            address.user_phone = fromModel.recipient_phone;//用户联系电话
            address.user_name = fromModel.recipient_phone;//用户姓名
            //TODO 佣金待确认
            model.remark = fromModel.caution;//备注
            model.status = OrderConst.OrderStatus30;//初始化订单状态 第三方代接入
            model.create_time = fromModel.ctime;//订单发单时间 创建时间
            model.payment = fromModel.pay_type;//支付类型
            model.is_pay = fromModel.pay_type == 1 ? false : true;//目前货到付款时取未支付，在线支付取已支付

            address.longitude = fromModel.longitude; //经度
            address.latitude = fromModel.latitude; //纬度
            model.store_info = store; //店铺 
            model.address = address; //订单ID

            //订单明细不为空时做处理 
            if (fromModel.detail != null && fromModel.detail.Length > 0)
            {
                OrderDetail[] details = new OrderDetail[fromModel.detail.Length];
                for (int i = 0; i < fromModel.detail.Length; i++)
                {
                    OrderDetail tempdetail = new OrderDetail();
                    tempdetail.product_name = fromModel.detail[i].food_name;//菜品名称
                    tempdetail.quantity = fromModel.detail[i].quantity;//菜品数量
                    tempdetail.unit_price = fromModel.detail[i].price;//菜品单价
                    tempdetail.detail_id = 0;//美团不传递明细id，明细id为0
                    details[i] = tempdetail;
                }
                model.order_details = details; //订单ID
            }

            model.receive_time = fromModel.ctime;//美团不传递，E代送必填 要求送餐时间
            //fromModel.extras 说明，暂时不用 
            return model;
        }

        /// <summary>
        /// 新增美团订单 add by caoheyang 20150421 
        /// </summary>
        /// <param name="fromModel">paraModel</param>
        public int AddOrder(CreatePM_OpenApi paramodel) {
           var redis = new ETS.NoSql.RedisCache.RedisCache();
          int businessId= ParseHelper.ToInt(redis.Get<string>(string.Format(ETS.Const.RedissCacheKey.OtherBusinessIdInfo, paramodel.store_info.group.ToString(),
              paramodel.store_info.store_id.ToString()))); //缓存中取E代送商户id
          if (businessId == 0) {
              return 0;   //商户不存在发布订单失败
          }
            return string.IsNullOrWhiteSpace(new Ets.Dao.Order.OrderDao().CreateToSql(paramodel)) ? 0 : 1;
        }
    }



    #region 美团订单数据实体 add by caoheyang 20150420
    /// <summary>
    /// 美团抓取过来的订单明细对应的model实体
    /// </summary>
    public class MeiTuanOrdeModel
    {
        /// <summary>
        /// app_id
        /// </summary>
        public string app_id { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sig { get; set; }

        /// <summary>
        /// 美团订单ID
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// APP方商家ID
        /// </summary>
        public int app_poi_code { get; set; }

        /// <summary>
        /// 美团商家名称
        /// </summary>
        public string wm_poi_name { get; set; }

        /// <summary>
        ///美团商家地址
        /// </summary>
        public string wm_poi_address { get; set; }


        /// <summary>
        /// 美团商家电话
        /// </summary>
        public string wm_poi_phone { get; set; }


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
        /// 是否为美团商家APP方配送
        /// </summary>
        public int is_third_shipping { get; set; }
        /// <summary>
        /// 支付类型。1：货到付款，2：在线支付
        /// </summary>
        public int pay_type { get; set; }

        /// <summary>
        /// 实际送餐地址纬度
        /// </summary>
        public decimal latitude { get; set; }
        /// <summary>
        /// 实际送餐地址经度
        /// </summary>
        public decimal longitude { get; set; }
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
