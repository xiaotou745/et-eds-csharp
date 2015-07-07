using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETS.Const;
using Ets.Dao.Clienter;
using Ets.Dao.Message;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Business;
using Ets.Model.ParameterModel.Clienter;
using Ets.Service.IProvider.Clienter;
using Ets.Model.DomainModel.Clienter;
using ETS.Data.PageData;
using System;
using CalculateCommon;
using Ets.Dao.Finance;
using Ets.Service.Provider.Order;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Dao.WtihdrawRecords;
using ETS.Util;
using ETS.Transaction.Common;
using ETS.Transaction;
using Ets.Dao.Order;
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.WtihdrawRecords;
using Ets.Service.Provider.MyPush;
using Ets.Model.DomainModel.Business;
using Ets.Model.ParameterModel.Order;
using Ets.Model.DomainModel.Order;
using Ets.Service.IProvider.Order;
using Ets.Model.ParameterModel.Finance;
using Ets.Dao.Business;
using Ets.Model.DomainModel.GlobalConfig;
using Ets.Service.Provider.Common;
using Ets.Service.IProvider.Common;
namespace Ets.Service.Provider.Clienter
{
    public class ClienterProvider : IClienterProvider
    {
        readonly ClienterDao clienterDao = new ClienterDao();
        readonly OrderDao orderDao = new OrderDao();
        readonly OrderOtherDao orderOtherDao = new OrderOtherDao();
        readonly OrderChildDao orderChildDao = new OrderChildDao();
        readonly ClienterBalanceRecordDao clienterBalanceRecordDao = new ClienterBalanceRecordDao();
        readonly BusinessDao businessDao = new BusinessDao();
        readonly BusinessClienterRelationDao businessClienterDao = new BusinessClienterRelationDao();
        readonly BusinessBalanceRecordDao businessBalanceRecordDao = new BusinessBalanceRecordDao();

        readonly IAreaProvider iAreaProvider = new AreaProvider();
        readonly IOrderOtherProvider iOrderOtherProvider = new OrderOtherProvider();

        /// <summary>
        /// 骑士上下班功能 add by caoheyang 20150312
        /// </summary>
        /// <param name="paraModel"></param>
        /// <returns></returns>
        public ChangeWorkStatusEnum ChangeWorkStatus(Ets.Model.ParameterModel.Clienter.ChangeWorkStatusPM paraModel)
        {
            if (paraModel.WorkStatus == ClienteWorkStatus.Status1.GetHashCode())  //如果要下班，先判断超人是否还有未完成的订单
            {
                //查询当前超人有无已接单但是未完成的订单
                int ordercount = clienterDao.QueryOrderount(new Model.ParameterModel.Clienter.ChangeWorkStatusPM() { Id = paraModel.Id });
                if (ordercount > 0)
                    return ChangeWorkStatusEnum.OrderError;
            }
            int changeResult = clienterDao.ChangeWorkStatusToSql(paraModel);
            if (changeResult <= 0)
            {
                if (paraModel.WorkStatus == ClienteWorkStatus.Status0.GetHashCode())
                {
                    return ChangeWorkStatusEnum.WorkError; //上班失败
                }
                else
                {
                    return ChangeWorkStatusEnum.StatusError; //休息失败
                }
            }
            else
            {
                if (paraModel.WorkStatus == ClienteWorkStatus.Status0.GetHashCode())
                {
                    return ChangeWorkStatusEnum.WorkSuccess;  //上班成功
                }
                else
                {
                    return ChangeWorkStatusEnum.StatusSuccess; //休息成功
                }
            }
        }

        /// <summary>
        /// 获取我的任务   根据状态判断是已完成任务还是我的任务
        /// </summary>
        /// <returns></returns>
        public virtual IList<ClientOrderResultModel> GetMyOrders(ClientOrderSearchCriteria clientOrderModel)
        {
            //throw new System.NotImplementedException();
            IList<ClientOrderResultModel> listOrder = new List<ClientOrderResultModel>();//组装成新的对象
            PageInfo<ClientOrderModel> pageinfo = new ClienterDao().GetMyOrders(clientOrderModel);
            if (pageinfo != null && pageinfo.Records != null)
            {
                IList<ClientOrderModel> list = pageinfo.Records;
                foreach (ClientOrderModel item in list)
                {
                    ClientOrderResultModel model = new ClientOrderResultModel();
                    model.userId = item.UserId;
                    model.OrderNo = item.OrderNo;
                    model.OrderId = item.OrderId;
                    #region 骑士佣金计算
                    OrderCommission oCommission = new OrderCommission()
                    {
                        Amount = item.Amount,
                        DistribSubsidy = item.DistribSubsidy,
                        OrderCount = item.OrderCount
                    };
                    #endregion
                    model.OriginalOrderNo = item.OriginalOrderNo;
                    model.OrderFrom = item.OrderFrom;
                    model.income = item.OrderCommission;  //佣金 Edit bycaoheyang 20150327
                    model.Amount = DefaultOrPriceProvider.GetCurrenOrderPrice(oCommission); //C端 获取订单的金额 Edit bycaoheyang 20150305

                    model.businessName = item.BusinessName;
                    model.businessPhone = item.BusinessPhone;
                    model.businessPhone2 = item.BusinessPhone2;
                    model.pickUpCity = item.pickUpCity;
                    model.pubDate = item.PubDate;

                    model.pickUpAddress = item.PickUpAddress;
                    model.receviceName = item.ReceviceName;
                    model.receviceCity = item.ReceviceCity;
                    //model.receviceAddress = item.ReceviceAddress;
                    if (!string.IsNullOrEmpty(item.ReceviceAddress))
                        model.receviceAddress = item.ReceviceAddress;
                    else
                    {
                        model.receviceAddress = OrderConst.ReceviceAddress;
                    }

                    model.recevicePhone = item.RecevicePhoneNo;
                    model.IsPay = item.IsPay;
                    model.Remark = item.Remark;
                    model.Status = item.Status;
                    model.OrderCount = item.OrderCount;
                    model.GroupId = item.GroupId;
                    model.HadUploadCount = item.HadUploadCount;
                    if (item.GroupId == GroupType.Group3.GetHashCode()) //全时 需要做验证码验证
                        model.NeedPickupCode = 1;
                    #region 计算经纬度     待封装  add by caoheyang 20150313

                    if (item.Longitude == null || item.Longitude == 0 || item.Latitude == null || item.Latitude == 0)
                    {
                        model.distance = "--";
                        model.distanceB2R = "--";
                        model.distance_OrderBy = 9999999.0;
                    }
                    else
                    {
                        if (degree.longitude == 0 || degree.latitude == 0 || item.BusinessId <= 0)
                        { model.distance = "--"; model.distance_OrderBy = 9999999.0; }
                        else if (item.BusinessId > 0)  //计算超人当前到商户的距离
                        {
                            Degree degree1 = new Degree(degree.longitude, degree.latitude);   //超人当前的经纬度
                            Degree degree2 = new Degree(item.Longitude.Value, item.Latitude.Value); //商户经纬度
                            var res = ParseHelper.ToDouble(CoordDispose.GetDistanceGoogle(degree1, degree2));
                            model.distance = res < 1000 ? (Math.Round(res).ToString() + "米") : ((res / 1000).ToString("f2") + "公里");
                            model.distance_OrderBy = res;
                        }
                        if (item.BusinessId > 0 && item.ReceviceLongitude != null && item.ReceviceLatitude != null
                            && item.ReceviceLongitude != 0 && item.ReceviceLatitude != 0)  //计算商户到收货人的距离
                        {
                            Degree degree1 = new Degree(item.Longitude.Value, item.Latitude.Value);  //商户经纬度
                            Degree degree2 = new Degree(item.ReceviceLongitude.Value, item.ReceviceLatitude.Value);  //收货人经纬度
                            var res = ParseHelper.ToDouble(CoordDispose.GetDistanceGoogle(degree1, degree2));
                            model.distanceB2R = res < 1000 ? (Math.Round(res).ToString() + "米") : ((res / 1000).ToString("f2") + "公里");
                        }
                        else
                            model.distanceB2R = "--";
                    }
                    #endregion
                    listOrder.Add(model);
                }
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
        public ResultModel<ClienterLoginResultModel> PostLogin_C(Model.ParameterModel.Clienter.LoginCPM model)
        {
            try
            {
                ClienterLoginResultModel resultModel = clienterDao.PostLogin_CSql(model);
                if (resultModel == null)
                {
                    return ResultModel<ClienterLoginResultModel>.Conclude(LoginModelStatus.InvalidCredential);
                }
                if (resultModel.IsBind == 1)
                {
                    resultModel.IsOnlyShowBussinessTask = IsOnlyShowBussinessTask(resultModel.userId);
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
        /// 2015年7月7日10:18:44 茹化肖 修改推荐人B端验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel<ClientRegisterResultModel> PostRegisterInfo_C(ClientRegisterInfoModel model)
        {
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            var code = redis.Get<string>(RedissCacheKey.PostRegisterInfo_C + model.phoneNo);
            if (string.IsNullOrEmpty(model.phoneNo))  //手机号非空验证
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberEmpty);
            else if (clienterDao.CheckClienterExistPhone(model.phoneNo))  //判断该手机号是否已经注册过
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberRegistered);
            else if (string.IsNullOrEmpty(model.passWord)) //密码非空验证
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PasswordEmpty);
            //else if (string.IsNullOrEmpty(model.City) || string.IsNullOrEmpty(model.CityId)) //城市以及城市编码非空验证
            //    return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.cityIdEmpty);

            else if (string.IsNullOrEmpty(code) || model.verifyCode != code) //判断验码法录入是否正确
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.IncorrectCheckCode);
            else if (!string.IsNullOrEmpty(model.recommendPhone) && (!clienterDao.CheckClienterExistPhone(model.recommendPhone))
                && (!new BusinessDao().CheckRecommendPhone(model.recommendPhone))) //如果推荐人手机号在B端C端都不存在提示信息 
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberNotExist);



            var clienter = ClientRegisterInfoModelTranslator.Instance.Translate(model);
            //根据用户传递的  名称，取得 国标编码 wc,这里的 city 是二级 ，已和康珍 确认过
            //新版的 骑士 注册， 城市 非 必填
            if (!string.IsNullOrWhiteSpace(clienter.City))
            {
                Model.DomainModel.Area.AreaModelTranslate areaModel = iAreaProvider.GetNationalAreaInfo(new Model.DomainModel.Area.AreaModelTranslate() { Name = clienter.City.Trim(), JiBie = 2 });
                if (areaModel != null)
                {
                    clienter.CityId = areaModel.NationalCode.ToString();
                }
                else
                {
                    clienter.CityId = model.CityId;
                }
            }
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
            bool result = false;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                result = clienterDao.RushOrder(userId, orderNo);
                tran.Complete();
            }
            Task.Factory.StartNew(() =>
            {
                if (result)
                {
                    new OrderProvider().AsyncOrderStatus(orderNo);
                }
            });

            return result;
        }


        /// <summary>
        /// 平扬 2015.3.30
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ResultModel<ClientOrderNoLoginResultModel[]> GetJobListNoLogin_C(ClientOrderSearchCriteria model)
        {
            try
            {
                var pageinfo = clienterDao.GetOrdersNoLogin(model);
                if (pageinfo == null)
                    return ResultModel<ClientOrderNoLoginResultModel[]>.Conclude(GetOrdersNoLoginStatus.FailedGetOrders);

                IList<order> list = pageinfo.Records;
                IList<ClientOrderNoLoginResultModel> listOrder = new List<ClientOrderNoLoginResultModel>(); //组装成新的对象
                foreach (order item in list)
                {
                    var resultModel = new ClientOrderNoLoginResultModel();
                    if (item.clienterId != null)
                        resultModel.userId = item.clienterId.Value;
                    resultModel.OrderNo = item.OrderNo;

                    #region 骑士佣金计算

                    var oCommission = new OrderCommission()
                    {
                        Amount = item.Amount,
                        DistribSubsidy = item.DistribSubsidy,
                        OrderCount = item.OrderCount
                    };

                    #endregion

                    resultModel.income = item.OrderCommission; //佣金 Edit bycaoheyang 20150327
                    resultModel.Amount = DefaultOrPriceProvider.GetCurrenOrderPrice(oCommission);
                    //C端 获取订单的金额 Edit bycaoheyang 20150305

                    resultModel.businessName = item.BusinessName;
                    resultModel.businessPhone = item.BusinessPhone;
                    resultModel.businessPhone2 = item.BusinessPhone2;
                    resultModel.pickUpCity = item.PickUpCity;
                    resultModel.pubDate = item.PubDate.ToString();

                    resultModel.pickUpAddress = item.PickUpAddress;
                    resultModel.receviceName = item.ReceviceName;
                    resultModel.receviceCity = item.ReceviceCity;
                    resultModel.receviceAddress = item.ReceviceAddress;
                    resultModel.recevicePhone = item.RecevicePhoneNo;
                    resultModel.IsPay = item.IsPay ?? false;
                    resultModel.Remark = item.Remark;
                    resultModel.Status = item.Status;
                    resultModel.OrderCount = item.OrderCount;

                    #region 计算经纬度     待封装  add by caoheyang 20150313

                    if (item.BusiLongitude == null || item.BusiLongitude == 0 || item.BusiLatitude == null ||
                        item.BusiLatitude == 0)
                    {
                        resultModel.distance = "--";
                        resultModel.distanceB2R = "--";
                    }
                    else
                    {
                        if (degree.longitude == 0 || degree.latitude == 0 || item.businessId <= 0)
                            resultModel.distance = "--";
                        else if (item.businessId > 0) //计算超人当前到商户的距离
                        {
                            Degree degree1 = new Degree(degree.longitude, degree.latitude); //超人当前的经纬度
                            Degree degree2 = new Degree(item.BusiLongitude.Value, item.BusiLatitude.Value); //商户经纬度
                            var res = ParseHelper.ToDouble(CoordDispose.GetDistanceGoogle(degree1, degree2));
                            resultModel.distance = res < 1000 ? (Math.Round(res).ToString() + "米") : ((res / 1000).ToString("f2") + "公里");
                        }
                        if (item.businessId > 0 && item.ReceviceLongitude != null && item.ReceviceLatitude != null
                            && item.ReceviceLongitude != 0 && item.ReceviceLatitude != 0) //计算商户到收货人的距离
                        {
                            Degree degree1 = new Degree(item.BusiLongitude.Value, item.BusiLatitude.Value); //商户经纬度
                            Degree degree2 = new Degree(item.ReceviceLongitude.Value, item.ReceviceLatitude.Value);
                            //收货人经纬度
                            var res = ParseHelper.ToDouble(CoordDispose.GetDistanceGoogle(degree1, degree2));
                            resultModel.distance = res < 1000 ? (Math.Round(res).ToString() + "米") : ((res / 1000).ToString("f2") + "公里");
                        }
                        else
                            resultModel.distanceB2R = "--";
                    }

                    #endregion

                    listOrder.Add(resultModel);
                }

                if (!model.isLatest) //不是最新任务，按距离排序
                {
                    listOrder.OrderBy(o => o.distance);
                }
                return ResultModel<ClientOrderNoLoginResultModel[]>.Conclude(GetOrdersNoLoginStatus.Success,
                    listOrder.ToArray());
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return ResultModel<ClientOrderNoLoginResultModel[]>.Conclude(GetOrdersNoLoginStatus.FailedGetOrders);
        }

        /// <summary>
        /// 根据骑士Id判断骑士是否存在
        /// danny-20150530
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool CheckClienterExistById(int Id)
        {
            return clienterDao.CheckClienterExistById(Id);
        }
        /// <summary>
        /// 更新骑士照片信息
        /// danny-10150330
        /// </summary>
        /// <param name="clienter"></param>
        public bool UpdateClientPicInfo(ClienterModel clienter)
        {
            ETS.NoSql.RedisCache.RedisCache redis = new ETS.NoSql.RedisCache.RedisCache();
            string cacheKey = string.Format(RedissCacheKey.ClienterProvider_GetUserStatus, clienter.Id);
            redis.Delete(cacheKey);
            return clienterDao.UpdateClientPicInfo(clienter);
        }

        /// <summary>
        /// 根据电话获取当前用户的信息
        /// danny-20150330
        /// </summary>
        /// <param name="PhoneNo"></param>
        /// <returns></returns>
        public ClienterModel GetUserInfoByUserPhoneNo(string PhoneNo)
        {
            return clienterDao.GetUserInfoByUserPhoneNo(PhoneNo);
        }
        /// <summary>
        /// 根据用户ID更新密码
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <param name="UserPwd">新密码</param>
        /// <returns></returns>
        public bool UpdateClienterPwdByUserId(int UserId, string UserPwd)
        {
            return clienterDao.UpdateClienterPwdSql(UserId, UserPwd);
        }

        /// <summary>
        /// 获取用户状态
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public ClienterStatusModel GetUserStatus(int UserId)
        {
            try
            {
                //ETS.NoSql.RedisCache.RedisCache redis = new ETS.NoSql.RedisCache.RedisCache();
                //string cacheKey = string.Format(RedissCacheKey.ClienterProvider_GetUserStatus, UserId);
                //var cacheValue = redis.Get<string>(cacheKey);
                //if (!string.IsNullOrEmpty(cacheValue))
                //{
                //    return Letao.Util.JsonHelper.ToObject<ClienterStatusModel>(cacheValue);
                //}
                var UserInfo = clienterDao.GetUserStatus(UserId);
                if (UserInfo != null && UserInfo.IsBind == 1)
                {
                    UserInfo.IsOnlyShowBussinessTask = IsOnlyShowBussinessTask(UserId);
                }
                //if (UserInfo != null)
                //{
                //    redis.Add(cacheKey, Letao.Util.JsonHelper.ToJson(UserInfo));
                //}
                return UserInfo;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return null;
        }
        /// <summary>
        /// 判断给定骑士是否只显示雇主任务
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private int IsOnlyShowBussinessTask(int userId)
        {
            try
            {
                bool isSetting = clienterDao.IsSettingOnlyShowBussinessTask(userId);
                if (isSetting)
                {
                    GlobalConfigModel globalSetting = new GlobalConfigProvider().GlobalConfigMethod(0);
                    if (globalSetting != null && !string.IsNullOrEmpty(globalSetting.EmployerTaskTimeSet))
                    {
                        string[] settings = globalSetting.EmployerTaskTimeSet.Split('-');
                        string date = DateTime.Now.ToString("yyyy-MM-dd");
                        DateTime beginDate = DateTime.Parse(date + " " + settings[0]);
                        DateTime endDate = DateTime.Parse(date + " " + settings[1]);
                        DateTime currentDate = DateTime.Now;
                        if (beginDate <= currentDate && currentDate <= endDate)
                        {
                            return 1;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return 0;
        }
        /// <summary>
        /// 超人完成订单  
        /// </summary>
        /// <param name="userId">超人id</param>
        /// <param name="orderNo">订单号码</param>
        /// <param name="pickupCode">取货码</param>
        /// <returns></returns>
        public FinishOrderResultModel FinishOrder(OrderCompleteModel parModel)
        {
            string orderNo = parModel.orderNo;
            string pickupCode = parModel.pickupCode;
            #region 验证
            FinishOrderResultModel model = new FinishOrderResultModel() { };

            OrderListModel myOrderInfo = orderDao.GetByOrderNo(orderNo);
            if (myOrderInfo == null)  //关联表用的join,数据不完整关联表时会查不到数据
            {
                model.FinishOrderStatus = FinishOrderStatus.DataError;
                return model;
            }
            //获取该订单信息和该  骑士现在的 收入金额
            if (myOrderInfo.GroupId == GroupType.Group3.GetHashCode() && !string.IsNullOrWhiteSpace(myOrderInfo.PickupCode)
                && pickupCode != myOrderInfo.PickupCode) //全时订单 判断 取货码是否正确             
            {
                model.FinishOrderStatus = FinishOrderStatus.PickupCodeError;
                return model;
            }

            GlobalConfigModel globalSetting = new GlobalConfigProvider().GlobalConfigMethod(0);
            int limitFinish = ParseHelper.ToInt(globalSetting.CompleteTimeSet, 5);
            //取到任务的接单时间、从缓存中读取完成任务时间限制，判断要用户点击完成时间>接单时间+限制时间           
            DateTime yuJiFinish = myOrderInfo.GrabTime.Value.AddMinutes(limitFinish);
            if (DateTime.Compare(DateTime.Now, yuJiFinish) < 0)  //小于0说明用户完成时间 太快
            {
                model.FinishOrderStatus = FinishOrderStatus.TooQuickly;
                return model;
            }
            if (!new OrderDao().IsOrNotFinish(myOrderInfo.Id))//是否有未完成子订单
            {
                model.FinishOrderStatus = FinishOrderStatus.ExistNotPayChildOrder;
                return model;
            }
            #endregion

            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                //更新订单状态                
                var upresult = orderDao.FinishOrderStatus(myOrderInfo);
                if (upresult <= 0)
                {
                    model.FinishOrderStatus = FinishOrderStatus.OrderHadCancel;
                    return model;
                }

                //写入骑士完成坐标                 
                orderOtherDao.UpdateComplete(parModel);
                string mess = "更新坐标:" + parModel.orderId;               
                LogHelper.LogWriter(" FinishOrder", new { obj = "时间：" + DateTime.Now.ToString() + mess });

                //更新骑士和商家金额
                bool blUpdate = UpdateMoney(myOrderInfo);
                mess = "更新骑士和商家金额订单Id:" + myOrderInfo.Id;
                mess += "列新状态:" + blUpdate.ToString();
                LogHelper.LogWriter(" FinishOrder", new { obj = "时间：" + DateTime.Now.ToString() + mess });
                if (!blUpdate)
                {
                    return model;
                }
                tran.Complete();
            }
            //异步回调第三方，推送通知
            Task.Factory.StartNew(() =>
            {
                if (myOrderInfo.businessId != 0 )
                {
                    Push.PushMessage(1, "订单提醒", "有订单完成了！", "有超人完成了订单！", myOrderInfo.businessId.ToString(), string.Empty);
                    new OrderProvider().AsyncOrderStatus(orderNo);
                }
            });

            model.IsModifyTicket = myOrderInfo.HadUploadCount >= myOrderInfo.OrderCount ? true : false;//是否允许修改小票     
            model.FinishOrderStatus = FinishOrderStatus.Success;
            return model;

            #region 临时
            ////int userId=parModel.userId;
            //string orderNo= parModel.orderNo;
            //float completeLongitude=parModel.Longitude; 
            //float CompleteLatitude=parModel.Latitude; 
            //string pickupCode = parModel.pickupCode;
            //FinishOrderResultModel model = new FinishOrderResultModel() {};

            //OrderListModel myOrderInfo = orderDao.GetByOrderNo(orderNo);
            //if (myOrderInfo == null)  //关联表用的join,数据不完整关联表时会查不到数据
            //{
            //    //model.Message = "500";
            //    model.FinishOrderStatus = FinishOrderStatus.DataError;
            //    return model;
            //}

            //GlobalConfigModel globalSetting = new GlobalConfigProvider().GlobalConfigMethod(0);
            //int limitFinish = ParseHelper.ToInt(globalSetting.CompleteTimeSet, 5);
            ////取到任务的接单时间、从缓存中读取完成任务时间限制，判断要用户点击完成时间>接单时间+限制时间           
            //DateTime yuJiFinish = myOrderInfo.GrabTime.Value.AddMinutes(limitFinish);
            //if (DateTime.Compare(DateTime.Now, yuJiFinish) < 0)  //小于0说明用户完成时间 太快
            //{
            //    //model.Message = "501";
            //    model.FinishOrderStatus = FinishOrderStatus.TooQuickly;
            //    return model;
            //}
            //if (!new OrderDao().IsOrNotFinish(myOrderInfo.Id))//是否有未完成子订单
            //{
            //    //model.Message = "502";
            //    model.FinishOrderStatus = FinishOrderStatus.ExistNotPayChildOrder;
            //    return model;
            //}

            ////获取该订单信息和该  骑士现在的 收入金额
            //if (myOrderInfo.GroupId == SystemConst.Group3 && !string.IsNullOrWhiteSpace(myOrderInfo.PickupCode)
            //    && pickupCode != myOrderInfo.PickupCode) //全时订单 判断 取货码是否正确             
            //{
            //    //model.Message = FinishOrderStatus.PickupCodeError.ToString();
            //    model.FinishOrderStatus = FinishOrderStatus.PickupCodeError;
            //    return model;
            //}

            //using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            //{               
            //    //更新订单状态                
            //    var upresult = orderDao.FinishOrderStatus(myOrderInfo);
            //    if (upresult <= 0)
            //    {
            //        //model.Message = "3";
            //        model.FinishOrderStatus = FinishOrderStatus.OrderHadCancel;
            //        return model;
            //    }

            //     //写入骑士完成坐标                 
            //    //orderOtherDao.UpdateComplete(orderNo, completeLongitude, CompleteLatitude);
            //    orderOtherDao.UpdateComplete(parModel);

            //    //更新骑士和商家金额
            //    UpdateMoney(myOrderInfo);                

            //    tran.Complete();          

            //}
            ////异步回调第三方，推送通知
            //Task.Factory.StartNew(() =>
            //{
            //    if (myOrderInfo.businessId != 0 && model.Message == "1")
            //    {
            //        Push.PushMessage(1, "订单提醒", "有订单完成了！", "有超人完成了订单！", myOrderInfo.businessId.ToString(), string.Empty);
            //        new OrderProvider().AsyncOrderStatus(orderNo);
            //    }
            //});

            //model.IsModifyTicket = myOrderInfo.HadUploadCount >= myOrderInfo.OrderCount ? true : false;//是否允许修改小票     
            ////model.Message = "1";
            //model.FinishOrderStatus = FinishOrderStatus.Success;
            //return model;
            #endregion
        }

        /// <summary>
        /// 更新用户金额
        /// wc 完成订单时候调用
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="myOrderInfo"></param>
        public void UpdateClienterAccount(OrderListModel myOrderInfo)
        {
            //更新骑士 金额  
            bool b = clienterDao.UpdateClienterAccountBalanceForFinish(new WithdrawRecordsModel() { UserId = myOrderInfo.clienterId, Amount = myOrderInfo.OrderCommission.Value });
            //增加记录 
            decimal? accountBalance = 0;
            //更新用户相关金额数据 
            if (myOrderInfo.AccountBalance.HasValue)
            {
                accountBalance = myOrderInfo.AccountBalance.Value + (myOrderInfo.OrderCommission == null ? 0 : Convert.ToDecimal(myOrderInfo.OrderCommission));
            }
            else
            {
                accountBalance = myOrderInfo.OrderCommission == null ? 0 : Convert.ToDecimal(myOrderInfo.OrderCommission);
            }

            ///TODO 骑士余额流水表，不是这个吧？
            ClienterBalanceRecord cbrm = new ClienterBalanceRecord()
            {
                ClienterId = myOrderInfo.clienterId,
                Amount = myOrderInfo.OrderCommission == null ? 0 : Convert.ToDecimal(myOrderInfo.OrderCommission),
                Status = ClienterBalanceRecordStatus.Success.GetHashCode(),
                Balance = accountBalance ?? 0,
                RecordType = ClienterBalanceRecordRecordType.OrderCommission.GetHashCode(),
                Operator = string.IsNullOrEmpty(myOrderInfo.ClienterName) ? "骑士" : myOrderInfo.ClienterName,
                WithwardId = myOrderInfo.Id,
                RelationNo = myOrderInfo.OrderNo,
                Remark = "骑士完成订单"
            };
            clienterBalanceRecordDao.Insert(cbrm);
        }


        /// <summary>
        /// 更新用户金额      
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="myOrderInfo"></param>
        public void UpdateAccountBalanceAndWithdraw(int userId, OrderListModel myOrderInfo)
        {
            //更新骑士 金额  
            bool b = clienterDao.UpdateAccountBalanceAndWithdraw(new WithdrawRecordsModel() { UserId = userId, Amount = myOrderInfo.OrderCommission.Value });
            //增加记录 
            decimal? accountBalance = 0;
            //更新用户相关金额数据 
            if (myOrderInfo.AccountBalance.HasValue)
            {
                accountBalance = myOrderInfo.AccountBalance.Value + (myOrderInfo.OrderCommission == null ? 0 : Convert.ToDecimal(myOrderInfo.OrderCommission));
            }
            else
            {
                accountBalance = myOrderInfo.OrderCommission == null ? 0 : Convert.ToDecimal(myOrderInfo.OrderCommission);
            }

            ClienterBalanceRecord cbrm = new ClienterBalanceRecord()
            {
                ClienterId = userId,
                Amount = myOrderInfo.OrderCommission == null ? 0 : Convert.ToDecimal(myOrderInfo.OrderCommission),
                Status = ClienterBalanceRecordStatus.Success.GetHashCode(),
                Balance = accountBalance ?? 0,
                RecordType = ClienterBalanceRecordRecordType.OrderCommission.GetHashCode(),
                Operator = string.IsNullOrEmpty(myOrderInfo.ClienterName) ? "骑士:" + userId : myOrderInfo.ClienterName,
                RelationNo = myOrderInfo.OrderNo,
                Remark = "骑士完成订单"
            };
            clienterBalanceRecordDao.Insert(cbrm);
        }


        public ClienterModel GetUserInfoByUserId(int UserId)
        {
            return clienterDao.GetUserInfoByUserId(UserId);
        }
        /// <summary>
        /// 骑士配送统计
        /// danny-20150408
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<BusinessesDistributionModel> GetClienterDistributionStatisticalInfo(OrderSearchCriteria criteria)
        {
            PageInfo<BusinessesDistributionModel> pageinfo = clienterDao.GetClienterDistributionStatisticalInfo<BusinessesDistributionModel>(criteria);
            return pageinfo;
        }
        /// <summary>
        /// 骑士门店抢单统计
        /// danny-20150408
        /// </summary>
        /// <returns></returns>
        public IList<BusinessesDistributionModel> GetClienteStorerGrabStatisticalInfo()
        {
            return clienterDao.GetClienteStorerGrabStatisticalInfo();
        }

        /// <summary>
        /// 骑士门店抢单统计
        /// 胡灵波-20150424
        /// </summary>
        /// <param name="daysAgo">几天前</param>
        /// <returns></returns>
        public IList<BusinessesDistributionModel> GetClienteStorerGrabStatisticalInfo(int daysAgo)
        {
            return clienterDao.GetClienteStorerGrabStatisticalInfo(daysAgo);
        }
        /// <summary>
        /// 骑士门店抢单统计
        /// danny-20150408,一个月后删除，窦海超，2015年4月17日 19:10:30
        /// </summary>
        /// <returns></returns>
        public IList<BusinessesDistributionModelOld> GetClienteStorerGrabStatisticalInfoOld(int NewCount)
        {
            return clienterDao.GetClienteStorerGrabStatisticalInfoOld(NewCount);
        }
        /// <summary>
        /// 上传小票
        /// wc
        /// </summary>
        /// <param name="uploadReceiptModel"></param>
        /// <returns></returns>
        public OrderOther UpdateClientReceiptPicInfo(UploadReceiptModel uploadReceiptModel)
        {
            OrderOther orderOther = null;
            var myOrderInfo = orderDao.GetByOrderId(uploadReceiptModel.OrderId);
            ///TODO 事务里有多个库的连接串
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                bool HasUploadTicket = orderChildDao.GetHasUploadTicket(uploadReceiptModel.OrderId, uploadReceiptModel.OrderChildId);
                if (HasUploadTicket)
                {
                    uploadReceiptModel.HadUploadCount = 0;
                }
                else
                {
                    uploadReceiptModel.HadUploadCount = 1;
                }


                orderOther = clienterDao.UpdateClientReceiptPicInfo(uploadReceiptModel);
                if (orderOther == null) return null;

                //上传成功后， 判断 订单 创建时间在 2015-4-18 00：00 之前的订单不在增加佣金
                string date = "2015-04-18 00:00:00";

                //更新骑士金额

                #region 是否给骑士加佣金，如果当前时间大于等于 上传小票的时间+24小时，就不增加佣金 && 把订单加入到已增加已提现里
                DateTime doneDate = ParseHelper.ToDatetime(myOrderInfo.ActualDoneDate, DateTime.Now).AddDays(1);//完成时间加一天
                bool IsPayOrderCommission = true;
                if (myOrderInfo.ActualDoneDate != null && DateTime.Now >= doneDate)
                {
                    IsPayOrderCommission = false;
                    orderDao.UpdateJoinWithdraw(myOrderInfo.Id);//把订单加入到已增加可提现里

                    if (orderOther.HadUploadCount >= orderOther.NeedUploadCount && orderOther.OrderStatus ==OrderStatus.Status1.GetHashCode())
                    {
                        CheckOrderPay(myOrderInfo.Id);
                    }
                }
                #endregion

                if (IsPayOrderCommission && orderOther.OrderCreateTime > Convert.ToDateTime(date)
                   && orderOther.OrderStatus == OrderStatus.Status1.GetHashCode())
                {
                    UpdateClienterMoney(myOrderInfo, uploadReceiptModel, orderOther);
                }

                tran.Complete();

                #region 是否允许修改小票
                orderOther.IsModifyTicket = true;
                if (orderOther.HadUploadCount >= orderOther.NeedUploadCount && myOrderInfo.Status == OrderStatus.Status1.GetHashCode())
                {
                    orderOther.IsModifyTicket = false;
                }
                #endregion
            }
            return orderOther;
        }

        #region 更改上传小票、完成任务余额

        /// <summary>
        /// 更新上传小票余额
        /// 2015年7月1日 11:24:08
        /// 胡灵波
        /// </summary>
        /// <param name="myOrderInfo"></param>
        /// <param name="uploadReceiptModel"></param>
        /// <param name="orderOther"></param>
        void UpdateClienterMoney(OrderListModel myOrderInfo, UploadReceiptModel uploadReceiptModel, OrderOther orderOther)
        {
            string mess = "付款方式:" + myOrderInfo.IsPay;
            mess += " 订单编号:" + myOrderInfo.OrderNo;
            mess += " HadUploadCount:" + myOrderInfo.HadUploadCount;
            mess += " NeedUploadCount:" + myOrderInfo.NeedUploadCount;
            LogHelper.LogWriter(" UpdateClienterMoney", new { obj = "时间：" + DateTime.Now.ToString() + mess });

            if ((bool)myOrderInfo.IsPay)//已付款
            {
                //上传完小票
                //(1)更新给骑士余额
                if (orderOther.HadUploadCount >= orderOther.NeedUploadCount)
                {
                    if (CheckOrderPay(myOrderInfo.Id))
                    {
                        UpdateClienterAccount(myOrderInfo);
                    }
                }
            }
            else if (!(bool)myOrderInfo.IsPay && myOrderInfo.MealsSettleMode == MealsSettleMode.Status0.GetHashCode())//未付款,骑士代付
            {
                //上传完小票
                //(1)更新给骑士余额
                if (orderOther.HadUploadCount >= orderOther.NeedUploadCount)
                {
                    if (CheckOrderPay(myOrderInfo.Id))
                    {
                        UpdateClienterAccount(myOrderInfo);
                    }
                }
            }
            else if (!(bool)myOrderInfo.IsPay && myOrderInfo.MealsSettleMode == MealsSettleMode.Status1.GetHashCode())//未付款,线上结算
            {
                //上传完小票
                //(1)更新给骑士余额、可提现余额
                //(2)把OrderOther把IsJoinWithdraw状态改为1
                if (orderOther.HadUploadCount >= orderOther.NeedUploadCount)
                {
                    if (CheckOrderPay(myOrderInfo.Id))
                    {
                        UpdateAccountBalanceAndWithdraw(uploadReceiptModel.ClienterId, myOrderInfo);
                        iOrderOtherProvider.UpdateIsJoinWithdraw(myOrderInfo.Id);
                    }
                }
            }
        }


        /// <summary>
        /// 更新完成订单余额
        /// 2015年7月1日 11:24:08
        /// 胡灵波
        /// </summary>
        /// <param name="myOrderInfo"></param>
        /// <param name="uploadReceiptModel"></param>
        /// <param name="orderOther"></param>
        bool UpdateMoney(OrderListModel myOrderInfo)
        {
            string mess = "付款方式:" + myOrderInfo.IsPay;
            mess += " 订单编ID:" + myOrderInfo.Id;
            mess += " HadUploadCount:" + myOrderInfo.HadUploadCount;
            mess += " OrderCount:" + myOrderInfo.OrderCount;
            LogHelper.LogWriter(" UpdateMoney", new { obj = "时间：" + DateTime.Now.ToString() + mess });

            if (ParseHelper.ToBool(myOrderInfo.IsPay, false))//已付款
            {
                //上传完小票
                //(1)更新给骑士余额
                if (myOrderInfo.HadUploadCount >= myOrderInfo.OrderCount)
                {
                    if (CheckOrderPay(myOrderInfo.Id))
                    {
                        UpdateClienterAccount(myOrderInfo);
                    }
                }
                return true;
            }
            else if (!(bool)myOrderInfo.IsPay && myOrderInfo.MealsSettleMode == MealsSettleMode.Status0.GetHashCode())//未付款,骑士代付
            {
                //上传完小票
                //(1)更新给骑士余额
                if (myOrderInfo.HadUploadCount >= myOrderInfo.OrderCount)
                {
                    if (CheckOrderPay(myOrderInfo.Id))
                    {
                        UpdateClienterAccount(myOrderInfo);
                    }
                }
                return true;
            }
            else if (!(bool)myOrderInfo.IsPay && myOrderInfo.MealsSettleMode == MealsSettleMode.Status1.GetHashCode())//未付款,线上结算
            {
                //返还商户金额
                businessDao.UpdateForWithdrawC(new UpdateForWithdrawPM()
                {
                    Id = Convert.ToInt32(myOrderInfo.businessId),
                    Money = myOrderInfo.BusinessReceivable
                });

                #region 商户余额流水操作
                businessBalanceRecordDao.Insert(new BusinessBalanceRecord()
                {
                    BusinessId = Convert.ToInt32(myOrderInfo.businessId),
                    Amount = myOrderInfo.BusinessReceivable,
                    Status = (int)BusinessBalanceRecordStatus.Success, //流水状态(1、交易成功 2、交易中）
                    RecordType = (int)BusinessBalanceRecordRecordType.OrderMeals,
                    Operator = myOrderInfo.ClienterName,
                    Remark = "返还商家订单菜品费",
                    WithwardId = myOrderInfo.Id,
                    RelationNo = myOrderInfo.OrderNo
                });
                #endregion

                //上传完小票
                //(1)更新给骑士余额、可提现余额
                //(2)把OrderOther把IsJoinWithdraw状态改为1
                if (myOrderInfo.HadUploadCount >= myOrderInfo.OrderCount)
                {
                    if (CheckOrderPay(myOrderInfo.Id))
                    {
                        UpdateAccountBalanceAndWithdraw(myOrderInfo.clienterId, myOrderInfo);
                        iOrderOtherProvider.UpdateIsJoinWithdraw(myOrderInfo.Id);
                    }
                }
                return true;
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 验证该订单是否支付
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>2015061</UpdateTime>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private bool CheckOrderPay(int orderId)
        {
            return orderDao.UpdateFinishAll(orderId);
        }

        /// <summary>
        /// 删除小票
        /// wc
        /// </summary>
        /// <param name="uploadReceiptModel"></param>
        /// <returns></returns>
        public OrderOther DeleteReceipt(UploadReceiptModel uploadReceiptModel)
        {
            var orderOther = clienterDao.DeleteReceipt(uploadReceiptModel);

            return orderOther;
        }
        /// <summary>
        /// 新增小票信息
        /// wc
        /// </summary>
        /// <param name="uploadReceiptModel"></param>
        /// <returns></returns>
        public OrderOther InsertReceiptInfo(UploadReceiptModel uploadReceiptModel)
        {
            return clienterDao.InsertReceiptInfo(uploadReceiptModel);
        }      

        /// <summary>
        /// 根据订单Id获取小票信息
        /// wc
        /// </summary>
        /// <param name="uploadReceiptModel"></param>
        /// <returns></returns>
        public OrderOther GetReceipt(int orderId)
        {
            return clienterDao.GetReceiptInfo(orderId);

        }

        /// <summary>
        /// 根据订单id获取订单信息 和 小票相关
        /// 王超
        /// 2015年5月6日 20:40:05
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public order GetOrderInfoByOrderId(int orderId)
        {
            return orderDao.GetOrderInfoByOrderId(orderId);
        }

        /// <summary>
        ///  C端抢单
        ///  窦海超
        ///  2015年5月6日 20:40:56
        /// </summary>
        /// <param name="userId">骑士ID</param>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>     
        public ResultModel<RushOrderResultModel> RushOrder_C(int userId, string orderNo)
        {
            if (string.IsNullOrEmpty(orderNo)) //订单号码非空验证
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.OrderEmpty);

            ClienterModel clienterModel = new Ets.Dao.Clienter.ClienterDao().GetUserInfoByUserId(userId);
            if (userId == 0 || clienterModel == null) //用户id验证
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.userIdEmpty);
            if (clienterModel.Status != 1)  //判断 该骑士 是否 有资格 抢单 wc
            {
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.HadCancelQualification);
            }

            var myorder = new Ets.Dao.Order.OrderDao().GetOrderDetailByOrderNo(orderNo);
            if (myorder == null)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.RushOrderResultModel>.Conclude(RushOrderStatus.OrderIsNotExist);  //订单不存在

            }
            if (myorder.Status == OrderStatus.Status3.GetHashCode())   //判断订单状态是否为 已取消
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.RushOrderResultModel>.Conclude(RushOrderStatus.OrderHadCancel);  //订单已被取消
            }
            if (myorder.Status == OrderStatus.Status2.GetHashCode() || myorder.Status == OrderStatus.Status1.GetHashCode())  //订单已接单，被抢  或 已完成
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.RushOrderResultModel>.Conclude(RushOrderStatus.OrderIsNotAllowRush);
            }
            OrderListModel model = new OrderListModel()
            {
                clienterId = userId,
                OrderNo = orderNo
            };
            bool bResult = orderDao.RushOrder(model);
            if (bResult)
            {
                Task.Factory.StartNew(() =>
                {
                    new OrderProvider().AsyncOrderStatus(orderNo);//同步第三方订单
                    Push.PushMessage(1, "订单提醒", "有订单被抢了！", "有超人抢了订单！", myorder.businessId.ToString(), string.Empty);
                });

                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.Success);
            }

            return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.Failed);
        }

        /// <summary>
        /// 获取骑士详情
        /// hulingbo 20150511
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="id">骑士id</param>
        /// <returns></returns>
        public ClienterDM GetDetails(int id)
        {
            ClienterDM model = clienterDao.GetDetails(id);
            model.HasMessage = new ClienterMessageDao().HasMessage(id);
            return model;
        }

        /// <summary>
        /// 判断骑士是否存在        
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="id">骑士Id</param>
        /// <returns></returns>
        public bool IsExist(int id)
        {
            return clienterDao.IsExist(id);
        }

        /// <summary>
        /// 根据订单Id和子订单Id获取子订单信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderChildId"></param>
        /// <returns></returns>
        public List<OrderChildForTicket> GetOrderChildInfo(int orderId, int orderChildId)
        {
            return orderChildDao.GetOrderChildInfo(orderId, orderChildId);
        }
        /// <summary>
        /// 获取骑士详细信息
        /// danny-20150513
        /// </summary>
        /// <param name="clienterId">骑士Id</param>
        /// <returns></returns>
        public ClienterDetailModel GetClienterDetailById(string clienterId)
        {
            return clienterDao.GetClienterDetailById(clienterId);
        }


        /// <summary>
        ///  C端抢单
        ///  wc
        ///  2015年5月6日 20:40:56
        /// </summary>
        /// <param name="userId">骑士ID</param>
        /// <param name="orderNo">订单号</param>
        /// <param name="bussinessId"></param>
        /// <returns></returns>       
        public ResultModel<RushOrderResultModel> Receive_C(int userId, string orderNo, int bussinessId, float grabLongitude, float grabLatitude)
        {
            //这里可以优化，去掉提前验证用户信息，当失败的时候在去验证 
            OrderListModel model = new OrderListModel()
            {
                clienterId = userId,
                OrderNo = orderNo
            };
            ///TODO 骑士是否有资格抢单放前面
            ClienterModel clienterModel = new Ets.Dao.Clienter.ClienterDao().GetUserInfoByUserId(userId);

            if (clienterModel.Status != 1)  //判断 该骑士 是否 有资格 抢单 wc
            {
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.HadCancelQualification);
            }
            bool bResult = orderDao.RushOrder(model);
            ///TODO 同步第三方状态和jpush 以后放到后台服务或mq进行。
            if (bResult)
            {
                //异步回调第三方，推送通知
                Task.Factory.StartNew(() =>
                {
                    //写入骑士抢单坐标
                    orderOtherDao.UpdateGrab(orderNo, grabLongitude, grabLatitude);
                    new OrderProvider().AsyncOrderStatus(orderNo);//同步第三方订单
                    Push.PushMessage(1, "订单提醒", "有订单被抢了！", "有超人抢了订单！", bussinessId.ToString(), string.Empty);
                });

                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.Success);
            }

            return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.OrderIsNotAllowRush);
        }

        /// <summary>
        /// 获取骑士用户名
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public string GetName(string phoneNo)
        {
            return clienterDao.GetName(phoneNo);
        }
        /// <summary>
        /// 获取骑士列表
        /// danny-20150608
        /// </summary>
        /// <param name="model"></param>
        public IList<ClienterListModel> GetClienterList(ClienterListModel model)
        {
            return clienterDao.GetClienterList(model);
        }
        /// <summary>
        /// 获取骑士Id
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150609</UpdateTime>
        public int GetId(string phoneNo, string trueName)
        {
            return clienterDao.GetId(phoneNo, trueName);
        }
        /// <summary>
        /// 查询骑士列表
        /// danny-20150609
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<ClienterListModel> GetClienterList(ClienterSearchCriteria criteria)
        {
            return clienterDao.GetClienterList<ClienterListModel>(criteria);
        }
    }

}
