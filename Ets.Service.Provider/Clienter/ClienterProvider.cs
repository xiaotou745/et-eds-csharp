﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Const;
using Ets.Dao.Clienter;
using Ets.Dao.Message;
using Ets.Dao.Tag;
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
using Ets.Model.DataModel.Business;
using Ets.Service.IProvider.DeliveryCompany;
using Ets.Service.Provider.DeliveryCompany;
using Ets.Model.DataModel.DeliveryCompany;
using Ets.Service.IProvider.GlobalConfig;
using Ets.Dao.GlobalConfig;
using ETS.NoSql.RedisCache;
using Ets.Service.IProvider.Business;
using Ets.Service.Provider.Business;
using Ets.Model.ParameterModel.Common;
using ETS.Security;
using ETS;
using Ets.Dao.Authority;
using Ets.Model.DomainModel.Authority;
using ETS.Extension;

namespace Ets.Service.Provider.Clienter
{
    public class ClienterProvider : IClienterProvider
    {
        readonly ClienterDao clienterDao = new ClienterDao();
        readonly OrderDao orderDao = new OrderDao();
        readonly OrderOtherDao orderOtherDao = new OrderOtherDao();
        readonly OrderChildDao orderChildDao = new OrderChildDao();
        readonly GlobalConfigDao globalConfigDao = new GlobalConfigDao();
        readonly ClienterBalanceRecordDao clienterBalanceRecordDao = new ClienterBalanceRecordDao();
        readonly ClienterAllowWithdrawRecordDao clienterAllowWithdrawRecordDao = new ClienterAllowWithdrawRecordDao();
        readonly BusinessDao businessDao = new BusinessDao();
        readonly BusinessClienterRelationDao businessClienterDao = new BusinessClienterRelationDao();
        readonly BusinessBalanceRecordDao businessBalanceRecordDao = new BusinessBalanceRecordDao();
        readonly DeliveryCompanyProvider deliveryCompanyProvider = new DeliveryCompanyProvider();
        readonly IAreaProvider iAreaProvider = new AreaProvider();
        readonly IOrderOtherProvider iOrderOtherProvider = new OrderOtherProvider();
        readonly ITokenProvider iTokenProvider = new TokenProvider();
        private IBusinessProvider iBusinessProvider = new BusinessProvider();
        readonly ClienterLoginLogDao clienterLoginLogDao = new ClienterLoginLogDao();
        private TagRelationDao tagRelationDao = new TagRelationDao();
        private IGroupBusinessProvider iGroupBusinessProvider = new GroupBusinessProvider();
        OrderSubsidiesLogDao orderSubsidiesLogDao = new OrderSubsidiesLogDao();
        /// <summary>
        /// 骑士上下班功能 add by caoheyang 20150312
        /// </summary>
        /// <param name="paraModel"></param>
        /// <returns></returns>
        public ChangeWorkStatusEnum ChangeWorkStatus(Ets.Model.ParameterModel.Clienter.ChangeWorkStatusPM paraModel)
        {
            if (paraModel.WorkStatus == ClienteWorkStatus.WorkOff.GetHashCode())  //如果要下班，先判断超人是否还有未完成的订单
            {
                //查询当前超人有无已接单但是未完成的订单
                int ordercount = clienterDao.QueryOrderount(new Model.ParameterModel.Clienter.ChangeWorkStatusPM() { Id = paraModel.Id });
                if (ordercount > 0)
                    return ChangeWorkStatusEnum.OrderError;
            }
            int changeResult = clienterDao.ChangeWorkStatusToSql(paraModel);
            if (changeResult <= 0)
            {
                if (paraModel.WorkStatus == ClienteWorkStatus.WorkOn.GetHashCode())
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
                if (paraModel.WorkStatus == ClienteWorkStatus.WorkOn.GetHashCode())
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
                    if (item.GroupId == GroupConst.Group3) //全时 需要做验证码验证
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
                            model.distance = res < 1000 ? (Math.Round(res).ToString() + "米") : ((res / 1000).ToString("f2") + "km");
                            model.distance_OrderBy = res;
                        }
                        if (item.BusinessId > 0 && item.ReceviceLongitude != null && item.ReceviceLatitude != null
                            && item.ReceviceLongitude != 0 && item.ReceviceLatitude != 0)  //计算商户到收货人的距离
                        {
                            Degree degree1 = new Degree(item.Longitude.Value, item.Latitude.Value);  //商户经纬度
                            Degree degree2 = new Degree(item.ReceviceLongitude.Value, item.ReceviceLatitude.Value);  //收货人经纬度
                            var res = ParseHelper.ToDouble(CoordDispose.GetDistanceGoogle(degree1, degree2));
                            model.distanceB2R = res < 1000 ? (Math.Round(res).ToString() + "米") : ((res / 1000).ToString("f2") + "km");
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
        public ResultModel<ClienterLoginResultModel> PostLogin_C(ParamModel parModel)
        {
            try
            {
                LoginCPM model = JsonHelper.JsonConvertToObject<LoginCPM>(AESApp.AesDecrypt(parModel.data));
                var redis = new RedisCache();
                string key = string.Concat(RedissCacheKey.LoginCount_C, model.phoneNo);
                int excuteCount = redis.Get<int>(key);
                if (excuteCount >= 10)
                {
                    return ResultModel<ClienterLoginResultModel>.Conclude(LoginModelStatus.CountError);
                }
                redis.Set(key, excuteCount + 1, new TimeSpan(0, 5, 0));

                ClienterLoginResultModel resultModel = clienterDao.PostLogin_CSql(model);
                if (resultModel == null)
                {
                    return ResultModel<ClienterLoginResultModel>.Conclude(LoginModelStatus.InvalidCredential);
                }
                if (resultModel.DeliveryCompanyId > 0)
                {
                    resultModel.IsOnlyShowBussinessTask = 0;
                    resultModel.IsBind = 0;
                }
                else if (resultModel.IsBind == 1)
                {
                    resultModel.IsOnlyShowBussinessTask = IsOnlyShowBussinessTask(resultModel.userId);
                }

                string token = iTokenProvider.GetToken(new TokenModel()
                   {
                       Ssid = model.Ssid,
                       Appkey = resultModel.Appkey.ToString()
                   });
                resultModel.Token = token;
                //string resultStr = AESApp.AesDecrypt(JsonHelper.JsonConvertToString(resultModel));
                //记录登陆日志
                clienterLoginLogDao.Insert(new ClienterLoginLogDM
                                        {
                                            ClienterId = resultModel.userId,
                                            PhoneNo = model.phoneNo,
                                            Ssid = model.Ssid,
                                            OperSystem = model.OperSystem,
                                            OperSystemModel = model.OperSystemModel,
                                            PhoneType = model.PhoneType,
                                            AppVersion = model.AppVersion,
                                            Description = "登陆成功",
                                            IsSuccess = 1
                                        }
                    );
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
        public ResultModel<ClienterModifyPwdResultModel> PostForgetPwd_C(ParamModel parModel)//ClienterModifyPwdResultModel
        {
            ModifyPwdInfoModel model = JsonHelper.JsonConvertToObject<ModifyPwdInfoModel>(AESApp.AesDecrypt(parModel.data));
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            string key = string.Concat(RedissCacheKey.ChangePasswordCount_C, model.phoneNo);
            int excuteCount = redis.Get<int>(key);
            if (excuteCount >= 10)
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ModifyPwdStatus.CountError);
            }
            redis.Set(key, excuteCount + 1, new TimeSpan(0, 5, 0));

            if (string.IsNullOrEmpty(model.newPassword))
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ModifyPwdStatus.NewPwdEmpty);
            }
            //获取验证码
            var codekey = string.Concat(RedissCacheKey.ChangePasswordCheckCode_C, model.phoneNo);
            var codevalue = redis.Get<string>(codekey);
            if (string.IsNullOrWhiteSpace(model.checkCode) || codevalue == null || model.checkCode.ToLower() != codevalue.ToLower())
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ModifyPwdStatus.CheckCodeError);
            }
            var clienter = clienterDao.GetUserInfoByUserPhoneNo(model.phoneNo);
            if (clienter == null)
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ModifyPwdStatus.ClienterIsNotExist);
            }
            if (string.IsNullOrWhiteSpace(model.oldPassword) || clienter.Password != model.oldPassword)
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ModifyPwdStatus.OldPwdError);
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
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel<ClientRegisterResultModel> PostRegisterInfo_C(ParamModel parModel)//ClientRegisterInfoModel
        {
            ClientRegisterInfoModel model = JsonHelper.ToObject<ClientRegisterInfoModel>(AESApp.AesDecrypt(parModel.data));
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            string key = string.Concat(RedissCacheKey.RegisterCount_C, model.phoneNo);
            int excuteCount = redis.Get<int>(key);
            if (excuteCount >= 10)
            {
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.CountError);
            }
            redis.Set(key, excuteCount + 1, new TimeSpan(0, 5, 0));


            var code = redis.Get<string>(RedissCacheKey.PostRegisterInfo_C + model.phoneNo);
            if (string.IsNullOrEmpty(model.phoneNo))  //手机号非空验证
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberEmpty);
            if (clienterDao.CheckClienterExistPhone(model.phoneNo))  //判断该手机号是否已经注册过
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberRegistered);
            if (string.IsNullOrEmpty(model.passWord)) //密码非空验证
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PasswordEmpty);
            if (string.IsNullOrEmpty(code) || model.verifyCode.ToLower() != code.ToLower()) //判断验码法录入是否正确
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.IncorrectCheckCode);
            if (string.IsNullOrEmpty(model.timespan)) //判断时间戳是否为空
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.TimespanEmpty);
            var wuliuCode = string.IsNullOrWhiteSpace(model.recommendPhone) ? 0 : clienterDao.CheckRecommendPhone(model.recommendPhone);//获取物流公司编码
            model.DeliveryCompanyId = wuliuCode;

            if (!string.IsNullOrEmpty(model.recommendPhone) && (wuliuCode == -1))//如果推荐人手机号在B端C端都不存在提示信息 
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberNotExist);
            //如果已注册  add by 彭宜   20150716
            if (clienterDao.IsExist(model))
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.HasExist);
            var clienter = ClientRegisterInfoModelTranslator.Instance.Translate(model);
            //根据用户传递的  名称，取得 国标编码 wc,这里的 city 是二级 ，已和康珍 确认过
            //新版的 骑士 注册， 城市 非 必填
            if (!string.IsNullOrWhiteSpace(clienter.City))
            {
                Model.DomainModel.Area.AreaModelTranslate areaModel = iAreaProvider.GetNationalAreaInfo(new Model.DomainModel.Area.AreaModelTranslate() { Name = clienter.City.Trim(), JiBie = 3 });
                if (areaModel != null)
                {
                    clienter.CityId = areaModel.NationalCode.ToString();
                }
                else
                {
                    clienter.CityId = model.CityId;
                }
            }
            int id = clienterDao.AddClienter(clienter, model.timespan);
            var resultModel = new ClientRegisterResultModel
            {
                userId = id,
                city = string.IsNullOrWhiteSpace(clienter.City) ? null : clienter.City.Trim(),  //城市
                cityId = string.IsNullOrWhiteSpace(clienter.CityId) ? null : clienter.CityId.Trim()  //城市编码
            };
            if (wuliuCode > 0)
            {
                var deliveryModel = deliveryCompanyProvider.GetById(model.DeliveryCompanyId);
                resultModel.DeliveryCompanyId = deliveryModel.Id;
                resultModel.DeliveryCompanyName = deliveryModel.DeliveryCompanyName;
                resultModel.IsDisplay = deliveryModel.IsDisplay;
            }
            else
            {
                resultModel.DeliveryCompanyId = 0;
                resultModel.DeliveryCompanyName = "";
                resultModel.IsDisplay = 1;
            }
            string appkey = clienter.Appkey;
            string token = iTokenProvider.GetToken(new TokenModel()
            {
                Ssid = model.Ssid,
                Appkey = appkey
            });
            resultModel.Appkey = appkey;
            resultModel.Token = token;
            //string resultStr = JsonHelper.ToJson(resultModel);
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
                            resultModel.distance = res < 1000 ? (Math.Round(res).ToString() + "米") : ((res / 1000).ToString("f2") + "km");
                        }
                        if (item.businessId > 0 && item.ReceviceLongitude != null && item.ReceviceLatitude != null
                            && item.ReceviceLongitude != 0 && item.ReceviceLatitude != 0) //计算商户到收货人的距离
                        {
                            Degree degree1 = new Degree(item.BusiLongitude.Value, item.BusiLatitude.Value); //商户经纬度
                            Degree degree2 = new Degree(item.ReceviceLongitude.Value, item.ReceviceLatitude.Value);
                            //收货人经纬度
                            var res = ParseHelper.ToDouble(CoordDispose.GetDistanceGoogle(degree1, degree2));
                            resultModel.distance = res < 1000 ? (Math.Round(res).ToString() + "米") : ((res / 1000).ToString("f2") + "km");
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
                var UserInfo = clienterDao.GetUserStatus(UserId);
                if (UserInfo == null)
                {
                    return null;
                }

                if (UserInfo.DeliveryCompanyId > 0)
                {
                    UserInfo.IsOnlyShowBussinessTask = 0;
                    UserInfo.IsBind = 0;
                }
                else if (UserInfo.IsBind == 1)
                {
                    UserInfo.IsOnlyShowBussinessTask = IsOnlyShowBussinessTask(UserId);
                }

                return UserInfo;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return null;
        }
        /// <summary>
        /// 超人完成订单  
        /// 2015年8月10日11:27:18
        /// 茹化肖
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
            if (myOrderInfo.GroupId == GroupConst.Group3 && !string.IsNullOrWhiteSpace(myOrderInfo.PickupCode)
                && pickupCode != myOrderInfo.PickupCode) //全时订单 判断 取货码是否正确             
            {
                model.FinishOrderStatus = FinishOrderStatus.PickupCodeError;
                return model;
            }
            if (myOrderInfo.Platform == PlatformEnum.FlashToSendModel.GetHashCode()) // 闪送模式
            {
                //验证取货码
                if (string.IsNullOrWhiteSpace(myOrderInfo.Receivecode))
                {
                    //return ResultModel<string>.Conclude(FinishOrderStatus.ReceiveCodeIsEmpty);
                    model.FinishOrderStatus = FinishOrderStatus.ReceiveCodeIsEmpty;
                    return model;
                }
                //验证取货码是否正确
                if (myOrderInfo.Receivecode.Trim() != parModel.ReceiveCode.Trim())
                {
                    model.FinishOrderStatus = FinishOrderStatus.ReceiveCodeIsEmpty;
                    return model;
                }
            }

            GlobalConfigModel globalSetting = new GlobalConfigProvider().GlobalConfigMethod(0);
            //取到任务的接单时间、从缓存中读取完成任务时间限制，判断要用户点击完成时间>接单时间+限制时间 
            int limitFinish = ParseHelper.ToInt(globalSetting.CompleteTimeSet, 0);
            //LogHelper.LogWriter("完成时间:" + limitFinish);
            if (limitFinish > 0 && myOrderInfo.IsOrderChecked == 1)//任务需要判断的时候进来
            {
                //LogHelper.LogWriter("完成太快时间:" + limitFinish);
                DateTime yuJiFinish = myOrderInfo.GrabTime.Value.AddMinutes(limitFinish);
                if (DateTime.Compare(DateTime.Now, yuJiFinish) < 0)  //小于0说明用户完成时间 太快
                {
                    model.FinishOrderStatus = FinishOrderStatus.TooQuickly;
                    return model;
                }
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
                    model.FinishOrderStatus = FinishOrderStatus.OrderHadCancelOrComplete;
                    return model;
                }
                //更新商家金额
                UpdateBusinessMoney(myOrderInfo);
                //更新骑士金额
                UpdateClienterMoney(myOrderInfo);
                //写入骑士完成坐标                 
                orderOtherDao.UpdateComplete(parModel);
                tran.Complete();
            }
            //异步回调第三方，推送通知
            Task.Factory.StartNew(() =>
            {
                if (myOrderInfo.businessId != 0)
                {
                    Push.PushMessage(1, "订单提醒", "有订单完成了！", "有超人完成了订单！", myOrderInfo.businessId.ToString(), string.Empty);
                    new OrderProvider().AsyncOrderStatus(orderNo);
                }
            });

            model.IsModifyTicket = myOrderInfo.HadUploadCount >= myOrderInfo.OrderCount ? true : false;//是否允许修改小票     
            model.FinishOrderStatus = FinishOrderStatus.Success;
            return model;
        }

        public ClienterModel GetUserInfoByUserId(int UserId)
        {
            return clienterDao.GetUserInfoByUserId(UserId);
        }
        /// <summary>
        /// 骑士配送统计
        /// danny-20150408
        /// </summary>
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
                    myOrderInfo.HadUploadCount = myOrderInfo.HadUploadCount + 1;
                }


                orderOther = clienterDao.UpdateClientReceiptPicInfo(uploadReceiptModel);
                if (orderOther == null) return null;

                //上传成功后， 判断 订单 创建时间在 2015-4-18 00：00 之前的订单不在增加佣金
                string date = "2015-04-18 00:00:00";

                if (orderOther.OrderStatus == OrderStatus.Status1.GetHashCode() && orderOther.OrderCreateTime > Convert.ToDateTime(date))
                {
                    //更新骑士金额
                    UpdateClienterMoney(myOrderInfo);
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

            var myorder = new Ets.Dao.Order.OrderDao().GetByOrderNo(orderNo);
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
                    UpdateDeliveryCompanyOrderCommssion(myorder, userId);
                    new OrderProvider().AsyncOrderStatus(orderNo);//同步第三方订单
                    Push.PushMessage(1, "订单提醒", "有订单被抢了！", "有超人抢了订单！", myorder.businessId.ToString(), string.Empty);
                });
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.Success);
            }

            return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.Failed);
        }
        /// <summary>
        /// 计算物流公司的订单的佣金,zhaohailong
        /// </summary>
        /// <param name="orderModel"></param>
        private void UpdateDeliveryCompanyOrderCommssion(OrderListModel orderModel, int clienterId)
        {
            //此时orderModel中还没获取到clienterId，因此不能用orderModel.clienterId
            DeliveryCompanyModel companyDetail = deliveryCompanyProvider.GetDeliveryCompanyByClienterID(clienterId);
            if (companyDetail != null && companyDetail.IsEnable == 1)
            {
                DeliveryCompanyPriceProvider pro = new DeliveryCompanyPriceProvider();
                decimal orderCommission = pro.GetCurrenOrderCommission(orderModel, companyDetail);
                decimal deliveryCompanySettleMoney = pro.GetDeliveryCompanySettleMoney(orderModel, companyDetail);
                //更新订单的佣金
                orderDao.UpdateDeliveryCompanyOrderCommssion(orderModel.Id.ToString(), orderCommission, deliveryCompanySettleMoney, companyDetail.Id);

            }
        }
        /// <summary>
        /// 获取骑士详情
        /// hulingbo 20150511
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <UpdateBy>danny</UpdateBy>
        /// <UpdateTime>20150824</UpdateTime>
        /// <param name="id">骑士id</param>
        /// <returns></returns>
        public ClienterDM GetDetails(int id)
        {
            var model = clienterDao.GetDetails(id);
            model.HeadImgUrl = ImageCommon.GetUserImage(model.HeadImgUrl, ImageType.Clienter);
            model.HasMessage = new ClienterMessageDao().HasMessage(id); 
            model.VehicleId = EnumUtils.GetEnumHashCode(typeof (VehicleEnum), model.VehicleName);
              
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
        public ResultModel<RushOrderResultModel> Receive_C(OrderReceiveModel parmodel)
        {
            //int userId, string orderNo, int bussinessId, float grabLongitude, float grabLatitude
            //TODO 骑士是否有资格抢单放前面
            ClienterModel clienterModel = new Ets.Dao.Clienter.ClienterDao().GetUserInfoByUserId(parmodel.ClienterId);

            BusinessClienterRelation bcrModel= new Ets.Dao.Business.BusinessClienterRelationDao().GetDetails(new BusinessClienterRelationPM() 
                                { 
                                    BusinessId = parmodel.businessId, 
                                    ClienterId = parmodel.ClienterId 
                                }   
            );

            if (clienterModel.Status != 1)  //判断 该骑士 是否 有资格 抢单 wc
            {
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.HadCancelQualification);
            }
            //这里可以优化，去掉提前验证用户信息，当失败的时候在去验证 
            OrderListModel model = new OrderListModel()
            {
                clienterId = parmodel.ClienterId,
                ClienterTrueName = clienterModel.TrueName,
                OrderNo = parmodel.orderNo,
                DeliveryCompanyID = parmodel.DeliveryCompanyID//物流公司ID
            };
            bool bResult = orderDao.RushOrder(model);
            //TODO 同步第三方状态和jpush 以后放到后台服务或mq进行。            


            OrderListModel currOrderListModel= orderDao.GetByOrderNo(parmodel.orderNo);
            //不属于物流公司同时属于合作骑士
            if (parmodel.DeliveryCompanyID <= 0 && bcrModel != null && bcrModel.IsCooperation == 1)
            {
                if (currOrderListModel.GroupBusinessId > 0)
                {
                     //更新集团余额
                        iGroupBusinessProvider.UpdateGBalance(new GroupBusinessPM()
                        {
                            BusinessId = currOrderListModel.businessId,
                            GroupId = currOrderListModel.GroupBusinessId,
                            GroupAmount = currOrderListModel.SettleMoney,
                            Status = BusinessBalanceRecordStatus.Success.GetHashCode(),
                            RecordType = BusinessBalanceRecordRecordType.ReturnDeliveryFee.GetHashCode(),
                            Operator = currOrderListModel.GroupBusiName,
                            WithwardId = currOrderListModel.Id,
                            RelationNo = currOrderListModel.OrderNo,
                            Remark = "返还配送费支出金额(店内骑士抢单)"
                        });
                }
                else
                {
                    // 更新商户余额、可提现余额                        
                    iBusinessProvider.UpdateBBalanceAndWithdraw(new BusinessMoneyPM()
                    {
                        BusinessId = currOrderListModel.businessId,
                        Amount = currOrderListModel.SettleMoney,
                        Status = BusinessBalanceRecordStatus.Success.GetHashCode(),
                        RecordType = BusinessBalanceRecordRecordType.ReturnDeliveryFee.GetHashCode(),
                        Operator = currOrderListModel.BusinessName,
                        WithwardId = currOrderListModel.Id,
                        RelationNo = currOrderListModel.OrderNo,
                        Remark = "返还配送费支出金额(店内骑士抢单)"
                    });
                }
                if (currOrderListModel.Platform != 3)//闪送订单且骑士为店内 
                {
                    //更新订单状态 FinishAll=1
                    CheckOrderPay(currOrderListModel.Id);
                }
                //将订单标记为加入已提现
                orderOtherDao.UpdateJoinWithdraw(currOrderListModel.Id);
                //更新订单审核通过 
                orderOtherDao.UpdateAuditStatus(currOrderListModel.Id, OrderAuditStatusCommon.Through.GetHashCode(), clienterModel.TrueName);
             
            }

            if (bResult)
            {
                //异步回调第三方，推送通知
                Task.Factory.StartNew(() =>
                {
                    //更新骑士抢单记录
                    orderOtherDao.UpdateGrab(new OrderCompleteModel
                                            {
                                                orderNo = parmodel.orderNo,
                                                Longitude = parmodel.Longitude,
                                                Latitude = parmodel.Latitude,
                                                IsTimely = parmodel.IsTimely
                                            });
                    new OrderProvider().AsyncOrderStatus(parmodel.orderNo);//同步第三方订单
                    Push.PushMessage(1, "订单提醒", "有订单被抢了！", "有超人抢了订单！", parmodel.businessId.ToString(), string.Empty);
                    //调用java接口 里程计算 推单  (处理订单)  caoheyang 20160105
                    if (currOrderListModel.Platform == PlatformEnum.FlashToSendModel.GetHashCode())
                    {
                        new OrderProvider().ShanSongPushOrderForJava(currOrderListModel.Id);
                    }
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
        /// <summary>
        /// 修改骑士详细信息
        /// danny-20150707
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DealResultInfo ModifyClienterDetail(ClienterDetailModel model)
        {
            var dealResultInfo = new DealResultInfo
            {
                DealFlag = false
            };
            if (model.GradeType == (int)GradeType.Fulltime)
            {
                int internalNum = businessClienterDao.GetBusinessInternalNum(model.Id);
                if (internalNum > 0)
                {
                    dealResultInfo.DealMsg = "当前骑士已经是其他商家的店内骑士，不能改为全职骑士！";
                    return dealResultInfo;
                }
            }
            if (!string.IsNullOrWhiteSpace(model.recommendPhone))
            {
                int result = clienterDao.CheckRecommendPhone(model.recommendPhone); //判断推荐人号码在 骑士或者物流公司中是否存在
                if (result == -1)
                {
                    dealResultInfo.DealMsg = "推荐人不存在！";
                    return dealResultInfo;
                }
            }
            using (var tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (!clienterDao.ModifyClienterDetail(model))
                {
                    dealResultInfo.DealMsg = "修改骑士信息失败！";
                    return dealResultInfo;
                }
                if (!string.IsNullOrEmpty(model.Tags))
                {
                    var tag = model.Tags.Split(';');
                    foreach (var item in tag)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            var temp = item.Split(',');
                            var berm = new Ets.Model.DataModel.Tag.TagRelation
                            {
                                TagId = Convert.ToInt32(temp[0]),
                                IsEnable = Convert.ToInt32(temp[1]),
                                UserId = model.Id,
                                CreateBy = model.OptUserName,
                                UserType = TagUserType.Clienter.GetHashCode()
                            };
                            if (!tagRelationDao.Edit(berm))
                            {
                                dealResultInfo.DealMsg = "编辑标签配置信息失败！";
                                return dealResultInfo;
                            }
                        }
                    }
                }
                tran.Complete();
            }
            dealResultInfo.DealMsg = "修改骑士信息成功！";
            dealResultInfo.DealFlag = true;
            return dealResultInfo;
        }

        #region 更新其实余额、可提现余额
        /// <summary>
        /// 更新骑士余额
        /// 胡灵波
        /// 2015年8月13日 09:53:33
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="myOrderInfo"></param>
        public void UpdateCAccountBalance(ClienterMoneyPM clienterMoneyPM)
        {
            //更新骑士余额
            clienterDao.UpdateCAccountBalance(new UpdateForWithdrawPM()
            {
                Id = clienterMoneyPM.ClienterId,
                Money = clienterMoneyPM.Amount
            });

            //更新骑士余额流水          
            clienterBalanceRecordDao.Insert(new ClienterBalanceRecord()
            {
                ClienterId = clienterMoneyPM.ClienterId,
                Amount = clienterMoneyPM.Amount,
                Status = clienterMoneyPM.Status,
                Balance = clienterMoneyPM.Balance,
                RecordType = clienterMoneyPM.RecordType,
                Operator = clienterMoneyPM.Operator,
                WithwardId = clienterMoneyPM.WithwardId,
                RelationNo = clienterMoneyPM.RelationNo,
                Remark = clienterMoneyPM.Remark
            });
        }

        /// <summary>
        /// 更新骑士可提现余额
        /// 胡灵波
        /// 2015年8月13日 10:38:31
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="myOrderInfo"></param>
        public void UpdateCAllowWithdrawPrice(ClienterMoneyPM clienterMoneyPM)
        {
            //更新骑士余额
            clienterDao.UpdateCAllowWithdrawPrice(new UpdateForWithdrawPM()
            {
                Id = clienterMoneyPM.ClienterId,
                Money = clienterMoneyPM.Amount
            });

            //更新骑士可提现流水          
            clienterAllowWithdrawRecordDao.Insert(new ClienterAllowWithdrawRecord()
            {
                ClienterId = clienterMoneyPM.ClienterId,
                Amount = clienterMoneyPM.Amount,
                Status = clienterMoneyPM.Status,
                Balance = clienterMoneyPM.Balance,
                RecordType = clienterMoneyPM.RecordType,
                Operator = clienterMoneyPM.Operator,
                WithwardId = clienterMoneyPM.WithwardId,
                RelationNo = clienterMoneyPM.RelationNo,
                Remark = clienterMoneyPM.Remark
            });
        }

        /// <summary>
        /// 更新骑士余额、可提现余额      
        /// 胡灵波
        /// 2015年8月13日 18:11:23
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="myOrderInfo"></param>
        public void UpdateCBalanceAndWithdraw(ClienterMoneyPM clienterMoneyPM)
        {
            //更新骑士余额
            clienterDao.UpdateCBalanceAndWithdraw(new UpdateForWithdrawPM()
            {
                Id = clienterMoneyPM.ClienterId,
                Money = clienterMoneyPM.Amount
            });

            //更新骑士余额流水          
            clienterBalanceRecordDao.Insert(new ClienterBalanceRecord()
            {
                ClienterId = clienterMoneyPM.ClienterId,
                Amount = clienterMoneyPM.Amount,
                Status = clienterMoneyPM.Status,
                Balance = clienterMoneyPM.Balance,
                RecordType = clienterMoneyPM.RecordType,
                Operator = clienterMoneyPM.Operator,
                WithwardId = clienterMoneyPM.WithwardId,
                RelationNo = clienterMoneyPM.RelationNo,
                Remark = clienterMoneyPM.Remark
            });
            /*不知道谁在下面多添加了这么一次插入流水,查出来打死他!!!*/
            ////更新骑士余额流水          
            //clienterBalanceRecordDao.Insert(new ClienterBalanceRecord()
            //{
            //    ClienterId = clienterMoneyPM.ClienterId,
            //    Amount = clienterMoneyPM.Amount,
            //    Status = clienterMoneyPM.Status,
            //    Balance = clienterMoneyPM.Balance,
            //    RecordType = clienterMoneyPM.RecordType,
            //    Operator = clienterMoneyPM.Operator,
            //    WithwardId = clienterMoneyPM.WithwardId,
            //    RelationNo = clienterMoneyPM.RelationNo,
            //    Remark = clienterMoneyPM.Remark
            //});
            //更新骑士可提现流水          
            clienterAllowWithdrawRecordDao.Insert(new ClienterAllowWithdrawRecord()
            {
                ClienterId = clienterMoneyPM.ClienterId,
                Amount = clienterMoneyPM.Amount,
                Status = clienterMoneyPM.Status,
                Balance = clienterMoneyPM.Balance,
                RecordType = clienterMoneyPM.RecordType,
                Operator = clienterMoneyPM.Operator,
                WithwardId = clienterMoneyPM.WithwardId,
                RelationNo = clienterMoneyPM.RelationNo,
                Remark = clienterMoneyPM.Remark
            });

        }
        #endregion

        #region  用户自定义方法 金额
        /// <summary>
        /// 更新骑士金额
        /// 胡灵波
        /// 2015年7月1日 11:24:08        
        /// </summary>
        /// <param name="myOrderInfo"></param>
        /// <param name="uploadReceiptModel"></param>
        /// <param name="orderOther"></param>
        void UpdateClienterMoney(OrderListModel myOrderInfo)
        {
            if (myOrderInfo.HadUploadCount < myOrderInfo.OrderCount)
            {
                return;
            }
            if (!CheckOrderPay(myOrderInfo.Id))
            {
                return;
            }

            decimal orderCommission = myOrderInfo.OrderCommission == null ? 0 : myOrderInfo.OrderCommission.Value;
            decimal settleMoney = myOrderInfo.SettleMoney;
            decimal baseCommission = 0;
            decimal bt = 0;
            if (orderCommission > settleMoney)
            {
                bt = orderCommission - settleMoney;
            }
            baseCommission = orderCommission - bt;

            if (myOrderInfo.Platform == 3)
            {
                //更新骑士余额
                UpdateCAccountBalance(new ClienterMoneyPM()
                {
                    ClienterId = myOrderInfo.clienterId,
                    Amount = orderCommission,
                    Status = ClienterBalanceRecordStatus.Success.GetHashCode(),
                    RecordType = ClienterBalanceRecordRecordType.OrderCommission.GetHashCode(),
                    Operator = string.IsNullOrEmpty(myOrderInfo.ClienterName) ? "骑士" : myOrderInfo.ClienterName,
                    WithwardId = myOrderInfo.Id,
                    RelationNo = myOrderInfo.OrderNo,
                    Remark = "基本佣金" + baseCommission + "元+平台补贴" + bt + "元就是你的订单佣金！"
                });
                //更新骑士无效订单金额
                UpdateInvalidOrder(myOrderInfo);
            }
            else
            {
                bool isOrderNeedAudit = IsOrderNeedAudit(myOrderInfo);
                if (isOrderNeedAudit)//需要审核
                {
                    //更新骑士余额
                    UpdateCAccountBalance(new ClienterMoneyPM()
                                            {
                                                ClienterId = myOrderInfo.clienterId,
                                                Amount = orderCommission,
                                                Status = ClienterBalanceRecordStatus.Success.GetHashCode(),
                                                RecordType = ClienterBalanceRecordRecordType.OrderCommission.GetHashCode(),
                                                Operator = string.IsNullOrEmpty(myOrderInfo.ClienterName) ? "骑士" : myOrderInfo.ClienterName,
                                                WithwardId = myOrderInfo.Id,
                                                RelationNo = myOrderInfo.OrderNo,
                                                Remark = "基本佣金" + baseCommission + "元+平台补贴" + bt + "元就是你的订单佣金！"
                                            });
                    //更新骑士无效订单金额
                    UpdateInvalidOrder(myOrderInfo);
                }
                else
                {
                    // 更新骑士余额、可提现余额  
                    UpdateCBalanceAndWithdraw(new ClienterMoneyPM()
                                            {
                                                ClienterId = myOrderInfo.clienterId,
                                                Amount = orderCommission,
                                                Status = ClienterBalanceRecordStatus.Success.GetHashCode(),
                                                RecordType = ClienterBalanceRecordRecordType.OrderCommission.GetHashCode(),
                                                Operator = string.IsNullOrEmpty(myOrderInfo.ClienterName) ? "骑士" : myOrderInfo.ClienterName,
                                                WithwardId = myOrderInfo.Id,
                                                RelationNo = myOrderInfo.OrderNo,
                                                Remark = "基本佣金" + baseCommission + "元+平台补贴" + bt + "元就是你的订单佣金！"
                                            });
                    //将订单标记为加入已提现
                    orderOtherDao.UpdateJoinWithdraw(myOrderInfo.Id);
                    //更新订单审核通过 
                    orderOtherDao.UpdateAuditStatus(myOrderInfo.Id, OrderAuditStatusCommon.Through.GetHashCode(), myOrderInfo.ClienterName);
                }
            }
        }

        /// <summary>
        ///  更新无效订单
        ///  胡灵波
        ///  2015年8月6日 20:11:09
        /// </summary>
        /// <param name="myOrderInfo"></param>
        /// <param name="isNotRealOrder"></param>
        private void UpdateInvalidOrder(OrderListModel myOrderInfo)
        {
            string mess = "主单Id:" + myOrderInfo.Id;

            decimal realOrderCommission = myOrderInfo.OrderCommission == null ? 0 : myOrderInfo.OrderCommission.Value;
            var deductCommissionReason = "";//无效订单原因
            bool isNotRealOrder = CheckIsNotRealOrder(myOrderInfo, out deductCommissionReason);
            mess += ";isNotRealOrder" + isNotRealOrder.ToString();
            LogHelper.LogWriter(" UpdateInvalidOrder", new { obj = "时间：" + DateTime.Now.ToString() + mess });

            if (isNotRealOrder)
            {
                //获取无效订单佣金
                realOrderCommission = realOrderCommission > myOrderInfo.SettleMoney ? myOrderInfo.SettleMoney : realOrderCommission;
          
                IOrderProvider iOrderProvider = new OrderProvider();
                OrderOtherPM orderOtherPM = new OrderOtherPM();
                orderOtherPM.OrderId = myOrderInfo.Id;
                orderOtherPM.RealOrderCommission = realOrderCommission;
                orderOtherPM.DeductCommissionReason = deductCommissionReason;
                orderOtherPM.DeductCommissionType = 1;
                iOrderProvider.UpdateOrderIsReal(orderOtherPM);
            }
        }
        /// <summary>
        /// 更新商户金额
        /// 胡灵波
        /// 2015年8月6日 18:33:16
        /// </summary>
        /// <param name="myOrderInfo"></param>
        /// <returns></returns>
        void UpdateBusinessMoney(OrderListModel myOrderInfo)
        {
            if (!(bool)myOrderInfo.IsPay && myOrderInfo.MealsSettleMode == MealsSettleMode.LineOn.GetHashCode())
            {
                // 更新商户余额、可提现余额     
                BusinessMoneyPM businessMoneyPm = new BusinessMoneyPM()
                {
                    BusinessId = myOrderInfo.businessId,
                    Amount = myOrderInfo.BusinessReceivable,
                    Status = BusinessBalanceRecordStatus.Success.GetHashCode(),
                    RecordType = BusinessBalanceRecordRecordType.OrderMeals.GetHashCode(),
                    Operator = myOrderInfo.ClienterName,
                    WithwardId = myOrderInfo.Id,
                    RelationNo = myOrderInfo.OrderNo,
                    Remark = "返还商家订单菜品费"
                };

                #region 当有现金支付的子订单时，商家余额增加金额 = 商户应收-现金支付的子订单金额  wc修改

                //获取该订单信息
                List<OrderChildInfo> orderChildInfos = orderChildDao.GetByOrderId(myOrderInfo.Id);
                List<OrderChildInfo> cashChildInfos =
                    orderChildInfos.Where(t => t.PayType == PayTypeEnum.CashPay.GetHashCode()).ToList();
                if (cashChildInfos.Count > 0)
                {
                    decimal goodPrice = cashChildInfos.Sum(t => t.GoodPrice);
                    businessMoneyPm.Amount = businessMoneyPm.Amount - goodPrice;
                }

                #endregion

                iBusinessProvider.UpdateBBalanceAndWithdraw(businessMoneyPm);
            }
        }

        /// <summary>
        /// 判断当前订单是否为无效订单
        /// zhaohailong20150706
        /// 更改人：胡灵波
        /// 2015年8月13日 19:50:45
        /// </summary>
        /// <param name="myOrderInfo"></param>
        /// <returns></returns>
        private bool CheckIsNotRealOrder(OrderListModel myOrderInfo, out string reason)
        {
            OrderMapDetail mapDetail = orderDao.GetOrderMapDetail(myOrderInfo.Id);
            GlobalConfigModel globalSetting = GlobalConfigDao.GlobalConfigGet(0);
            string mess = "";
            mess += "GrabToCompleteDistance" + mapDetail.GrabToCompleteDistance.ToString();
            mess += "GrabToCompleteDistanc" + globalSetting.GrabToCompleteDistance;
            LogHelper.LogWriter(" CheckIsNotRealOrder", new { obj = "时间：" + DateTime.Now.ToString() + mess });

            bool boolreason = false;
            StringBuilder sb = new StringBuilder();
            if (mapDetail.GrabToCompleteDistance > -1)//如果抢单和完成两个点的坐标都有效，才进行距离判断
            {
                if (mapDetail.GrabToCompleteDistance <= ParseHelper.ToInt(globalSetting.GrabToCompleteDistance, 0))
                {
                    sb.Append("接单完成位置重合;");
                    boolreason = true;
                }
            }

            //DateTime actualDoneDate = actualDoneDate = ParseHelper.ToDatetime(mapDetail.ActualDoneDate);
            //if (!(DateTime.Parse(mapDetail.GrabTime).AddMinutes(5) < actualDoneDate &&
            //    actualDoneDate < DateTime.Parse(mapDetail.GrabTime).AddMinutes(120)))
            //{
            //    sb.Append("完成时间不在5-120分钟内;");
            //    boolreason = true;
            //}
            DateTime actualDoneDate = actualDoneDate = ParseHelper.ToDatetime(mapDetail.ActualDoneDate);
            if (actualDoneDate < DateTime.Parse(mapDetail.GrabTime).AddMinutes(5))
            {
                sb.Append("任务完成时间小于5分钟");
                boolreason = true;
            }
            if (actualDoneDate > DateTime.Parse(mapDetail.GrabTime).AddMinutes(120))
            {
                sb.Append("任务完成时间大于120分钟");
                boolreason = true;
            }


            int num = orderDao.GetTotalOrderNumByClienterID(myOrderInfo.clienterId, actualDoneDate);
            var orderCountSetting = ParseHelper.ToInt(globalSetting.OrderCountSetting, 50);
            //如果骑士今天已经完成（或完成后，又取消了,不包含当前任务中的订单数量）的订单数量大于配置的值，则当前任务中的所有订单都扣除网站补贴
            if (num > orderCountSetting)
            {
                sb.AppendFormat("完成订单超过{0}个;", orderCountSetting);
                boolreason = true;
            }

            DateTime doneDate = ParseHelper.ToDatetime(myOrderInfo.ActualDoneDate, DateTime.Now).AddDays(1);//完成时间加一天
            if (myOrderInfo.Status == OrderStatus.Status1.GetHashCode() && myOrderInfo.ActualDoneDate != null && DateTime.Now >= doneDate)
            {
                sb.Append("上传小票不符要求");
                boolreason = true;
            }
            reason = sb.ToString();
            return boolreason;
        }
        #endregion

        #region 用户自定义方法  骑士账号错误信息
        /// <summary>
        /// 获取获取骑士账号错误信息
        /// 胡灵波
        /// 2015年8月19日 13:18:10
        /// </summary>
        public void GetAccountErr()
        {
            var emailSendTo = Config.ConfigKey("EmailSendTo");
            var copyTo = Config.ConfigKey("CopyTo");
            StringBuilder sbEmail = new StringBuilder("");

            IList<ClienterModel> clienterList = clienterDao.QueryIdList();
            for (int k = 0; k < clienterList.Count; k++)
            {
                int id = clienterList[k].Id;
                string trueName = clienterList[k].TrueName;
                string phoneNo = clienterList[k].PhoneNo;

                //获取骑士账号错误信息
                StringBuilder sbClienterErr = GetClienterErr(id);
                //获取骑士余额流水错误信息
                StringBuilder sbCBalanceRecordErr = GetCBalanceRecordErr(id);
                //获取骑士可提现余额流水错误信息
                StringBuilder sbCAWthdrawRecordErr = GetCAWthdrawRecordErr(id);

                if (sbClienterErr.Length > 0 ||
                    sbCBalanceRecordErr.Length > 0 ||
                    sbCAWthdrawRecordErr.Length > 0
                    )
                {
                    sbEmail.AppendLine("当前骑士Id:" + id.ToString() + " 真实姓名：" + trueName + " 联系电话：" + phoneNo);
                    sbEmail.Append(sbClienterErr);
                    sbEmail.Append(sbCBalanceRecordErr);
                    sbEmail.Append(sbCAWthdrawRecordErr);
                    sbEmail.AppendLine("");
                }
            }

            //获取FinishAll错误的订单
            StringBuilder sbFinishAllErr = GetFinishAllErr();
            if (sbFinishAllErr.Length > 0)
            {
                sbEmail.Append(sbFinishAllErr);
            }

            if (sbEmail.Length > 0)
                EmailHelper.SendEmailTo(sbEmail.ToString(), emailSendTo, "骑士异常金额", copyTo, false);

        }

        /// <summary>
        /// 获取骑士账号错误信息
        /// 胡灵波
        /// 2015年8月19日 15:59:10
        /// </summary>
        /// <param name="clienterId"></param>
        /// <returns></returns>
        private StringBuilder GetClienterErr(int clienterId)
        {
            StringBuilder strErr = new StringBuilder("");
            ClienterModel clienterModel = clienterDao.GetClienterById(clienterId);
            decimal accountBalance = clienterModel.AccountBalance == null ? 0 : clienterModel.AccountBalance.Value;
            decimal allowWithdrawPrice = clienterModel.AllowWithdrawPrice;
            if (accountBalance < 0)
            {
                strErr.AppendLine("账户余额<0：" + accountBalance);
            }
            if (allowWithdrawPrice < 0)
            {
                strErr.AppendLine("可提现余额<0：" + allowWithdrawPrice);
            }
            if (allowWithdrawPrice > accountBalance)
            {
                strErr.AppendLine("可提现余额>提现余额：" + allowWithdrawPrice + "," + accountBalance);
            }

            return strErr;
        }

        /// <summary>
        /// 获取骑士余额流水错误信息
        /// 胡灵波
        /// 2015年8月19日 15:59:52
        /// </summary>
        /// <param name="clienterId"></param>
        /// <returns></returns>
        private StringBuilder GetCBalanceRecordErr(int clienterId)
        {
            StringBuilder strErr = new StringBuilder("");
            IList<ClienterBalanceRecord> clienterBalanceRecordList = clienterBalanceRecordDao.Query(new ClienterBalanceRecordPM() { ClienterId = clienterId });
            for (int i = 0; i < clienterBalanceRecordList.Count - 1; i++)
            {
                long id = clienterBalanceRecordList[i].Id;
                decimal balance = clienterBalanceRecordList[i].Balance + clienterBalanceRecordList[i + 1].Amount;
                decimal balance2 = clienterBalanceRecordList[i + 1].Balance;
                if (balance != balance2)
                {
                    strErr.AppendLine("骑士余额表id:" + id + " 骑士余额：" + balance + " != 骑士余额:" + balance2);
                    return strErr;
                }
            }

            return strErr;
        }

        /// <summary>
        /// 获取骑士可提现余额流水错误信息
        /// 胡灵波
        /// 2015年8月19日 15:59:52
        /// </summary>
        /// <param name="clienterId"></param>
        /// <returns></returns>
        private StringBuilder GetCAWthdrawRecordErr(int clienterId)
        {
            StringBuilder strErr = new StringBuilder("");
            IList<ClienterAllowWithdrawRecord> clienterBalanceRecordList = clienterAllowWithdrawRecordDao.Query(new ClienterAllowWithdrawRecordPM() { ClienterId = clienterId });
            for (int i = 0; i < clienterBalanceRecordList.Count - 1; i++)
            {
                long id = clienterBalanceRecordList[i].Id;
                decimal balance = clienterBalanceRecordList[i].Balance + clienterBalanceRecordList[i + 1].Amount;
                decimal balance2 = clienterBalanceRecordList[i + 1].Balance;
                if (balance != balance2)
                {
                    strErr.AppendLine("骑士可提现余额id:" + id + " 骑士可提现余额：" + balance + " != 骑士可提现余额:" + balance2);
                    return strErr;
                }
            }

            return strErr;
        }

        /// <summary>
        /// 获取FinishAll错误的订单
        /// </summary>
        /// <param name="clienterId"></param>
        /// <returns></returns>
        private StringBuilder GetFinishAllErr()
        {
            StringBuilder strErr = new StringBuilder("");
            IList<order> orderList = orderDao.GetFinallErrByClienterId();
            for (int i = 0; i < orderList.Count; i++)
            {
                int id = orderList[i].Id;
                strErr.AppendLine("订单表id:" + id + " 已完成且上传完小票FinishAll=0");
            }
            return strErr;
        }
        #endregion

        #region 用户自定义方法

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
        /// 订单是否需要审核
        /// 默认不审核
        /// </summary>
        /// <param name="myOrderInfo"></param>
        /// <returns></returns>
        bool IsOrderNeedAudit(OrderListModel myOrderInfo)
        {
            bool bl=false;

            //非物流公司、需要审核、众包骑士
            if (myOrderInfo.DeliveryCompanyID <= 0 && myOrderInfo.IsOrderChecked == 1 && myOrderInfo.GradeType == 1)
            {
                bl = true;
            }
            return bl;
        }
        #endregion
        /// 设置骑士是否接受推送
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SetReceivePush(ClienterReceivePushModel model)
        {
            return clienterDao.SetReceivePush(model);
        }


        public bool UpdateClientHeadPhotoInfo(ClienterModel clienterModel)
        {
            return clienterDao.UpdateClientHeadPhotoInfo(clienterModel);
        }

        /// <summary>
        /// 更新骑士可提现 
        /// 闪送模式 服务调用
        /// </summary>
        public void UpdateCAllowWithdrawPrice(NonJoinWithdrawModel njwModel)
        {
            //更新骑士可提现余额
            UpdateCAllowWithdrawPrice(new ClienterMoneyPM()
            {
                ClienterId = njwModel.clienterId,
                Amount = njwModel.clienterPrice,
                Status = ClienterAllowWithdrawRecordStatus.Success.GetHashCode(),
                RecordType = ClienterAllowWithdrawRecordType.OrderCommission.GetHashCode(),
                Operator = njwModel.clienterId.ToString(),
                WithwardId = njwModel.id,
                RelationNo = njwModel.OrderNo,
                Remark = "闪送模式服务加提现"
            });

            //更新已提现状态
            orderOtherDao.UpdateJoinWithdraw(njwModel.id);
            //更新审核状态
            orderOtherDao.UpdateAuditStatus(njwModel.id, OrderAuditStatusCommon.Through.GetHashCode(), "系统");
            //写入订单日志                
            orderSubsidiesLogDao.Insert(new OrderSubsidiesLog()
            {
                OrderId = njwModel.id,
                Price = njwModel.clienterPrice,
                OptName = "系统服务",
                Remark = "闪送模式服务自动审核通过，增加" + njwModel.clienterPrice + "元可提现金额",
                OptId = njwModel.clienterId,
                OrderStatus = OrderOperationCommon.AuditStatusOk.GetHashCode(),
                Platform = SuperPlatform.ManagementBackground.GetHashCode()
            });

        }

    }

}
