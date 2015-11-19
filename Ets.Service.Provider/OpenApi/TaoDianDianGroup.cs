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
namespace Ets.Service.Provider.OpenApi
{
    public class TaoDianDianGroup : IGroupProviderOpenApi
    {
        BusinessDao businessDao=new BusinessDao();
        OrderDao orderDao = new OrderDao();
        OrderSubsidiesLogDao orderSubsidiesLogDao = new OrderSubsidiesLogDao();
        OrderOtherDao orderOtherDao = new OrderOtherDao();
        OrderChildDao orderChildDao = new OrderChildDao();

        /// <summary>
        /// 回调万达接口同步订单状态  add by caoheyang 20150326
        /// 茹化肖修改
        /// 2015年8月26日11:09:56
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        public OrderApiStatusType AsyncStatus(ParaModel<AsyncStatusPM_OpenApi> paramodel)
        {
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
            TaoBaoResponseBase res = JsonHelper.JsonConvertToObject<TaoBaoResponseBase>(json);
            return res.is_success ? OrderApiStatusType.Success : OrderApiStatusType.SystemError;
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
        public void TaoBaoPushOrder(OrderDispatch p)
        {
            try
            {
                #region 商户
                //查询商户是否存在
                BusListResultModel blrModel = businessDao.GetBusiness(p.store_id, GroupType.Group7.GetHashCode());
                if (blrModel == null)
                {
                    BusinessModel bModel = new BusinessModel();
                    bModel.Name = p.store_name;
                    bModel.City = p.city_name;
                    bModel.district = "";
                    bModel.PhoneNo = p.shipper_phone;
                    bModel.PhoneNo2 = p.shipper_phone;
                    bModel.Password = "";
                    bModel.CheckPicUrl = "";
                    bModel.IDCard = "";
                    bModel.Address = p.store_address;
                    string starting_point = p.starting_point;
                    bModel.Landline = "";
                    bModel.Longitude = 0;
                    bModel.Latitude = 0;
                    bModel.Status = 1;
                    bModel.districtId = "";
                    bModel.CityId = p.city_code;
                    bModel.GroupId = GroupType.Group7.GetHashCode();
                    bModel.OriginalBusiId = 0;
                    bModel.OriginalBusiUnitId = p.store_id;
                    bModel.ProvinceCode = "";
                    bModel.CityCode = "";
                    bModel.AreaCode = "";
                    bModel.Province = "";
                    bModel.CommissionTypeId = 0;//佣金类型
                    bModel.DistribSubsidy = 0;
                    bModel.BusinessCommission = 0;
                    bModel.CommissionType = 1;
                    bModel.BusinessGroupId = 1;//默认组
                    bModel.BalancePrice = 0;
                    bModel.AllowWithdrawPrice = 0;
                    bModel.HasWithdrawPrice = 0;
                    bModel.MealsSettleMode = 0;//线下结算
                    bModel.BusinessLicensePic = "";
                    bModel.IsBind = 0;
                    bModel.OneKeyPubOrder = 0;
                    bModel.IsAllowOverdraft = 0;
                    bModel.IsEmployerTask = 0;
                    bModel.RecommendPhone = "";
                    bModel.Timespan = "";
                    bModel.Appkey = Guid.NewGuid().ToString();
                    bModel.IsOrderChecked = 1;
                    bModel.IsAllowCashPay = 1;
                    bModel.LastLoginTime = System.DateTime.Now;

                    int businessId = businessDao.Insert(bModel);
                }
                #endregion

                #region 订单表
                order oModel=new order();
                //oModel.OrderNo = Helper.generateOrderCode(blrModel.userId, busiOrderInfoModel.TimeSpan);  //根据userId生成订单号(15位)
                int oId=orderDao.Insert(oModel);
                #endregion               

                #region 订单日志
                OrderSubsidiesLog oslModel=new OrderSubsidiesLog();
                int oslId= orderSubsidiesLogDao.Insert(oslModel);
                #endregion

                #region 订单Other
                OrderOtherModel ooModel=new OrderOtherModel();
                int ooId=orderOtherDao.Insert(ooModel);
                #endregion

                #region 子订单
                orderChildDao.InsertList(oModel);
                #endregion

            }
            catch (Exception err)
            {
                string str = err.Message;
            }
        }
    }
}
