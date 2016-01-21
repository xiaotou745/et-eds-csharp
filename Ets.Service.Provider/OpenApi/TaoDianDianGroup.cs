using System.Configuration;
using System.Net.Http;
using ETS.Const;
using Ets.Dao.Common;
using Ets.Dao.Order;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.Common.TaoBao;
using Ets.Model.ParameterModel.Order;
using ETS.Security;
using Ets.Service.IProvider.OpenApi;
using ETS.Util;
using Top.Api.Request;
using Ets.Dao.Business;
using Ets.Model.DataModel.Business;
using System;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Order;
using Ets.Service.Provider.Order;
using ETS.Sms;
using ETS.Transaction.Common;
using ETS.Transaction;
using System.Threading.Tasks;
namespace Ets.Service.Provider.OpenApi
{
    public class TaoDianDianGroup : IGroupProviderOpenApi
    {
        BusinessDao businessDao=new BusinessDao();
        OrderDao orderDao = new OrderDao();
        OrderSubsidiesLogDao orderSubsidiesLogDao = new OrderSubsidiesLogDao();
        OrderOtherDao orderOtherDao = new OrderOtherDao();
        OrderChildDao orderChildDao = new OrderChildDao();
        OrderDetailDao orderDetailDao = new OrderDetailDao();

        /// <summary>
        /// 回调万达接口同步订单状态  add by caoheyang 20150326
        /// 茹化肖修改
        /// 2015年8月26日11:09:56
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        public OrderApiStatusType AsyncStatus(ParaModel<AsyncStatusPM_OpenApi> paramodel)
        {
            LogHelper.LogWriter("淘点点入口参数",paramodel);
            string jsonData, url = null;
            switch (paramodel.fields.status)
            {
                case OrderConst.OrderStatus1:  //完成
                     jsonData = Confirm(paramodel.fields);
                     url = ConfigurationManager.AppSettings["TaoBaoConfirmAsyncStatus"];
                     break;
                case OrderConst.OrderStatus2:  //骑士接单
                     jsonData = Update(paramodel.fields);
                     url = ConfigurationManager.AppSettings["TaoBaoUpdateAsyncStatus"];
                     break;
                case OrderConst.OrderStatus4:  //订单已取货 配送中
                     jsonData = PickUp(paramodel.fields);
                     url = ConfigurationManager.AppSettings["TaoBaoPickUpAsyncStatus"];
                     break;
                default:
                    return OrderApiStatusType.Success;
            }
            var reqpar = "data=" + AESApp.AesEncrypt(jsonData);
            string json = new HttpClient().PostAsJsonAsync(url, new { data = AESApp.AesEncrypt(jsonData) }).Result.Content.ReadAsStringAsync().Result;
            Log(url, reqpar, json); //记录日志
            if (json != null && json != "")
            {
                TaoBaoResponseBase res = JsonHelper.JsonConvertToObject<TaoBaoResponseBase>(json);
                return res.is_success == true || res.result == true
                    ? OrderApiStatusType.Success
                    : OrderApiStatusType.SystemError;
            }
            else
            {
                return OrderApiStatusType.SystemError;
            }
        }


        /// <summary>
        /// 记录第三方调用日志
        /// caoheyang  20151117
        /// </summary>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private void Log(string url,string request, string response)
        {
            new HttpDao().LogThirdPartyInfo(new HttpModel()
            {
                Url = url,
                Htype = HtypeEnum.ReqType.GetHashCode(),
                RequestBody = request,
                ResponseBody = response,
                ReuqestPlatForm = RequestPlatFormEnum.OpenApiPlat.GetHashCode(),
                ReuqestMethod = "Ets.Service.Provider.OpenApi.TaoDianDianGroup.AsyncStatus",
                Status = 1,
                Remark = "调用淘点点:同步订单状态"
            });
        }

        /// <summary>
        /// 更新配送员信息接口（API） 
        /// caoheyang  20151116
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>

        private string Update(AsyncStatusPM_OpenApi p)
        {
            var order = new OrderDao().GetOrderMapDetail(p.order_no);
            var temp = new 
            {
                deliveryOrderNo = ParseHelper.ToLong(p.OriginalOrderNo),
                delivererPhone=p.ClienterPhoneNo,
                delivererName=p.ClienterTrueName,
                lng = order.GrabLongitude.ToString(),
                lat = order.GrabLatitude.ToString()
            };
            return JsonHelper.JsonConvertToString(temp);
        }

        /// <summary>
        /// 取件（API）  配送中
        /// caoheyang  20151116
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string PickUp(AsyncStatusPM_OpenApi p)
        {
            var order = new OrderDao().GetOrderMapDetail(p.order_no);
            var temp = new 
            {
                deliveryOrderNo = ParseHelper.ToLong(p.OriginalOrderNo),
                lng = order.TakeLongitude.ToString(),
                lat = order.TakeLatitude.ToString()
            };
            return JsonHelper.JsonConvertToString(temp);
        }

        /// <summary>
        /// 妥投（API） 订单完成
        /// caoheyang  20151116
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string Confirm(AsyncStatusPM_OpenApi p)
        {
            var order = new OrderDao().GetOrderMapDetail(p.order_no);
            var temp = new 
            {
                deliveryOrderNo = ParseHelper.ToLong(p.OriginalOrderNo),
                lng = order.CompleteLongitude.ToString(),
                lat = order.CompleteLatitude.ToString()
            };
            return JsonHelper.JsonConvertToString(temp);
        }

        /// <summary>
        /// 新增商铺时根据集团id为店铺设置外送费，结算比例等财务相关信息 add by caoheyang 20150417
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        public CreatePM_OpenApi SetCommissonInfo(CreatePM_OpenApi paramodel)
        {
            return paramodel;
        }

        /// <summary>
        ///  发布订单 
        ///  胡灵波
        ///  2015年11月18日 13:14:19
        /// </summary>
        /// <param name="info"></param>
        public TaoBaoPushOrder TaoBaoPushOrder(OrderDispatch p)
        {
            try
            {
                #region 商户
                //查询商户是否存在
                BusinessModel bModel = businessDao.GetBusiness(p.store_id, GroupConst.Group100);
                
                if (bModel == null)
                {
                    bModel = new BusinessModel();
                    bModel.Name = p.store_name;
                    bModel.City = p.city_name;

                    bModel.district = "";
                    bModel.PhoneNo = p.shipper_phone;
                    bModel.PhoneNo2 = p.shipper_phone;
                    bModel.Password = MD5Helper.MD5("123456");
                    bModel.CheckPicUrl = "";
                    bModel.IDCard = "";
                    bModel.Address = p.store_address;                           
                    bModel.Landline = "";

                    //转换坐标
                    bModel.Longitude = 0;
                    bModel.Latitude = 0;                  
                    if (!string.IsNullOrEmpty(p.starting_point))
                    {
                        string[] sp = p.starting_point.Replace("LngLatAlt", "").Replace("{", "").Replace("}", "").Split(',');
                        if(sp.Length>0)
                        {
                            string sp0 = sp[0].Split('=')[1];
                            string sp1 = sp[1].Split('=')[1];
                            if (!string.IsNullOrEmpty(sp0) && !string.IsNullOrEmpty(sp1))
                            {
                                bModel.Longitude =Convert.ToDouble(sp0);
                                bModel.Latitude = Convert.ToDouble(sp1);
                            }
                        }
                    }
                    bModel.Status = 1;//默认审核通过
                    bModel.districtId = "";
                    bModel.CityId = p.city_code;
                    bModel.GroupId = GroupConst.Group100;
                    bModel.OriginalBusiId = 0;
                    bModel.OriginalBusiUnitId = p.store_id;
                    bModel.ProvinceCode = "";
                    bModel.CityCode = p.city_code;
                    bModel.AreaCode = "";
                    bModel.Province = "";
                    bModel.CommissionTypeId = 0;//佣金类型
                    bModel.DistribSubsidy = 0;//默认0
                    bModel.BusinessCommission = 0;//默认0
                    bModel.CommissionType = 1;//默认1
                    bModel.BusinessGroupId = 1;//默认1
                    bModel.BalancePrice = 0;
                    bModel.AllowWithdrawPrice = 0;
                    bModel.HasWithdrawPrice = 0;
                    bModel.MealsSettleMode = 0;//线下结算
                    bModel.BusinessLicensePic = "";
                    bModel.IsBind = 0;//是否绑定了骑士（0：否 1：是）
                    bModel.OneKeyPubOrder = 0;
                    bModel.IsAllowOverdraft = 0;
                    bModel.IsEmployerTask = 0;
                    bModel.RecommendPhone = "";
                    bModel.Timespan = "";
                    bModel.Appkey = Guid.NewGuid().ToString();
                    bModel.IsOrderChecked = 1;
                    bModel.IsAllowCashPay = 1;
                    bModel.LastLoginTime = System.DateTime.Now;
                    bModel.Id = businessDao.Insert(bModel); 
                }
                #endregion

                using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
                {

                    bool isExist=orderOtherDao.IsExistByOrderNo(p.delivery_order_no.ToString());
                    if (isExist)
                    {
                        return ETS.Enums.TaoBaoPushOrder.OrderId;    
                    }
                    #region 订单表
                    order oModel = new order();
                    TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    oModel.OrderNo = Helper.generateOrderCode(bModel.Id, (ts.TotalSeconds).ToString());  //根据userId生成订单号(15位)
                    oModel.businessId = bModel.Id;
                    oModel.PickUpCity = bModel.City;  //商户所在城市
                    oModel.PickUpAddress = bModel.Address;  //取货地址
                    oModel.PubDate = DateTime.Now; //取货时间
                    oModel.ReceviceCity = bModel.City; //城市
                    //oModel.DistribSubsidy = p.delivery_fee/100m;//外送费
                    oModel.DistribSubsidy = 0;
                    oModel.BusinessCommission = ParseHelper.ToDecimal(bModel.BusinessCommission);//商户结算比例
                    oModel.BusinessName = bModel.Name;
                    oModel.CommissionType = bModel.CommissionType;//结算类型：1：固定比例 2：固定金额
                    oModel.CommissionFixValue = ParseHelper.ToDecimal(bModel.CommissionFixValue);//固定金额     
                    oModel.BusinessGroupId = bModel.BusinessGroupId;
                    oModel.MealsSettleMode = bModel.MealsSettleMode;
                    oModel.OneKeyPubOrder = bModel.OneKeyPubOrder;
                    oModel.IsOrderChecked = bModel.IsOrderChecked;
                    oModel.IsAllowCashPay = bModel.IsAllowCashPay;
                    oModel.IsBindGroup = 0;//是否绑定集团
                    oModel.GroupBusiName = "";
                    oModel.BussGroupAmount = 0;
                    oModel.BussGroupIsAllowOverdraft = 0;
                    oModel.BalancePrice = bModel.BalancePrice;
                    oModel.OrderFrom = GroupConst.Group100;//订单来源
                    //oModel.Remark = p.order_ext_info;//
                    oModel.Remark ="";
                    oModel.ReceviceName = p.consignee_name;
                    oModel.RecevicePhoneNo = p.consignee_phone;
                    oModel.ReceviceAddress = p.consignee_address;
                    oModel.IsPay = true;
                    oModel.Amount = ParseHelper.ToDecimal(p.actually_paid)/100m;
                    oModel.OrderCount = p.quantity;  //订单数量
                    //收货 经纬度            
                    oModel.ReceviceLongitude = 0;
                    oModel.ReceviceLatitude = 0;
                    oModel.OriginalOrderNo = p.delivery_order_no.ToString();
                    oModel.OriginalOrderId = p.delivery_order_no;
                    if (!string.IsNullOrEmpty(p.destination_point))
                    {
                        string[] sp = p.starting_point.Replace("LngLatAlt", "").Replace("{", "").Replace("}", "").Split(',');
                        if (sp.Length > 0)
                        {
                            string sp0 = sp[0].Split('=')[1];
                            string sp1 = sp[1].Split('=')[1];
                            if (!string.IsNullOrEmpty(sp0) && !string.IsNullOrEmpty(sp1))
                            {
                                bModel.Longitude = Convert.ToDouble(sp0);
                                bModel.Latitude = Convert.ToDouble(sp1);
                            }
                        }
                    }
                    oModel.PubLongitude = bModel.Longitude.Value;//发单经纬度 取商户
                    oModel.PubLatitude = bModel.Latitude.Value;
                    oModel.IsPubDateTimely = 0;//是否及时上传坐标

                    OrderCommission orderComm = new OrderCommission()
                    {
                        Amount = oModel.Amount, /*订单金额*/
                        DistribSubsidy = oModel.DistribSubsidy,/*外送费*/
                        OrderCount = p.quantity/*订单数量*/,
                        BusinessCommission = bModel.BusinessCommission.Value, /*商户结算比例*/
                        CommissionType = bModel.CommissionType,/*结算类型：1：固定比例 2：固定金额*/
                        CommissionFixValue = bModel.CommissionFixValue,/*固定金额*/
                        BusinessGroupId = bModel.BusinessGroupId,
                        StrategyId = bModel.StrategyId
                    };

                    OrderPriceProvider commProvider = CommissionFactory.GetCommission(bModel.StrategyId);
                    oModel.CommissionFormulaMode = bModel.StrategyId;
                    oModel.CommissionRate =0; //佣金比例 
                    oModel.BaseCommission =0;//基本佣金
                    oModel.OrderCommission = p.delivery_fee / 100m; //订单佣金
                    oModel.WebsiteSubsidy = 0;//网站补贴
                    oModel.SettleMoney = 0;      

                    oModel.Adjustment = 0;//订单额外补贴金额           
                    oModel.Status = Convert.ToByte(OrderStatus.Status0.GetHashCode());
                    oModel.TimeSpan = bModel.Timespan;

                    if (!(bool)oModel.IsPay && oModel.MealsSettleMode == MealsSettleMode.LineOn.GetHashCode())//未付款且线上支付
                    {
                        oModel.BusinessReceivable = Decimal.Round(ParseHelper.ToDecimal(oModel.Amount) +
                                       ParseHelper.ToDecimal(oModel.DistribSubsidy) * ParseHelper.ToInt(oModel.OrderCount), 2);//第三方如果设置商家外送费会多给第三方商户返回菜品金额+外送费
                    }

                    if (bModel.IsBindGroup == 1 && oModel.SettleMoney > bModel.BalancePrice)
                    {
                        oModel.GroupBusinessId = bModel.BussGroupId;
                    }

                    oModel.ProductName = p.title;//产品名称
                    
                    int oId = orderDao.Insert(oModel);
                    #endregion

                    #region 订单日志
                    OrderSubsidiesLog oslModel = new OrderSubsidiesLog();
                    oslModel.OrderId = oId;
                    oslModel.OptName = oModel.BusinessName;
                    oslModel.Remark = OrderConst.PublishOrder;
                    oslModel.OptId = bModel.Id;
                    oslModel.OrderStatus = OrderStatus.Status0.GetHashCode();
                    oslModel.Platform = SuperPlatform.FromBusiness.GetHashCode();
                    int oslId = orderSubsidiesLogDao.Insert(oslModel);
                    #endregion

                    #region 订单Other
                    OrderOtherModel ooModel = new OrderOtherModel();
                    ooModel.OrderId = oId;
                    ooModel.NeedUploadCount = 0;//需上传小票数量
                    ooModel.ReceiptPic = "";
                    ooModel.PubLatitude = Convert.ToDecimal(bModel.Latitude.Value);//发单经纬度
                    ooModel.PubLongitude = Convert.ToDecimal(bModel.Longitude.Value);
                    ooModel.OneKeyPubOrder = bModel.OneKeyPubOrder;
                    ooModel.IsOrderChecked = bModel.IsOrderChecked;
                    ooModel.IsAllowCashPay = bModel.IsAllowCashPay;
                    ooModel.IsPubDateTimely = 0;
                    ooModel.DeliveryOrderNo = p.delivery_order_no;
                    ooModel.NotifyTime = TimeHelper.TimeStampToCurrDateTime(Convert.ToDouble(p.notify_time));
                    ooModel.EndTime = TimeHelper.TimeStampToCurrDateTime(Convert.ToDouble(p.end_time));
                    ooModel.ExpectedTakeTime = TimeHelper.TimeStampToCurrDateTime(Convert.ToDouble(p.expected_take_time));
                    ooModel.ExpectedDelivery = TimeHelper.TimeStampToCurrDateTime(Convert.ToDouble(p.expected_delivery));
                    ooModel.ReceiptId = p.receipt_id;
                    int ooId = orderOtherDao.Insert(ooModel);
                    #endregion

                    #region 子订单
                    OrderChild ocMode = new OrderChild();
                    ocMode.OrderId = oId;
                    ocMode.ChildId = 1;
                    ocMode.TotalPrice = p.actually_paid/100m;//商品总价格
                    ocMode.GoodPrice = p.actually_paid/100m;//商品价格
                    ocMode.DeliveryPrice = p.delivery_fee/100m;//配送费
                    ocMode.PayStyle = 1;//用户支付
                    ocMode.PayType = 0;//支付类型
                    ocMode.PayStatus = 1;//已支付
                    ocMode.PayBy = p.consignee_name;//支付人
                    ocMode.PayTime = DateTime.Now;//支付时间
                    ocMode.PayPrice = p.actually_paid/100m;//支付金额
                    ocMode.HasUploadTicket = false;//是否上传小票
                    ocMode.TicketUrl = "";//小票地址
                    ocMode.CreateBy = "淘宝推送订单";//创建人
                    ocMode.CreateTime = DateTime.Now;
                    ocMode.UpdateBy = "淘宝推送订单";
                    ocMode.UpdateTime = DateTime.Now;
                    long ocId = orderChildDao.Insert(ocMode);
                    #endregion

                    #region 订单明细 临时
                    for (int i = 0; i < p.itemsList.Count; i++)
                    {                        
                        Ets.Model.DataModel.Order.OrderDetail odModel = new Ets.Model.DataModel.Order.OrderDetail();
                        odModel.OrderNo = oModel.OrderNo;
                        odModel.ProductName = p.itemsList[i].itemName;//商品名称
                        odModel.UnitPrice = p.itemsList[i].unitPrice/100m;
                        odModel.Quantity = p.itemsList[i].quantity;
                        odModel.FormDetailID = 0;
                        odModel.GroupID = 0;
                        if (p.itemsList[i].unit != null)
                            odModel.Unit = p.itemsList[i].unit;
                        else
                            odModel.Unit = "";
                        if (p.itemsList[i].unitWeight!=null)
                            odModel.UnitWeight=p.itemsList[i].unitWeight;
                        else
                            odModel.UnitWeight ="";
                        if (p.itemsList[i].totalWeight != null)
                            odModel.TotalWeight = p.itemsList[i].totalWeight;
                        else
                            odModel.TotalWeight = "";
                        odModel.TotalPrice = p.itemsList[i].totalPrice / 100m; 
                        long odId = orderDetailDao.Insert(odModel);
                    }

                    #endregion

                    if (oId > 0 && oslId > 0 && ooId > 0 && ocId > 0)
                    {
                        tran.Complete();
                    }
                }                
            }
            catch (Exception err)
            {
                LogHelper.LogWriter("错误",err.Message);
                string str = err.Message;
                return ETS.Enums.TaoBaoPushOrder.Error;    
            }
            return ETS.Enums.TaoBaoPushOrder.Success;
        }
        /// <summary>
        /// 催单更新数据库催单字段，发送催单短信
        /// wangchao
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public TaoBaoOrderRemindEnum TaoBaoOrderRemind(OrderRemind p)
        {
             OrderRemindModel orderRemindModel = new OrderDao().GetByDeliveryOrderNo(p.delivery_order_no);
            if (orderRemindModel == null)
            {
                return TaoBaoOrderRemindEnum.OrderNotExist;
            }
            else
            {
                if (orderRemindModel.IsOrderRemind == 1)
                {
                    return  TaoBaoOrderRemindEnum.HadOrderRemind;
                }
                string msg = string.Format(SupermanApiConfig.Instance.SmsContentOrderRemind, orderRemindModel.ReceviceName.Trim(), orderRemindModel.ClienterPhoneNo.Trim());
                if (!string.IsNullOrWhiteSpace(orderRemindModel.ClienterPhoneNo))
                {
                    Task.Factory.StartNew(() =>
                    {
                        SendSmsHelper.SendSendSmsSaveLog(orderRemindModel.RecevicePhoneNo, msg, SystemConst.SMSSOURCE);
                    });
                }
                int k = new OrderDao().UpdateByDeliveryOrderNo(orderRemindModel.Id, p.event_time);
                if (k > 0)
                {
                    return TaoBaoOrderRemindEnum.Success;
                }
                else
                {
                    return TaoBaoOrderRemindEnum.Error;
                }
            }
            
        }
    }
}
