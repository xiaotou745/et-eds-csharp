﻿using System.Collections.Generic;
using Ets.Dao.Clienter;
using Ets.Dao.User;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Bussiness;
using Ets.Model.ParameterModel.Clienter;
using Ets.Service.IProvider.Clienter;
using Ets.Model.DomainModel.Clienter;
using ETS.Data.PageData;
using System;
using CalculateCommon;
using Ets.Service.Provider.Order;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Dao.WtihdrawRecords;
using ETS.Util;

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
            if (paraModel.WorkStatus == ETS.Const.ClienterConst.ClienterWorkStatus1)  //如果要下班，先判断超人是否还有未完成的订单
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
        public virtual IList<ClientOrderResultModel> GetMyOrders(ClientOrderSearchCriteria clientOrderModel)
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

                model.income = item.OrderCommission;  //佣金 Edit bycaoheyang 20150327
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
                model.OrderCount = item.OrderCount;
                #region 计算经纬度     待封装  add by caoheyang 20150313

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
                        model.distance = res < 1000 ? (res.ToString("f2") + "米") : ((res / 1000).ToString("f2") + "公里");
                    }
                    if (item.BusinessId > 0 && item.ReceviceLongitude != null && item.ReceviceLatitude != null
                        && item.ReceviceLongitude != 0 && item.ReceviceLatitude != 0)  //计算商户到收货人的距离
                    {
                        Degree degree1 = new Degree(item.Longitude.Value, item.Latitude.Value);  //商户经纬度
                        Degree degree2 = new Degree(item.ReceviceLongitude.Value, item.ReceviceLatitude.Value);  //收货人经纬度
                        double res = CoordDispose.GetDistanceGoogle(degree1, degree2);
                        model.distanceB2R = res < 1000 ? (res.ToString("f2") + "米") : ((res / 1000).ToString("f2") + "公里");
                    }
                    else
                        model.distanceB2R = "--";
                }
                #endregion
                listOrder.Add(model);
            }

            return listOrder;
        }


        /// <summary>
        /// c端用户登录
        /// 窦海超
        /// 2015年3月17日 15:11:46
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.Common.ResultModel<ClienterLoginResultModel> PostLogin_C(Model.ParameterModel.Clienter.LoginModel model)
        {
            try
            {
                ClienterLoginResultModel resultModel = clienterDao.PostLogin_CSql(model);
                if (resultModel == null)
                {
                    return ResultModel<ClienterLoginResultModel>.Conclude(LoginModelStatus.InvalidCredential);
                }
                return ResultModel<ClienterLoginResultModel>.Conclude(LoginModelStatus.Success, resultModel);
            }
            catch (Exception ex)
            {
                return ResultModel<ClienterLoginResultModel>.Conclude(LoginModelStatus.InvalidCredential);
                throw;
            }
        }


        /// <summary>
        /// 获取当前配送员的流水信息
        /// 窦海超
        /// 2015年3月20日 17:12:11
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public ClienterRecordsListModel WtihdrawRecords(int UserId)
        {
            ClienterRecordsListModel listModel = new ClienterRecordsListModel();
            listModel.clienterModel = clienterDao.GetUserInfoByUserId(UserId);
            listModel.listClienterRecordsModel = new WtihdrawRecordsDao().GetClienterRecordsByUserId(UserId);
            return listModel;
        }

        /// <summary>
        ///  修改密码
        ///  窦海超
        ///  2015年3月23日 18:45:54
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel<ClienterModifyPwdResultModel> PostForgetPwd_C(Ets.Model.DataModel.Clienter.ModifyPwdInfoModel model)
        {
            if (string.IsNullOrEmpty(model.newPassword))
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ModifyPwdStatus.NewPwdEmpty);
            }
            var clienter = clienterDao.GetUserInfoByUserPhoneNo(model.phoneNo);
            if (clienter == null)
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ModifyPwdStatus.ClienterIsNotExist);
            }
            bool b = clienterDao.UpdateClienterPwdSql(clienter.Id, model.newPassword);
            if (b)
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ModifyPwdStatus.Success);
            }
            else
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ModifyPwdStatus.FailedModifyPwd);
            }
        }
        /// <summary>
        /// 判断 骑士端 手机号 是否注册过
        /// wc
        /// </summary>
        /// <param name="PhoneNo"></param>
        /// <returns></returns>
        public bool CheckClienterExistPhone(string PhoneNo)
        {
            return clienterDao.CheckClienterExistPhone(PhoneNo);
        }
        /// <summary>
        /// 判断该骑士是否有资格 
        /// wc
        /// </summary>
        /// <param name="clienterId"></param>
        /// <returns></returns>
        public bool HaveQualification(int clienterId)
        {
            return clienterDao.HaveQualification(clienterId);
        }

        /// <summary>
        /// 骑士注册 平扬 2015.3.30
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel<ClientRegisterResultModel> PostRegisterInfo_C(ClientRegisterInfoModel model)
        {
            if (string.IsNullOrEmpty(model.phoneNo))  //手机号非空验证
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberEmpty);
            else if (clienterDao.CheckExistPhone(model.phoneNo))  //判断该手机号是否已经注册过
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberRegistered);
            else if (string.IsNullOrEmpty(model.passWord)) //密码非空验证
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PasswordEmpty);
            else if (string.IsNullOrEmpty(model.City) || string.IsNullOrEmpty(model.CityId)) //城市以及城市编码非空验证
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.cityIdEmpty);
            else if (model.verifyCode != SupermanApiCaching.Instance.Get(model.phoneNo)) //判断验码法录入是否正确
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.IncorrectCheckCode);
            else if (model.recommendPhone != null && (!clienterDao.CheckExistPhone(model.recommendPhone))
                && (!new BusinessDao().CheckExistPhone(model.recommendPhone))) //如果推荐人手机号在B端C端都不存在提示信息
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberNotExist);
            var clienter = ClientRegisterInfoModelTranslator.Instance.Translate(model);
            int id = clienterDao.AddClienter(clienter);
            var resultModel = new ClientRegisterResultModel
            {
                userId = id,
                city = string.IsNullOrWhiteSpace(clienter.City) ? null : clienter.City.Trim(),  //城市
                cityId = string.IsNullOrWhiteSpace(clienter.CityId) ? null : clienter.CityId.Trim()  //城市编码
            };
            if (id > 0)
            {
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.Success, resultModel);
            }
            return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.ClientRegisterFaild);
        }

        /// <summary>
        /// 抢单 平扬 2015.3.30
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public bool RushOrder(int userId, string orderNo)
        { 
            try
            {
                return clienterDao.RushOrder(userId, orderNo); 
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return false;
        }
    }
}
