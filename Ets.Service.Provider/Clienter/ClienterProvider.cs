using System.Collections.Generic;
using Ets.Dao.Clienter;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Service.IProvider.Clienter;
using Ets.Model.DomainModel.Clienter;
using ETS.Data.PageData;
using System;
using CalculateCommon;
using Ets.Service.Provider.Order;

namespace Ets.Service.Provider.Clienter
{
    public class ClienterProvider : IClienterProvider
    {
        readonly ClienterDao clienterDao = new ClienterDao();

        public List<order> GetOrdersNoLoginLatest(ClientOrderSearchCriteria criteria)
        {
            return clienterDao.GetOrdersNoLoginLatest(criteria);
        }

        /// <summary>
        /// 骑士上下班功能 add by caoheyang 20150312
        /// </summary>
        /// <param name="paraModel"></param>
        /// <returns></returns>
        public ETS.Enums.ChangeWorkStatusEnum ChangeWorkStatus(Ets.Model.ParameterModel.Clienter.ChangeWorkStatusPM paraModel)
        {
            if (paraModel.WorkStatus == ETS.Const.ClienterConst.ClienterWorkStatus1)  //如果要下班，先判断超人是否还有为完成的订单
            {
                //查询当前超人有无已接单但是未完成的订单
                int ordercount = clienterDao.QueryOrderount(new Model.ParameterModel.Clienter.ChangeWorkStatusPM() { Id = paraModel.Id, OrderStatus = ETS.Const.OrderConst.OrderStatus2 });
                if (ordercount > 0)
                    return ETS.Enums.ChangeWorkStatusEnum.OrderError;
            }
            return clienterDao.ChangeWorkStatusToSql(paraModel) > 0 ? ETS.Enums.ChangeWorkStatusEnum.Success : ETS.Enums.ChangeWorkStatusEnum.Error;
        }

        /// <summary>
        /// 获取我的任务   根据状态判断是已完成任务还是我的任务
        /// </summary>
        /// <returns></returns>
        public IList<ClientOrderResultModel> GetMyOrders(ClientOrderSearchCriteria clientOrderModel)
        {
            //throw new System.NotImplementedException();
            PageInfo<ClientOrderModel> pageinfo = new ClienterDao().GetMyOrders(clientOrderModel);
            IList<ClientOrderModel> list = pageinfo.Records;

            IList<ClientOrderResultModel> listOrder = new List<ClientOrderResultModel>();//组装成新的对象
            foreach (ClientOrderModel item in list)
            {
                ClientOrderResultModel model = new ClientOrderResultModel();
                model.userId = item.UserId;
                model.OrderNo = item.OrderNo;

                #region 骑士佣金计算
                OrderCommission oCommission = new OrderCommission()
                {
                    Amount = item.Amount,
                    CommissionRate = item.CommissionRate,
                    DistribSubsidy = item.DistribSubsidy,
                    OrderCount = item.OrderCount,
                    WebsiteSubsidy = item.WebsiteSubsidy
                };
                #endregion

                model.income = OrderCommissionProvider.GetCurrenOrderCommission(oCommission);  //计算设置当前订单骑士可获取的佣金 Edit bycaoheyang 20150305
                model.Amount = OrderCommissionProvider.GetCurrenOrderPrice(oCommission); //C端 获取订单的金额 Edit bycaoheyang 20150305

                model.businessName = item.BusinessName;
                model.businessPhone = item.BusinessPhone;
                model.pickUpCity = item.pickUpCity;
                model.pubDate = item.PubDate;

                model.pickUpAddress = item.PickUpAddress;
                model.receviceName = item.ReceviceName;
                model.receviceCity = item.ReceviceCity;
                model.receviceAddress = item.ReceviceAddress;
                model.recevicePhone = item.RecevicePhoneNo;
                model.IsPay = item.IsPay;
                model.Remark = item.Remark;
                model.Status = item.Status;

                #region 计算经纬度

                if (item.Longitude == null || item.Longitude == 0 || item.Latitude == null || item.Latitude == 0)
                {
                    model.distance = "--";
                    model.distanceB2R = "--";
                }
                else
                {
                    if (degree.longitude == 0 || degree.latitude == 0 || item.BusinessId <= 0)
                        model.distance = "--";
                    else if (item.BusinessId > 0)  //计算超人当前到商户的距离
                    {
                        Degree degree1 = new Degree(degree.longitude, degree.latitude);   //超人当前的经纬度
                        Degree degree2 = new Degree(item.Longitude.Value, item.Latitude.Value); //商户经纬度
                        double res = CoordDispose.GetDistanceGoogle(degree1, degree2);
                        model.distance = res < 1000 ? (res.ToString("f2") + "m") : ((res / 1000).ToString("f2") + "km");
                    }
                    if (item.BusinessId > 0 && item.ReceviceLongitude != null && item.ReceviceLatitude != null
                        && item.ReceviceLongitude != 0 && item.ReceviceLatitude != 0)  //计算商户到收货人的距离
                    {
                        Degree degree1 = new Degree(item.Longitude.Value, item.Latitude.Value);  //商户经纬度
                        Degree degree2 = new Degree(item.ReceviceLongitude.Value, item.ReceviceLatitude.Value);  //收货人经纬度
                        double res = CoordDispose.GetDistanceGoogle(degree1, degree2);
                        model.distanceB2R = res < 1000 ? (res.ToString("f2") + "m") : ((res / 1000).ToString("f2") + "km");
                    }
                    else
                        model.distanceB2R = "--";
                }        
                #endregion
                listOrder.Add(model);
            }

            return listOrder;
        }

    }
}
