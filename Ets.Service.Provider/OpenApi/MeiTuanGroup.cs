using Ets.Model.Common;
using Ets.Model.DataModel.Bussiness;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.OpenApi;
using Ets.Service.Provider.OpenApi;
using Ets.Service.Provider.Order;
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
            "order_id="+model.OriginalOrderNo, //订单号
            "app_id="+app_id
            };
            @params.Sort();
            string url = ConfigSettings.Instance.MeiTuanConfirmAsyncStatus + "?";
            string sig = ETS.Security.MD5.DefaultEncrypt(url + string.Join("&", @params) + consumer_secret).ToLower();
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
            //参数信息  已经排序好
            List<string> @params = new List<string>() { 
            "app_id="+app_id,
            "order_id="+model.OriginalOrderNo, //订单号
            "reason=APP方用户要求取消", //取消原因
            "reason_code=2006", //规范化取消原因code
            "timestamp="+TimeHelper.GetTimeStamp(false)    
            };
            string url = ConfigSettings.Instance.MeiTuanCancelAsyncStatus + "?";
            string sig = ETS.Security.MD5.DefaultEncrypt(url + string.Join("&", @params) + consumer_secret).ToLower();
            string paras = string.Join("&", @params) + "&sig=" + sig;
            return GetDoAsyncStatus(url, paras);
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
            "order_id="+model.OriginalOrderNo, //订单号
            "courier_name="+model.ClienterTrueName, //配送员姓名
            "courier_phone="+model.ClienterPhoneNo, //配送电话
            "app_id="+app_id
            };
            @params.Sort();
            string url = ConfigSettings.Instance.MeiTuanDeliveringAsyncStatus + "?";
            string sig = ETS.Security.MD5.DefaultEncrypt(url + string.Join("&", @params) + consumer_secret).ToLower();
            string paras = string.Join("&", @params) + "&sig=" + sig;
            return GetDoAsyncStatus(url, paras);
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
            "order_id="+model.OriginalOrderNo, //订单号
            "app_id="+app_id
            };
            @params.Sort();
            string url = ConfigSettings.Instance.MeiTuanArrivedAsyncStatus + "?";
            string sig = ETS.Security.MD5.DefaultEncrypt(url + string.Join("&", @params) + consumer_secret).ToLower();
            string paras = string.Join("&", @params) + "&sig=" + sig;
            return GetDoAsyncStatus(url, paras);
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
        public CreatePM_OpenApi SetCommissonInfo(CreatePM_OpenApi paramodel)
        {
            return paramodel;
        }

        /// <summary>
        /// 返回美团当前请求对应的签名  add by caoheyang 20150421
        /// </summary>
        /// <param name="fromModel">美团数据实体</param>
        /// <returns></returns>
        public string PostGetSig(System.Web.HttpRequest httpRequest)
        {
            List<string> paras = new List<string>();
            foreach (string key in httpRequest.Form.Keys)
            {
                if (key != "sig")
                {
                    string valtemp = System.Web.HttpUtility.UrlDecode(System.Web.HttpUtility.UrlDecode(httpRequest.Form[key]));
                    paras.Add(key + "=" + (valtemp == null ? "" : valtemp));
                }
            }
            paras.Sort();
            int index = httpRequest.Url.ToString().IndexOf('?');
            string url = (index < 0 ? httpRequest.Url.ToString() : httpRequest.Url.ToString().Substring(0, index)) + "?";
            string waimd5 = url + string.Join("&", paras) + consumer_secret; //consumer_secret
            string sigtemp = ETS.Security.MD5.DefaultEncrypt(waimd5).ToLower();
            return sigtemp;
        }

        /// <summary>
        /// 返回美团当前请求对应的签名  add by caoheyang 20150421
        /// </summary>
        /// <param name="fromModel">美团数据实体</param>
        /// <returns></returns>
        public string GetSig(System.Web.HttpRequest httpRequest)
        {
            List<string> paras = new List<string>();
            foreach (string key in httpRequest.QueryString.Keys)
            {
                if (key != "sig")
                {
                    string valtemp = System.Web.HttpUtility.UrlDecode(System.Web.HttpUtility.UrlDecode(httpRequest.QueryString[key]));
                    paras.Add(key + "=" + (valtemp == null ? "" : valtemp));
                }
            }
            paras.Sort(new NewStringComparer());  //TODO待长时间验证  目前慎用
            int index = httpRequest.Url.ToString().IndexOf('?');
            string url = (index < 0 ? httpRequest.Url.ToString() : httpRequest.Url.ToString().Substring(0, index)) + "?";
            string waimd5 = url + string.Join("&", paras) + consumer_secret; //consumer_secret
            string sigtemp = ETS.Security.MD5.DefaultEncrypt(waimd5).ToLower();
            return sigtemp;
        }


        /// <summary>
        /// 美团的订单数据转成通用的openapi接入订单数据实体类型 20150421
        /// </summary>
        /// <param name="fromModel">美团数据实体</param>
        public CreatePM_OpenApi TranslateModel(MeiTuanOrdeModel fromModel)
        {
            
            #region 订单基础数据 
            CreatePM_OpenApi model = new CreatePM_OpenApi();
            model.order_id = fromModel.order_id; //订单ID
            ///店铺信息
            Store store = new Store();
            store.store_id = fromModel.app_poi_code;//对接方店铺id
            store.store_name = fromModel.wm_poi_name;//店铺名称
            store.address = fromModel.wm_poi_address;//店铺地址
            store.phone = fromModel.wm_poi_address;//店铺电话
         
            model.remark = fromModel.caution;//备注
            model.status = OrderConst.OrderStatus30;//初始化订单状态 第三方代接入
            model.create_time = DateTime.Now;//订单发单时间 创建时间
            model.payment = fromModel.pay_type;//支付类型
            model.is_pay = fromModel.pay_type == 1 ? false : true;//目前货到付款时取未支付，在线支付取已支付
            model.total_price = fromModel.total;//订单金额
            model.store_info = store; //店铺 

            //订单明细不为空时做处理 
            if (!string.IsNullOrWhiteSpace(fromModel.detail) && fromModel.detail != "")
            {
                MeiTuanOrdeDetailModel[] meituandetails = Letao.Util.JsonHelper.JsonConvertToObject<MeiTuanOrdeDetailModel[]>(fromModel.detail);
                OrderDetail[] details = new OrderDetail[meituandetails.Length];
                for (int i = 0; i < meituandetails.Length; i++)
                {
                    OrderDetail tempdetail = new OrderDetail();
                    tempdetail.product_name = meituandetails[i].food_name;//菜品名称
                    tempdetail.quantity = meituandetails[i].quantity;//菜品数量
                    tempdetail.unit_price = meituandetails[i].price;//菜品单价
                    tempdetail.detail_id = 0;//美团不传递明细id，明细id为0
                    details[i] = tempdetail;
                }
                model.order_details = details; //订单ID
            }

            model.orderfrom = OrderConst.OrderFrom4;// 订单来源  美团订单的订单来源是 4
            model.receive_time = TimeHelper.TimeStampToDateTime(fromModel.ctime);//美团不传递，E代送必填 要求送餐时间 
            #endregion

            #region 订单商户相关信息  
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            int businessId = ParseHelper.ToInt(redis.Get<string>(string.Format(ETS.Const.RedissCacheKey.OtherBusinessIdInfo, model.orderfrom,
            model.store_info.store_id.ToString()))); //缓存中取E代送商户id
            if (businessId == 0)
                return null;
            BusListResultModel business = new Ets.Dao.User.BusinessDao().GetBusiness(businessId);
            model.store_info.delivery_fee = ParseHelper.ToDecimal(business.DistribSubsidy);//外送费
            model.store_info.businesscommission = ParseHelper.ToDecimal(business.BusinessCommission);//结算比例
            model.businessId = businessId;
            model.CommissionType = business.CommissionType;
            model.CommissionFixValue = business.CommissionFixValue;
            model.BusinessGroupId = business.BusinessGroupId;
            #endregion

            #region 订单地址信息
            Address address = new Address();
            address.address = fromModel.recipient_address;//用户收货地址
            address.user_phone = fromModel.recipient_phone;//用户联系电话
            address.user_name = fromModel.recipient_phone;//用户姓名
            address.longitude = fromModel.longitude; //经度
            address.latitude = fromModel.latitude; //纬度
            address.city_code = business.CityId; //市
            address.city = business.City;
            address.area_code = business.districtId; //区
            address.area = business.district;
            address.province = business.Province; //省
            address.province_code = business.ProvinceCode;
            model.address = address; //订单ID 
            #endregion

            #region 佣金相关  
            model.package_count = 1; //订单份数
            //佣金计算规则
            //model.CommissionFormulaMode = ParseHelper.ToInt(Ets.Dao.GlobalConfig.GlobalConfigDao.GlobalConfigGet(1).CommissionFormulaMode);
            model.CommissionFormulaMode = business.StrategyId;
            //计算获得订单骑士佣金
            Ets.Model.DataModel.Order.OrderCommission orderComm = new Ets.Model.DataModel.Order.OrderCommission()
            {
                Amount = model.total_price, /*订单金额*/
                DistribSubsidy = model.store_info.delivery_fee,/*外送费*/
                OrderCount = model.package_count,/*订单数量*/
                BusinessCommission = model.store_info.businesscommission,/*商户结算比例*/
                BusinessGroupId = model.BusinessGroupId,
                StrategyId = model.CommissionFormulaMode
            }/*网站补贴*/;
            //OrderPriceProvider commissonPro = CommissionFactory.GetCommission();
            OrderPriceProvider commissonPro = CommissionFactory.GetCommission(business.StrategyId);
            model.ordercommission = commissonPro.GetCurrenOrderCommission(orderComm);  //骑士佣金
            model.websitesubsidy = commissonPro.GetOrderWebSubsidy(orderComm);//网站补贴
            model.commissionrate = commissonPro.GetCommissionRate(orderComm);//订单佣金比例
            model.settlemoney = commissonPro.GetSettleMoney(orderComm);//订单结算金额
            model.adjustment = commissonPro.GetAdjustment(orderComm);//订单额外补贴金额
            #endregion

            //fromModel.extras 说明，暂时不用 
            return model;
        }

        /// <summary>
        /// 新增美团订单 add by caoheyang 20150421 
        /// </summary>
        /// <param name="fromModel">paraModel</param>
        public int AddOrder(CreatePM_OpenApi paramodel)
        {
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
        public int app_id { get; set; }

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
        public string ctime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public string utime { get; set; }
        /// <summary>
        /// 订单详细类目列表
        /// </summary>
        public string detail { get; set; }
        /// <summary>
        /// 订单活动类目列表
        /// </summary>
        public string extras { get; set; }

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
        public string app_food_code { get; set; }

        /// <summary>
        /// 打包餐盒数量
        /// </summary>
        public int box_num { get; set; }
        /// <summary>
        /// 打包餐盒单价
        /// </summary>
        public int box_price { get; set; }

        /// <summary>
        /// 菜品折扣，只是美团商家，APP方配送的门店才会设置，默认为1。折扣值不参与总价计算。
        /// </summary>
        public int food_discount { get; set; }

        /// <summary>
        /// APP方菜品名称
        /// </summary>
        public string food_name { get; set; }

        /// <summary>
        /// 菜品单价
        /// </summary>
        public decimal price { get; set; }

        /// <summary>
        /// 菜品数量
        /// </summary>
        public int quantity { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string unit { get; set; }


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
