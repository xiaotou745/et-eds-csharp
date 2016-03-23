﻿using System.Data;
using System.Text;
using ETS.Const;
using Ets.Dao.GlobalConfig;
using Ets.Dao.Message;
using Ets.Dao.User;
using Ets.Model.Common;
using Ets.Model.DataModel.Tag;
using Ets.Model.DomainModel.Business;
using ETS.Data.PageData;
using System;
using System.Collections.Generic;
using CalculateCommon;
using Ets.Model.ParameterModel.Area;
using Ets.Model.ParameterModel.Business;
using System.Linq;
using ETS.Enums;
using Ets.Model.DataModel.Business;
using ETS.NoSql.RedisCache;
using ETS.Util;
using Ets.Model.DataModel.Group;
using ETS.Validator;
using ETS;
using System.Threading.Tasks;
using ETS.Sms;
using ETS.Transaction.Common;
using ETS.Transaction;
using Ets.Model.ParameterModel.User;
using Ets.Model.ParameterModel.Order;
using Ets.Service.Provider.Order;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Area;
using Ets.Service.IProvider.Business;
using Ets.Dao.Business;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using Ets.Model.DomainModel.Business;
using Ets.Model.ParameterModel.Finance;
using Ets.Dao.Finance;
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.Common;
using ETS.Security;
using Ets.Dao.Authority;
using Ets.Model.DomainModel.Authority;
using Ets.Dao.Order;
namespace Ets.Service.Provider.Business
{

    /// <summary>
    /// 商户业务逻辑接口实现类  add by caoheyang 20150311
    /// </summary>
    public class BusinessProvider : IBusinessProvider
    {
        readonly IAreaProvider iAreaProvider = new AreaProvider();
        readonly BusinessDao businessDao = new BusinessDao();
        readonly BusinessBalanceRecordDao businessBalanceRecordDao = new BusinessBalanceRecordDao();
        readonly ITokenProvider iTokenProvider = new TokenProvider();
        readonly BusinessLoginLogDao businessLoginLogDao = new BusinessLoginLogDao();
        readonly OrderRegionDao orderRegionDao = new OrderRegionDao();
        readonly BusinessGroupDao businessGroupDao = new BusinessGroupDao();
        BusinessSetpChargeChildDao businessSetpChargeChildDao = new BusinessSetpChargeChildDao();
        /// <summary>
        /// app端商户获取订单   add by caoheyang 20150311
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IList<BusiGetOrderModel> GetOrdersApp(BussOrderParaModelApp paraModel)
        {
            PageInfo<BusiOrderSqlModel> pageinfo = businessDao.GetOrdersAppToSql<BusiOrderSqlModel>(paraModel);
            IList<BusiOrderSqlModel> list = pageinfo.Records;

            List<BusiGetOrderModel> listOrder = new List<BusiGetOrderModel>();
            foreach (BusiOrderSqlModel from in list)
            {
                BusiGetOrderModel model = new BusiGetOrderModel();
                model.ActualDoneDate = from.ActualDoneDate;
                model.Amount = from.Amount;
                model.OrderCount = from.OrderCount;
                model.IsPay = from.IsPay;
                model.OrderNo = from.OrderNo;
                model.PickUpAddress = from.PickUpAddress;
                if (from.PubDate != null)
                {
                    model.PubDate = from.PubDate;
                }
                model.PickUpName = from.BusinessName;

                if (!string.IsNullOrEmpty(from.ReceviceAddress))
                    model.ReceviceAddress = from.ReceviceAddress;
                else
                {
                    model.ReceviceAddress = OrderConst.ReceviceAddress;
                }

                model.ReceviceName = from.ReceviceName == null ? "" : from.ReceviceName.Trim();
                model.RecevicePhoneNo = from.RecevicePhoneNo;
                model.Remark = from.Remark;
                model.Status = from.Status;
                model.superManName = from.SuperManName;
                model.superManPhone = from.SuperManPhone;
                model.OrderFrom = from.OrderFrom.ToString();
                model.OrderId = from.OrderId;
                model.MealsSettleMode = from.MealsSettleMode;
                model.TotalAmount = from.TotalAmount;
                model.OrderFromName = from.OrderFromName;
                model.OriginalOrderNo = from.OriginalOrderNo;
                if (from.BusinessId > 0 && from.ReceviceLongitude != null && from.ReceviceLatitude != null)
                {
                    var d1 = new Degree(from.Longitude.Value, from.Latitude.Value);
                    var d2 = new Degree(from.ReceviceLongitude.Value, from.ReceviceLatitude.Value);
                    model.distanceB2R = ParseHelper.ToDouble(CoordDispose.GetDistanceGoogle(d1, d2));
                }
                else
                    model.distanceB2R = 0;
                listOrder.Add(model);
            }
            return listOrder;
        }


        /// <summary>
        /// 商户结算列表--2015.3.12 平扬
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="name"></param>
        /// <param name="phoneno"></param>
        /// <param name="authorityCityNameListStr"></param>
        /// <returns></returns>
        public ResultInfo<IList<BusinessCommissionDM>> GetBusinessCommission(DateTime t1, DateTime t2, string name, string phoneno, int groupid, string BusinessCity, string authorityCityNameListStr)
        {
            var result = new ResultInfo<IList<BusinessCommissionDM>> { Data = null, Result = false, Message = "" };
            try
            {
                if (t1 > t2)
                {
                    result.Result = false;
                    result.Message = "开始时间不能大于结束时间";
                    return result;
                }
                var list = businessDao.GetBusinessCommission(t1, t2, name, phoneno, groupid, BusinessCity, authorityCityNameListStr);
                if (list != null && list.Count > 0)
                {
                    result.Data = list;
                    result.Result = true;
                    result.Message = "成功";
                }
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "失败";
                ETS.Util.LogHelper.LogWriter(ex, "BusinessProvider.GetBusinessCommission-商户结算列表");
            }
            return result;
        }

        /// <summary>
        /// 设置商家结算比例-外送费    设置结算比例2015.3.12 平扬
        /// </summary>
        /// <param name="id">商家id</param>
        /// <param name="price">结算比例</param>
        /// <param name="waisongfei">外送费</param>
        /// <param name="model">log实体</param>
        /// <returns></returns>
        public bool SetCommission(int id, decimal price, decimal waisongfei, UserOptRecordPara model)
        {
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                bool res = businessDao.setCommission(id, price, waisongfei);
                int result = new UserOptRecordDao().InsertUserOptRecord(model);
                tran.Complete();
                return res;
            }
        }

        /// <summary>
        /// 设置结算比例
        /// danny-20150504
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModifyCommission(BusListResultModel busListResultModel, UserOptRecordPara model)
        {
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                bool res = businessDao.ModifyCommission(busListResultModel);
                int result = new UserOptRecordDao().InsertUserOptRecord(model);
                tran.Complete();
                return res;
            }
        }

        /// <summary>
        /// 生成商户结算excel文件2015.3.12 平扬
        /// </summary>
        /// <returns></returns>
        public string CreateExcel(BusinessCommissionModel paraModel)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("<table border=1 cellspacing=0 cellpadding=5 rules=all>");
            //输出表头.
            strBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            strBuilder.AppendLine("<td>商户名称</td>");
            strBuilder.AppendLine("<td>订单金额</td>");
            strBuilder.AppendLine("<td>订单数量</td>");
            strBuilder.AppendLine("<td>结算比例(%)</td>");
            strBuilder.AppendLine("<td>开始时间</td>");
            strBuilder.AppendLine("<td>结束时间</td>");
            strBuilder.AppendLine("<td>结算金额</td>");
            strBuilder.AppendLine("</tr>");
            //输出数据.
            strBuilder.AppendLine(string.Format("<tr><td>{0}</td>", paraModel.Name));
            strBuilder.AppendLine(string.Format("<td>{0}</td>", paraModel.Amount));
            strBuilder.AppendLine(string.Format("<td>{0}</td>", paraModel.OrderCount));
            strBuilder.AppendLine(string.Format("<td>{0}%</td>", paraModel.BusinessCommission));
            strBuilder.AppendLine(string.Format("<td>{0}</td>", paraModel.T1));
            strBuilder.AppendLine(string.Format("<td>{0}</td>", paraModel.T2));
            strBuilder.AppendLine(string.Format("<td>{0}</td></tr>", paraModel.TotalAmount));
            strBuilder.AppendLine("</table>");
            return strBuilder.ToString();
        }


        /// <summary>
        /// B端注册 
        /// 窦海超
        /// 2015年3月16日 10:19:45
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel<BusiRegisterResultModel> PostRegisterInfo_B(ParamModel ParModel)
        {
            RegisterInfoPM model = JsonHelper.JsonConvertToObject<RegisterInfoPM>(AESApp.AesDecrypt(ParModel.data));
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            string key = string.Concat(RedissCacheKey.RegisterCount_B, model.phoneNo);
            int excuteCount = redis.Get<int>(key);
            if (excuteCount >= 10)
            {
                return ResultModel<BusiRegisterResultModel>.Conclude(BusinessRegisterStatus.CountError);
            }
            redis.Set(key, excuteCount + 1, new TimeSpan(0, 5, 0));


            var code = redis.Get<string>(RedissCacheKey.PostRegisterInfo_B + model.phoneNo);
            Enum returnEnum = null;
            if (string.IsNullOrEmpty(model.phoneNo))
            {
                returnEnum = BusinessRegisterStatus.PhoneNumberEmpty; //手机号非空验证
            }
            else if (string.IsNullOrEmpty(model.passWord))
            {
                returnEnum = BusinessRegisterStatus.PasswordEmpty;//密码非空验证 
            }
            else if (string.IsNullOrEmpty(code) || code.ToLower() != model.verifyCode.ToLower()) //验证码正确性验证
            {
                returnEnum = BusinessRegisterStatus.IncorrectCheckCode; //判断验证法录入是否正确
            }
            else if (businessDao.CheckBusinessExistPhone(model.phoneNo))
            {
                returnEnum = BusinessRegisterStatus.PhoneNumberRegistered;//判断该手机号是否已经注册过
            }
            else if (!string.IsNullOrWhiteSpace(model.RecommendPhone) &&
                (model.RecommendPhone.Length != 11 || model.RecommendPhone[0] != '1'))
            {
                returnEnum = BusinessRegisterStatus.RecommendPhoneError;//填入的推荐人手机号有误
            }
            if (string.IsNullOrEmpty(model.timespan)) //判断时间戳是否为空
            {
                returnEnum = BusinessRegisterStatus.TimespanEmpty;
            }
            else if (businessDao.IsExist(model))
            {
                returnEnum = BusinessRegisterStatus.HasExist;//商户已存在
            }
            if (returnEnum != null)
            {
                return ResultModel<BusiRegisterResultModel>.Conclude(returnEnum);
            }

            string appkey = Guid.NewGuid().ToString();
            model.Appkey = appkey;
            int userid = businessDao.Insert(model);

            string token = iTokenProvider.GetToken(new TokenModel()
                        {
                            Ssid = model.Ssid,
                            Appkey = appkey
                        });
            BusiRegisterResultModel resultModel = new BusiRegisterResultModel()
            {
                userId = userid,
                Appkey = appkey,
                Token = token
            };
            return ResultModel<BusiRegisterResultModel>.Conclude(BusinessRegisterStatus.Success, resultModel);

        }

        /// <summary>
        /// 后台添加商户
        /// 平扬
        /// 2015年4月17日 17:19:45
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel<BusiRegisterResultModel> AddBusiness(AddBusinessModel model)
        {

            Enum returnEnum = null;
            if (string.IsNullOrEmpty(model.phoneNo))
                returnEnum = BusinessRegisterStatus.PhoneNumberEmpty; //手机号非空验证
            else if (string.IsNullOrEmpty(model.passWord))
                returnEnum = BusinessRegisterStatus.PasswordEmpty;//密码非空验证  

            else if (businessDao.CheckBusinessExistPhone(model.phoneNo))
                returnEnum = BusinessRegisterStatus.PhoneNumberRegistered;//判断该手机号是否已经注册过

            else if (string.IsNullOrEmpty(model.city) || string.IsNullOrEmpty(model.CityId)) //城市以及城市编码非空验证
                returnEnum = BusinessRegisterStatus.cityIdEmpty;
            if (returnEnum != null)
            {
                return ResultModel<BusiRegisterResultModel>.Conclude(returnEnum);
            }
            model.passWord = ETS.Security.MD5.Encrypt(model.passWord);
            BusiRegisterResultModel resultModel = new BusiRegisterResultModel()
            {
                userId = businessDao.addBusiness(model)
            };
            return ResultModel<BusiRegisterResultModel>.Conclude(BusinessRegisterStatus.Success, resultModel);// CustomerRegisterStatusEnum.Success;//默认是成功状态

        }


        /// <summary>
        /// B端注册，供第三方使用 
        /// 平扬
        /// 2015年3月26日 17:19:45
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel<NewBusiRegisterResultModel> NewPostRegisterInfo_B(NewRegisterInfoModel model)
        {
            if (string.IsNullOrWhiteSpace(model.PhoneNo))   //手机号非空验证
                return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberEmpty);
            if (string.IsNullOrWhiteSpace(model.B_OriginalBusiId.ToString()))  //判断原平台商户Id不能为空
                return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.OriginalBusiIdEmpty);
            if (string.IsNullOrWhiteSpace(model.B_GroupId.ToString()))  //集团Id不能为空
                return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.GroupIdEmpty);
            //是否存在该商户
            BusinessModel busi = businessDao.CheckExistBusiness(model.B_OriginalBusiId, model.B_GroupId);
            return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.OriginalBusiIdRepeat);

            if (string.IsNullOrWhiteSpace(model.B_City) || string.IsNullOrWhiteSpace(model.B_CityCode.ToString())) //城市以及城市编码非空验证
                return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.cityIdEmpty);
            if (string.IsNullOrEmpty(model.B_Name.Trim())) //商户名称
                return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.BusiNameEmpty);
            if (string.IsNullOrWhiteSpace(model.Address) || string.IsNullOrWhiteSpace(model.B_Province) || string.IsNullOrWhiteSpace(model.B_City) || string.IsNullOrWhiteSpace(model.B_Area) || string.IsNullOrWhiteSpace(model.B_AreaCode) || string.IsNullOrWhiteSpace(model.B_CityCode) || string.IsNullOrWhiteSpace(model.B_ProvinceCode))  //商户地址 省市区 不能为空
                return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.BusiAddressEmpty);
            if (model.CommissionTypeId == 0)
            {
                return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.CommissionTypeIdEmpty);
            }
            model.B_Password = MD5Helper.MD5(string.IsNullOrEmpty(model.B_Password) ? "abc123" : model.B_Password);
            #region 转换省市区
            //转换省
            var _province = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModelTranslate() { Name = model.B_Province, JiBie = 2 });
            if (_province != null)
            {
                model.B_ProvinceCode = _province.NationalCode.ToString();
            }
            //转换市 
            var _city = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModelTranslate() { Name = model.B_City, JiBie = 3 });
            if (_city != null)
            {
                model.B_CityCode = _city.NationalCode.ToString();
            }
            //转换区
            var _area = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModelTranslate() { Name = model.B_Area, JiBie = 4 });
            if (_area != null)
            {
                model.B_AreaCode = _area.NationalCode.ToString();
            }
            #endregion
            var business = NewRegisterInfoModelTranslator.Instance.Translate(model);
            business.Status = (byte)BusinessStatus.Status1.GetHashCode();
            int businessid = businessDao.InsertOtherBusiness(business);
            if (businessid > 0)
            {
                var resultModel = new NewBusiRegisterResultModel
                {
                    BusiRegisterId = businessid
                };
                LogHelper.LogWriter("第三方调用商户注册接口", new { model = model, Message = CustomerRegisterStatus.Success });
                return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.Success, resultModel);
            }
            return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.Faild);

        }

        public BusinessModel CheckExistBusiness(int originalId, int groupId)
        {
            return businessDao.CheckExistBusiness(originalId, groupId);
        }

        /// <summary>
        /// B端登录
        /// 窦海超
        /// 2015年3月16日 16:11:59
        /// </summary>
        /// <param name="model">用户名，密码对象</param>
        /// <returns>登录后返回实体对象</returns>
        public ResultModel<BusiLoginResultModel> PostLogin_B(ParamModel parModel)
        {
            try
            {
                LoginModel model = new LoginModel();
                if (parModel.data == null)
                {
                    model.phoneNo = parModel.phoneNo;
                    model.Ssid = parModel.Ssid;
                    model.passWord = parModel.passWord;
                }
                else
                    model = JsonHelper.JsonConvertToObject<LoginModel>(AESApp.AesDecrypt(parModel.data));
                var redis = new RedisCache();
                string key = string.Concat(RedissCacheKey.LoginCount_B, model.phoneNo);
                int excuteCount = redis.Get<int>(key);
                if (excuteCount >= 10)
                {
                    return ResultModel<BusiLoginResultModel>.Conclude(LoginModelStatus.CountError);
                }
                redis.Set(key, excuteCount + 1, new TimeSpan(0, 5, 0));

                BusiLoginResultModel resultMode = new BusiLoginResultModel();
                DataTable dt = businessDao.LoginSql(model);
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return ResultModel<BusiLoginResultModel>.Conclude(LoginModelStatus.InvalidCredential, resultMode);
                }
                DataRow row = dt.Rows[0];

                string cityId = row["cityId"].ToString();
                if (cityId.Equals("110100"))
                {
                    //北京
                    cityId = "1";

                }
                else if (cityId.Equals("310100"))
                {
                    cityId = "73";
                }
                resultMode.userId = ParseHelper.ToInt(row["userId"]);
                resultMode.status = Convert.ToByte(row["status"]);
                resultMode.city = row["city"].ToString();
                resultMode.Address = row["Address"].ToString();
                resultMode.districtId = row["districtId"].ToString();
                resultMode.district = row["district"].ToString();
                resultMode.Landline = row["Landline"].ToString();
                resultMode.Name = row["Name"].ToString();
                resultMode.cityId = cityId;
                resultMode.phoneNo = row["PhoneNo2"] == null ? row["PhoneNo"].ToString() : row["PhoneNo2"].ToString();
                resultMode.DistribSubsidy = row["DistribSubsidy"] == null ? 0 : ParseHelper.ToDecimal(row["DistribSubsidy"]);
                resultMode.OriginalBusiId = row["OriginalBusiId"].ToString();
                resultMode.Appkey = new Guid(row["Appkey"].ToString());

                string token = iTokenProvider.GetToken(new TokenModel()
                {
                    Ssid = model.Ssid,
                    Appkey = row["Appkey"].ToString()
                });
                resultMode.Token = token;

                    //记录登陆日志
                businessLoginLogDao.Insert(new BusinessLoginLogDM
                                {
                                    BusinessId = ParseHelper.ToInt(row["userId"]),
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

                //string resultStr = AESApp.AesDecrypt(JsonHelper.JsonConvertToString(resultMode));
                return ResultModel<BusiLoginResultModel>.Conclude(LoginModelStatus.Success, resultMode);//BusiLoginResultModel
            }
            catch (Exception ex)
            {
                return ResultModel<BusiLoginResultModel>.Conclude(LoginModelStatus.InvalidCredential);
                throw;
            }
        }
        /// <summary>
        /// 根据商户Id获取商户信息
        /// </summary>
        /// <param name="busiId"></param>
        /// <returns></returns>
        public BusListResultModel GetBusiness(int busiId)
        {
            return businessDao.GetBusiness(busiId);
        }

        /// <summary>
        /// 根据商户Id获取商户信息
        /// </summary>
        /// <param name="busiId"></param>
        /// <returns></returns>
        public BusListResultModel GetBusiness(int originalBusiId, int groupId)
        {
            return businessDao.GetBusiness(originalBusiId, groupId);
        }
        /// <summary>
        /// 获取商户信息
        /// danny-20150316
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<BusListResultModel> GetBusinesses(BusinessSearchCriteria criteria)
        {
            PageInfo<BusListResultModel> pageinfo = businessDao.GetBusinesses<BusListResultModel>(criteria);
            return pageinfo;
        }
        /// <summary>
        /// 商户配送统计
        /// danny-20150408
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<BusinessesDistributionModel> GetBusinessesDistributionStatisticalInfo(OrderSearchCriteria criteria)
        {
            PageInfo<BusinessesDistributionModel> pageinfo = businessDao.GetBusinessesDistributionStatisticalInfo<BusinessesDistributionModel>(criteria);
            return pageinfo;
        }
        /// <summary>
        /// 更新审核状态
        /// danny-20150317
        /// </summary> 
        /// <returns></returns>
        //public bool UpdateAuditStatus(int id, AuditStatus enumStatusType)
        //{
        //    ETS.NoSql.RedisCache.RedisCache redis = new ETS.NoSql.RedisCache.RedisCache();
        //    string cacheKey = string.Format(RedissCacheKey.BusinessProvider_GetUserStatus, id);
        //    redis.Delete(cacheKey);
        //    return businessDao.UpdateAuditStatus(id, enumStatusType);
        //}
        /// <summary>
        /// 商户审核
        /// wc
        /// </summary>
        /// <param name="bam"></param>
        /// <returns></returns> 
        public bool UpdateAuditStatus(BusinessAuditModel bam)
        {
            ETS.NoSql.RedisCache.RedisCache redis = new ETS.NoSql.RedisCache.RedisCache();
            string cacheKey = string.Format(RedissCacheKey.BusinessProvider_GetUserStatus, bam.BusinessId);
            redis.Delete(cacheKey);
            return businessDao.UpdateAuditStatus(bam);
        }

        //public bool UpdateAuditStatus(int id, int enumStatus, string busiAddress)
        //{
        //    return businessDao.UpdateAuditStatus(id, enumStatus, busiAddress);
        //}
        public bool UpdateAuditStatus(BusinessAuditModel bam, string busiAddress)
        {
            return businessDao.UpdateAuditStatus(bam, busiAddress);
        }


        /// <summary>
        ///  根据城市信息查询当前城市下该集团的所有商户信息
        ///  danny-20150317
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enumStatusType"></param>
        /// <returns></returns>
        public IList<BusListResultModel> GetBussinessByCityInfo(BusinessSearchCriteria criteria)
        {
            return businessDao.GetBussinessByCityInfo(criteria);
        }

        /// <summary>
        /// 修改商户密码
        /// 窦海超
        /// 2015年3月23日 19:11:54
        /// </summary>
        /// <param name="model"></param>
        /// <param name="type">操作类型 默认 0   0代表修改密码  1 代表忘记密码</param>
        /// <returns></returns>
        public ResultModel<BusiModifyPwdResultModel> PostForgetPwd_B(ParamModel ParModel, int type = 0)
        {
            BusiForgetPwdInfoModel model = JsonHelper.JsonConvertToObject<BusiForgetPwdInfoModel>(AESApp.AesDecrypt(ParModel.data));
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            string key = string.Concat(RedissCacheKey.ChangePasswordCount_B, model.phoneNumber);
            int excuteCount = redis.Get<int>(key);
            if (excuteCount >= 10)
            {
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.CountError);
            }
            redis.Set(key, excuteCount + 1, new TimeSpan(0, 5, 0));

            if (string.IsNullOrEmpty(model.password))
            {
                //密码非空验证
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.NewPwdEmpty);
            }
            if (string.IsNullOrEmpty(model.checkCode)) //验证码非空验证
            {
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.checkCodeIsEmpty);
            }

            var code = redis.Get<string>(RedissCacheKey.CheckCodeFindPwd_B + model.phoneNumber);
            if (string.IsNullOrEmpty(code) || code.ToLower() != model.checkCode.ToLower()) //验证码正确性验证
            { return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.checkCodeWrong); }

            var business = businessDao.GetBusinessByPhoneNo(model.phoneNumber);
            if (business == null) //用户是否存在
            {
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.ClienterIsNotExist);
            }
            if (type == 0 && (string.IsNullOrWhiteSpace(model.oldpassword) || business.Password != model.oldpassword))
            {
                //旧密码错误
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.OldPwdError);
            }
            if (businessDao.UpdateBusinessPwdSql(business.Id, model.password))
            {
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.Success);
            }
            else
            {
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.FailedModifyPwd);
            }
        }

        /// <summary>
        /// 获取商户端的统计数量
        /// 窦海超
        /// 2015年3月23日 20:19:02
        /// </summary>
        /// <param name="BusinessId">商户ID</param>
        /// <returns></returns>
        public BusiOrderCountResultModel GetOrderCountData(int BusinessId)
        {
            return businessDao.GetOrderCountDataSql(BusinessId);
        }

        /// <summary>
        /// 判断该 商户是否有资格 
        /// wc
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public bool HaveQualification(int businessId)
        {
            return businessDao.HaveQualification(businessId);
        }
        /// <summary>
        /// 根据集团id获取集团名称
        /// danny-20150324
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public string GetGroupNameById(int groupId)
        {
            return businessDao.GetGroupNameById(groupId);
        }
        /// <summary>
        /// 获取所有可用的集团信息数据
        /// danny-20150324
        /// </summary>
        /// <returns></returns>
        public IList<GroupModel> GetGroups()
        {
            return businessDao.GetGroups();
        }

        /// <summary>
        /// 商户修改外送费
        /// wc
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="waiSongFei"></param>
        /// <returns></returns>
        public int ModifyWaiMaiPrice(int businessId, decimal waiSongFei)
        {
            return businessDao.ModifyWaiMaiPrice(businessId, waiSongFei);
        }
        /// <summary>
        /// 修改商户地址信息 
        /// 返回商户修改后的状态
        /// wc
        /// </summary>
        /// <param name="businessModel"></param>
        /// <returns>商户的当前状态</returns>
        public int UpdateBusinessAddressInfo(BusiAddAddressInfoModel businessModel)
        {
            BusinessModel business = TranslateBusiness(businessModel);

            var busi = businessDao.GetBusiness(businessModel.userId); //查询商户信息
            if (busi.Status == BusinessStatus.Status2.GetHashCode())  //如果商户的状态 为未审核未添加地址，则修改商户状态为 未审核
            {
                business.Status = (byte)BusinessStatus.Status0.GetHashCode();
            }
            int upResult = businessDao.UpdateBusinessAddressInfo(business);
            ETS.NoSql.RedisCache.RedisCache redis = new ETS.NoSql.RedisCache.RedisCache();
            string cacheKey = string.Format(RedissCacheKey.BusinessProvider_GetUserStatus, businessModel.userId);
            redis.Delete(cacheKey);
            return upResult;
        }


        /// <summary>
        /// B端修改商户信息 caoheyang
        /// </summary>
        /// <param name="model"></param>
        /// <returns>商户的当前状态</returns>
        public ResultModel<BusiModifyResultModelDM> UpdateBusinessInfoB(BusiAddAddressInfoModel model)
        {
            if (model.userId <= 0)  //判断 商户id是否正确
            {
                return ResultModel<BusiModifyResultModelDM>.Conclude(UpdateBusinessInfoBReturnEnums.InvalidUserId);
            }
            if (string.IsNullOrWhiteSpace(model.phoneNo))
            {
                return ResultModel<BusiModifyResultModelDM>.Conclude(UpdateBusinessInfoBReturnEnums.PhoneNumberEmpty);
            }
            if (string.IsNullOrWhiteSpace(model.Address))
            {
                return ResultModel<BusiModifyResultModelDM>.Conclude(UpdateBusinessInfoBReturnEnums.AddressEmpty);
            }
            if (string.IsNullOrWhiteSpace(model.businessName))
            {
                return ResultModel<BusiModifyResultModelDM>.Conclude(UpdateBusinessInfoBReturnEnums.BusinessNameEmpty);
            }
            UpdateBusinessInfoBPM business = UpdateBusinessInfoBTranslate(model);
            var busi = businessDao.GetBusiness(model.userId); //查询商户信息
            if (busi == null)
            {
                return ResultModel<BusiModifyResultModelDM>.Conclude(UpdateBusinessInfoBReturnEnums.InvalidUserId);
            }


            int upResult = businessDao.UpdateBusinessInfoB(business);
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            string cacheKey = string.Format(RedissCacheKey.BusinessProvider_GetUserStatus, model.userId);
            redis.Delete(cacheKey);
            return ResultModel<BusiModifyResultModelDM>.Conclude(upResult == -1 ? UpdateBusinessInfoBReturnEnums.UpdateFailed : UpdateBusinessInfoBReturnEnums.Success,
               new BusiModifyResultModelDM() { userId = model.userId, status = upResult });
        }

        /// <summary>
        /// 将入口参数转换为对应的 数据库 Business 实体
        /// wc 注意这里有商户状态的转换
        /// </summary>
        /// <param name="businessModel"></param>
        /// <returns></returns>
        private UpdateBusinessInfoBPM UpdateBusinessInfoBTranslate(BusiAddAddressInfoModel businessModel)
        {
            var to = new UpdateBusinessInfoBPM();
            to.Id = businessModel.userId;  //用户id
            to.Address = string.IsNullOrWhiteSpace(businessModel.Address) ? "" : businessModel.Address.Trim(); //地址
            to.Name = string.IsNullOrWhiteSpace(businessModel.businessName) ? "" : businessModel.businessName.Trim(); //商户名称
            to.Landline = businessModel.landLine; //座机
            to.PhoneNo2 = businessModel.phoneNo.Trim(); //手机号2
            to.City = string.IsNullOrWhiteSpace(businessModel.City) ? "" : businessModel.City.Trim(); //市
            to.district = string.IsNullOrWhiteSpace(businessModel.districtName) ? "" : businessModel.districtName.Trim();  //区域名称
            to.Province = string.IsNullOrWhiteSpace(businessModel.Province) ? "" : businessModel.Province.Trim();  //省份名称
            //修改地址转换 区域编码
            if (!string.IsNullOrWhiteSpace(businessModel.districtName))
            {
                AreaModelTranslate areaModel = iAreaProvider.GetNationalAreaInfo(new AreaModelTranslate() { Name = businessModel.districtName.Trim(), JiBie = 4 });
                to.districtId = areaModel != null ? areaModel.NationalCode.ToString() : "";
                to.AreaCode = areaModel != null ? areaModel.NationalCode.ToString() : "";
            }
            //修改地址转换 市编码
            if (!string.IsNullOrWhiteSpace(businessModel.City))
            {
                AreaModelTranslate areaModel = iAreaProvider.GetNationalAreaInfo(new AreaModelTranslate() { Name = businessModel.City.Trim(), JiBie = 3 });
                to.CityId = areaModel != null ? areaModel.NationalCode.ToString() : "";
                to.CityCode = areaModel != null ? areaModel.NationalCode.ToString() : "";
            }
            //修改地址转换 省份编码
            if (!string.IsNullOrWhiteSpace(businessModel.Province))
            {
                AreaModelTranslate areaModel = iAreaProvider.GetNationalAreaInfo(new AreaModelTranslate() { Name = businessModel.Province.Trim(), JiBie = 2 });
                to.ProvinceCode = areaModel != null ? areaModel.NationalCode.ToString() : "";
            }

            to.Longitude = businessModel.longitude; //经度
            to.Latitude = businessModel.latitude; //纬度 
            to.Status = (byte)BusinessStatus.Status0.GetHashCode();  //用户状态待审核
            to.CheckPicUrl = businessModel.CheckPicUrl;
            to.BusinessLicensePic = businessModel.BusinessLicensePic;
            to.OneKeyPubOrder = 0;//默认不允许一键发单
            //business.Status = Convert.ToByte(GetBussinessStatus.Auditing.GetHashCode());//审核中
            to.Status = Convert.ToByte(GetBussinessStatus.Auditing.GetHashCode());//审核中
            return to;
        }


        /// <summary>
        /// 更新图片地址信息
        /// wc
        /// </summary>
        /// <param name="busiId"></param>
        /// <param name="picName"></param>
        /// <returns></returns>
        public int UpdateBusinessPicInfo(int busiId, string picName)
        {
            ETS.NoSql.RedisCache.RedisCache redis = new ETS.NoSql.RedisCache.RedisCache();
            string cacheKey = string.Format(RedissCacheKey.BusinessProvider_GetUserStatus, busiId);
            redis.Delete(cacheKey);
            int upResult = businessDao.UpdateBusinessPicInfo(busiId, picName);
            return upResult;

        }
        /// <summary>
        /// 将入口参数转换为对应的 数据库 Business 实体
        /// wc 注意这里有商户状态的转换
        /// </summary>
        /// <param name="businessModel"></param>
        /// <returns></returns>
        private BusinessModel TranslateBusiness(BusiAddAddressInfoModel businessModel)
        {
            var to = new BusinessModel();
            to.Id = businessModel.userId;
            to.Address = businessModel.Address.Trim();
            to.Name = businessModel.businessName.Trim();
            to.Landline = businessModel.landLine;
            to.PhoneNo2 = businessModel.phoneNo.Trim();
            //修改地址转换 区域编码
            if (!string.IsNullOrWhiteSpace(businessModel.districtName))
            {
                Ets.Model.DomainModel.Area.AreaModelTranslate areaModel = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModelTranslate() { Name = businessModel.districtName.Trim(), JiBie = 4 });
                if (areaModel != null)
                {
                    to.districtId = areaModel.NationalCode.ToString();
                }
                else
                {
                    to.districtId = businessModel.districtId;
                }
            }

            to.district = businessModel.districtName;
            to.Longitude = businessModel.longitude;
            to.Latitude = businessModel.latitude;
            to.Status = (byte)BusinessStatus.Status0.GetHashCode();
            return to;
        }

        /// <summary>
        /// 请求动态验证码  (找回密码)
        /// 窦海超
        /// 2015年3月26日 17:16:02
        /// </summary>
        /// <param name="PhoneNumber">手机号码</param>
        /// <returns></returns>
        public SimpleResultModel CheckCodeFindPwd(string PhoneNumber)
        {
            if (!CommonValidator.IsValidPhoneNumber(PhoneNumber))  //检查手机号码的合法性
            {
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.InvlidPhoneNumber);
            }
            if (!businessDao.CheckBusinessExistPhone(PhoneNumber))
            {
                //账号不存在 
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.NotExists);
            }
            //string randomCode = new Random().Next(1000).ToString("D4");
            string randomCode = Helper.GenCode(6);
            var msg = string.Format(Config.SmsContentFindPassword, randomCode, SystemConst.MessageBusiness);
            try
            {
                var redis = new ETS.NoSql.RedisCache.RedisCache();
                redis.Add(RedissCacheKey.CheckCodeFindPwd_B + PhoneNumber, randomCode, DateTime.Now.AddHours(1));
                // CacheFactory.Instance.AddObject(PhoneNumber, randomCode);
                // 更新短信通道 
                Task.Factory.StartNew(() =>
                {
                    SendSmsHelper.SendSendSmsSaveLog(PhoneNumber, msg, SystemConst.SMSSOURCE);
                });
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.Sending);

            }
            catch (Exception)
            {
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.SendFailure);
            }
        }


        /// <summary>
        /// 请求动态验证码  (注册)
        /// 窦海超
        /// 2015年3月26日 17:46:08
        /// </summary>
        /// <param name="PhoneNumber">手机号码</param>
        /// <returns></returns>
        public SimpleResultModel CheckCode(string PhoneNumber)
        {
            if (!CommonValidator.IsValidPhoneNumber(PhoneNumber))  //验证电话号码合法性
            {
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.InvlidPhoneNumber);
            }
            //string randomCode = new Random().Next(1000).ToString("D4");  //生成短信验证码
            string randomCode = Helper.GenCode(6);
            var msg = string.Format(Config.SmsContentCheckCode, randomCode, SystemConst.MessageBusiness);  //获取提示用语信息
            try
            {
                if (businessDao.CheckBusinessExistPhone(PhoneNumber))  //判断该手机号是否已经注册过  .CheckBusinessExistPhone(PhoneNumber)
                    return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.AlreadyExists);
                else
                {
                    var redis = new ETS.NoSql.RedisCache.RedisCache();
                    redis.Add(RedissCacheKey.PostRegisterInfo_B + PhoneNumber, randomCode, new TimeSpan(0, 5, 0));
                    //CacheFactory.Instance.AddObject(PhoneNumber, randomCode);
                    //更新短信通道 
                    Task.Factory.StartNew(() =>
                    {
                        SendSmsHelper.SendSendSmsSaveLog(PhoneNumber, msg, SystemConst.SMSSOURCE);
                    });
                    return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.Sending);
                }
            }
            catch (Exception)
            {
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.SendFailure);
            }
        }
        /// <summary>
        /// 商户统计
        /// danny-20150326
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public Ets.Model.Common.BusinessCountManageList GetBusinessesCount(BusinessSearchCriteria criteria)
        {

            PageInfo<BusinessViewModel> pageinfo = businessDao.GetBusinessesCount<BusinessViewModel>(criteria);
            NewPagingResult pr = new NewPagingResult() { PageIndex = criteria.PagingRequest.PageIndex, PageSize = criteria.PagingRequest.PageSize, RecordCount = pageinfo.All, TotalCount = pageinfo.All };
            List<BusinessViewModel> list = pageinfo.Records.ToList();
            var businessCountManageList = new Ets.Model.Common.BusinessCountManageList(list, pr);
            return businessCountManageList;
        }



        /// <summary>
        /// 第三方平台取消订单 平扬-2015.3.27
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel<OrderCancelResultModel> NewOrderCancel(OrderCancelModel model)
        {
            LogHelper.LogWriter("第三方调用取消订单：", new { model = model });
            if (string.IsNullOrEmpty(model.OriginalOrderNo))   //订单号非空验证
                return ResultModel<OrderCancelResultModel>.Conclude(CancelOrderStatus.OrderEmpty);
            if (string.IsNullOrEmpty(model.OrderFrom.ToString()))   //订单来源非空验证
                return ResultModel<OrderCancelResultModel>.Conclude(CancelOrderStatus.OrderFromEmpty);
            order myOrder = businessDao.GetOrderByOrderNoAndOrderFrom(model.OriginalOrderNo, model.OrderFrom, model.OrderType);
            if (myOrder == null)//订单不存在
            {
                return ResultModel<OrderCancelResultModel>.Conclude(CancelOrderStatus.OrderIsNotExist);
            }

            bool b = businessDao.UpdateOrder(model.OriginalOrderNo, model.OrderFrom, OrderStatus.Status3);
            OrderOptionModel oom = new OrderOptionModel();
            oom.OptUserId = myOrder.businessId.Value;
            oom.OptUserName = "第三方";
            oom.OrderNo = myOrder.OrderNo;
            oom.OptLog = string.Format("第三方调用取消订单,订单来源{0}", model.OrderFrom);
            var reg = new OrderProvider().CancelOrderByOrderNo(oom);
            if (b)
            {
                return ResultModel<OrderCancelResultModel>.Conclude(CancelOrderStatus.Success);
            }
            return ResultModel<OrderCancelResultModel>.Conclude(CancelOrderStatus.NotCancelOrder, new OrderCancelResultModel { Remark = "取消失败" });
        }



        /// <summary>
        /// 获取用户状态信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public BussinessStatusModel GetUserStatus(int userid)
        {
            try
            {
                var UserInfo = businessDao.GetUserStatus(userid);
                return UserInfo;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return null;
        }

        /// <summary>
        /// 修改商户信息
        /// danny-20150417
        /// </summary>
        /// <param name="model"></param>
        /// <param name="orderOptionModel"></param>
        /// <returns></returns>
        public bool ModifyBusinessInfo(BusinessModel model, OrderOptionModel orderOptionModel)
        {
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            redis.Delete(string.Format(ETS.Const.RedissCacheKey.OtherBusinessIdInfo,  //清空之前的关系缓存
                ParseHelper.ToInt(model.oldGroupId), ParseHelper.ToInt(model.oldOriginalBusiId)));
            bool result = businessDao.ModifyBusinessInfo(model, orderOptionModel);
            if (result == true && ParseHelper.ToInt(model.GroupId) != 0)
            { //添加到缓存

                redis.Set(string.Format(ETS.Const.RedissCacheKey.OtherBusinessIdInfo,
                    ParseHelper.ToInt(model.GroupId), ParseHelper.ToInt(model.OriginalBusiId)), model.Id.ToString());
            }

            return result;
        }


        /// <summary>
        /// 请求语音验证码
        /// 窦海超
        /// 2015年4月20日 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SimpleResultModel VoiceCheckCode(Ets.Model.ParameterModel.Sms.SmsParaModel model)
        {
            if (!CommonValidator.IsValidPhoneNumber(model.PhoneNumber))
            {
                return SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.InvlidPhoneNumber);
            }
            string msg = string.Empty;
            string key = model.Stype == "0" ? RedissCacheKey.PostRegisterInfo_B + model.PhoneNumber : RedissCacheKey.CheckCodeFindPwd_B + model.PhoneNumber;
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            string obj = redis.Get<string>(key);
            if (string.IsNullOrEmpty(obj))
            {
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.CodeNotExists);
            }

            #region 判断该语音验证码是否存在
            string keycheck = key + "_voice";
            if (ParseHelper.ToInt(redis.Get<int>(keycheck)) == 1)
            {
                //如果该验证码存在直接提示成功
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.Sending);
            }
            redis.Add(keycheck, 1, new TimeSpan(0, 1, 0));
            #endregion

            string tempcode = obj.ToString().Aggregate("", (current, c) => current + (c.ToString() + ','));

            bool userStatus = businessDao.CheckBusinessExistPhone(model.PhoneNumber);
            if (model.Stype == "0")//注册
            {
                //判断该手机号是否已经注册过
                if (userStatus)
                {
                    return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.AlreadyExists);
                }
                msg = string.Format(ETS.Util.SupermanApiConfig.Instance.SmsContentCheckCodeVoice, tempcode, SystemConst.MessageClinenter);
            }
            else //修改密码
            {
                //判断该手机号是否已经注册过
                if (!userStatus)
                {
                    return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.NotExists);
                }
                msg = string.Format(ETS.Util.SupermanApiConfig.Instance.SmsContentCheckCodeFindPwdVoice, tempcode, SystemConst.MessageClinenter);
            }
            try
            {
                // 更新短信通道 
                Task.Factory.StartNew(() =>
                {
                    ETS.Sms.SendSmsHelper.SendSmsSaveLogNew(model.PhoneNumber, msg, SystemConst.SMSSOURCE);
                });
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.Sending);

            }
            catch (Exception)
            {
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.SendFailure);
            }
        }


        public int AddThirdBusiness(ParaModel<BusinessRegisterModel> paramodel)
        {
            var to = new BusinessModel();
            to.Province = paramodel.fields.B_Province;
            to.ProvinceCode = paramodel.fields.B_ProvinceCode.Trim();
            to.CityCode = paramodel.fields.B_CityCode;
            to.CityId = paramodel.fields.B_CityCode.Trim();
            to.City = paramodel.fields.B_City;

            to.districtId = paramodel.fields.B_AreaCode.Trim();
            to.district = paramodel.fields.B_Area;
            to.AreaCode = paramodel.fields.B_AreaCode.Trim();

            to.Address = paramodel.fields.Address.Trim();
            to.Address = to.Address.Replace((char)13, (char)0);
            to.Address = to.Address.Replace((char)10, (char)0);
            to.GroupId = paramodel.group;

            to.IDCard = paramodel.fields.B_IdCard;
            to.Password = paramodel.fields.B_Password;
            to.PhoneNo = paramodel.fields.PhoneNo.Trim();
            if (string.IsNullOrEmpty(paramodel.fields.PhoneNo2))
            {
                to.PhoneNo2 = paramodel.fields.PhoneNo;
            }
            else
            {
                to.PhoneNo2 = paramodel.fields.PhoneNo2;
            }
            to.Latitude = paramodel.fields.B_Latitude;
            to.Longitude = paramodel.fields.B_Longitude;
            to.Name = paramodel.fields.B_Name;
            to.OriginalBusiId = paramodel.fields.B_OriginalBusiId;
            to.CheckPicUrl = "/2015/05/01/01/201505011200_juwangke.jpg";  //图片给个默认的
            to.InsertTime = DateTime.Now;
            to.CommissionTypeId = 0;   //商户的佣金类型 
            to.DistribSubsidy = paramodel.fields.DistribSubsidy;  //商户外送费
            to.Status = (byte)BusinessStatus.Status0.GetHashCode();  //商户默认未审核
            to.IsAllowOverdraft = 1; //第三方商户是否允许透支，默认允许1,0不允许
            return InsertOtherBusiness(to);
        }

        /// <summary>
        /// 添加商户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertOtherBusiness(BusinessModel model)
        {
            return businessDao.InsertOtherBusiness(model);
        }

        /// <summary>
        /// 获取商户详情        
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="id">商户id</param>
        /// <returns></returns>
        public BusinessDM GetDetails(int id)
        {
            BusinessDM model = businessDao.GetDetails(id);
            model.HasMessage = new BusinessMessageDao().HasMessage(id);
            return model;
        }

        /// <summary>
        /// 获取门店发布任务需要的信息
        /// 门店,集团,任务
        /// add by 彭宜   20150714
        /// 修改人:胡灵波
        /// 2015年9月11日 17:15:31
        /// </summary>
        /// <param name="id">商户id</param>
        /// <param name="orderChildCount">子订单数量</param>
        /// <param name="amount">订单金额</param>
        /// <returns></returns>
        public BusiDistribSubsidyResultModel GetBusinessPushOrderInfo(int id, int orderChildCount, decimal amount)
        {
            var busiInfo = businessDao.GetSettlementRelevantById(id);
            if (busiInfo.ReceivableType == 1)
            {
                var result = new BusiDistribSubsidyResultModel
                        {
                            DistribSubsidy = busiInfo.DistribSubsidy,
                            GroupBusinessAmount = busiInfo.GroupBusinessAmount
                };
                result.OrderBalance = amount * busiInfo.BusinessCommission / 100 + (busiInfo.CommissionFixValue +
                                       busiInfo.DistribSubsidy ?? 0m) * orderChildCount;
                //剩余余额(商家余额 –当前任务结算金额)
                result.RemainBalance = busiInfo.BalancePrice - result.OrderBalance;

                return result;
            }
            else//
            {                
                decimal settleMoney = 0;
                BusinessSetpChargeChild bSetpChargeChild = businessSetpChargeChildDao.GetDetails(busiInfo.SetpChargeId);                
                if (amount > bSetpChargeChild.MaxValue)
                    settleMoney = bSetpChargeChild.ChargeValue;
                else
                    settleMoney = businessSetpChargeChildDao.GetChargeValue(busiInfo.SetpChargeId, amount);
                
                var result = new BusiDistribSubsidyResultModel
                {
                    DistribSubsidy = 0,
                    GroupBusinessAmount = busiInfo.GroupBusinessAmount
                };
                
                result.OrderBalance = settleMoney;
                //剩余余额(商家余额 –当前任务结算金额)3
                result.RemainBalance = busiInfo.BalancePrice - result.OrderBalance;
                //LogHelper.LogWriter("ResultModel<BusiOrderResultModel> GetBusinessPushOrderInfo()方法出错", new { obj = "时间：" + DateTime.Now.ToString() + "44444444444444444" });
                return result;
            }

            return null;            
        }
        /// <summary>
        /// 判断商户是否存在
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// </summary>
        /// <param name="id">商户Id</param>
        /// <returns></returns>
        public bool IsExist(int id)
        {
            return businessDao.IsExist(id);
        }
        /// <summary>
        /// 获取商户详细信息
        /// 胡灵波
        /// 2015年8月13日 16:42:01
        /// </summary>
        /// <param name="businessId">商户Id</param>
        /// <returns></returns>
        public BusinessDetailModel GetBusinessDetailById(string businessId)
        {
            return businessDao.GetBusinessDetailById(businessId);
        }
        /// <summary>
        /// 获取商户第三方绑定关系记录
        /// danny-20150602
        /// </summary>
        /// <param name="businessId">商户Id</param>
        /// <returns></returns>
        public IList<BusinessThirdRelationModel> GetBusinessThirdRelation(int businessId)
        {
            return businessDao.GetBusinessThirdRelation(businessId);
        }
        /// <summary>
        /// 修改商户详细信息
        /// danny-20150602
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DealResultInfo ModifyBusinessDetail(BusinessDetailModel model)
        {
            var dealResultInfo = new DealResultInfo
            {
                DealFlag = false
            };
            using (var tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (model.PushOrderType == 1)
                {  //快单模式
                    BusinessGroupModel groupModel = businessGroupDao.GetCurrenBusinessGroupByGroupId(model.BusinessGroupId);
                    if (groupModel.StrategyId != 4)//  所选的补贴策略不是基本佣金+网站补贴类型的策略
                    {
                        dealResultInfo.DealMsg = "选择快单模式时补贴策略只能选择基本佣金+网站补贴类型的策略！";
                        return dealResultInfo;
                    }
                }
                BusinessDetailModel old = businessDao.GetBusinessDetailById(model.Id.ToString());
                if ((old.BusinessCommission != model.BusinessCommission || old.CommissionFixValue != model.CommissionFixValue ||
                        old.DistribSubsidy != model.DistribSubsidy || old.BusinessGroupId != model.BusinessGroupId) && orderRegionDao.GetOrderCountInfoByBusinessId(model.Id))
                {
                    dealResultInfo.DealMsg = "当前商家有待接单订单尚未处理，不能修改商户结算（应收）和补贴设置（应付）";
                    return dealResultInfo;
                }
                if (!businessDao.ModifyBusinessDetail(model))
                {
                    dealResultInfo.DealMsg = "修改商户信息失败！";
                    return dealResultInfo;
                }
                tran.Complete();
                dealResultInfo.DealMsg = "修改商户信息成功！";
                dealResultInfo.DealFlag = true;
                return dealResultInfo;
            }
        }

        /// <summary>
        /// 修改商户配送公司绑定关系
        /// danny-20150706
        /// </summary>
        /// <param name="busiId"></param>
        /// <param name="deliveryCompanyList"></param>
        /// <param name="optName"></param>
        /// <returns></returns>
        public DealResultInfo ModifyBusinessExpress(int busiId, string deliveryCompanyList, string optName)
        {
            var dealResultInfo = new DealResultInfo
            {
                DealFlag = false
            };
            if (!string.IsNullOrEmpty(deliveryCompanyList))
            {

                var deliveryCompanyReg = deliveryCompanyList.Split(';');
                using (var tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
                {
                    foreach (var item in deliveryCompanyReg)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            var deliveryCompany = item.Split(',');
                            var berm = new BusinessExpressRelationModel
                            {
                                ExpressId = Convert.ToInt32(deliveryCompany[0]),
                                IsEnable = Convert.ToInt32(deliveryCompany[1]),
                                BusinessId = busiId,
                                OptName = optName
                            };
                            if (!businessDao.EditBusinessExpressRelation(berm))
                            {
                                dealResultInfo.DealMsg = "编辑商户物流公司配置信息失败！";
                                return dealResultInfo;
                            }
                        }
                    }
                    tran.Complete();
                    dealResultInfo.DealMsg = "编辑商户物流公司配置信息成功！";
                    dealResultInfo.DealFlag = true;
                    return dealResultInfo;
                }
            }
            dealResultInfo.DealMsg = "未获取到商户物流公司配置信息！";
            return dealResultInfo;
        }


        /// <summary>
        /// 获取商户绑定的骑士列表
        /// danny-20150608
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<BusinessClienterRelationModel> GetBusinessClienterRelationList(BusinessSearchCriteria criteria)
        {
            return businessDao.GetBusinessClienterRelationList<BusinessClienterRelationModel>(criteria);
        }

        /// <summary>
        /// 查询商户绑定骑士数量
        /// danny-20150608
        /// </summary>
        /// <param name="businessId">商户Id</param>
        /// <returns></returns>
        public int GetBusinessBindClienterQty(int businessId)
        {
            return businessDao.GetBusinessBindClienterQty(businessId);
        }

        /// <summary>
        /// 修改骑士绑定关系
        /// danny-20150608
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModifyClienterBind(ClienterBindOptionLogModel model)
        {
            var reg = false;
            using (var tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (businessDao.ModifyClienterBind(model))
                {
                    if (model.IsBind == 1)//绑定
                    {
                        if (businessDao.UpdateBusinessIsBind(model.BusinessId, 1))
                        {
                            if (businessDao.UpdateClienterIsBind(model.ClienterId, 1))
                            {
                                reg = true;
                                tran.Complete();
                            }
                        }
                    }
                    else//解绑
                    {
                        if (businessDao.GetClienterBindBusinessQty(model.ClienterId) > 0)
                        {
                            if (businessDao.GetBusinessBindClienterQty(model.BusinessId) > 0)
                            {
                                reg = true;
                                tran.Complete();
                            }
                            else
                            {
                                if (businessDao.UpdateBusinessIsBind(model.BusinessId, 0))
                                {
                                    reg = true;
                                    tran.Complete();
                                }
                            }
                        }
                        else
                        {
                            if (businessDao.UpdateClienterIsBind(model.ClienterId, 0))
                            {
                                if (businessDao.GetBusinessBindClienterQty(model.BusinessId) > 0)
                                {
                                    reg = true;
                                    tran.Complete();
                                }
                                else
                                {
                                    if (businessDao.UpdateBusinessIsBind(model.BusinessId, 0))
                                    {
                                        reg = true;
                                        tran.Complete();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return reg;
        }

        /// <summary>
        /// 删除骑士绑定关系
        /// danny-20150608
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool RemoveClienterBind(ClienterBindOptionLogModel model)
        {
            var reg = false;
            using (var tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (businessDao.RemoveClienterBind(model))
                {
                    if (businessDao.GetClienterBindBusinessQty(model.ClienterId) > 0)
                    {
                        if (businessDao.GetBusinessBindClienterQty(model.BusinessId) > 0)
                        {
                            reg = true;
                            tran.Complete();
                        }
                    }
                    else
                    {
                        if (businessDao.UpdateClienterIsBind(model.ClienterId, 0))
                        {
                            if (businessDao.GetBusinessBindClienterQty(model.BusinessId) > 0)
                            {
                                reg = true;
                                tran.Complete();
                            }
                            else
                            {
                                if (businessDao.UpdateBusinessIsBind(model.BusinessId, 0))
                                {
                                    reg = true;
                                    tran.Complete();
                                }
                            }
                        }
                    }
                }
            }
            return reg;
        }

        /// <summary>
        /// 添加骑士绑定关系
        /// danny-20150609
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddClienterBind(ClienterBindOptionLogModel model)
        {
            var reg = false;
            using (var tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (businessDao.AddClienterBind(model))
                {
                    if (businessDao.UpdateBusinessIsBind(model.BusinessId, 1))
                    {
                        if (businessDao.UpdateClienterIsBind(model.ClienterId, 1))
                        {
                            reg = true;
                            tran.Complete();
                        }
                    }

                }
            }
            return reg;
        }

        /// <summary>
        /// 验证是否有绑定关系
        /// danny-20150609
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CheckHaveBind(ClienterBindOptionLogModel model)
        {
            return businessDao.CheckHaveBind(model);
        }
        /// <summary>
        /// 查询商户结算列表（分页）
        /// danny-20150609
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<BusinessCommissionModel> GetBusinessCommissionOfPaging(Ets.Model.ParameterModel.Business.BusinessCommissionSearchCriteria criteria)
        {
            return businessDao.GetBusinessCommissionOfPaging<BusinessCommissionModel>(criteria);
        }

        /// <summary>
        /// 查询所有有效商户的总余额
        /// </summary>
        /// <UpdateBy>zhaohailong</UpdateBy>
        /// <UpdateTime>20150623</UpdateTime>
        /// <param name="pushCity"></param>
        /// <returns></returns>
        public decimal QueryAllBusinessTotalBalance()
        {
            return businessDao.QueryAllBusinessTotalBalance();
        }

        /// <summary>
        /// 获取商户操作记录
        /// wc
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns> 
        List<BusinessOptionLog> IBusinessProvider.GetBusinessOpLog(int businessId)
        {
            return businessDao.GetBusinessOpLog(businessId);
        }

        /// <summary>
        /// 获取商户和快递公司关系列表
        /// danny-20150706
        /// </summary>
        /// <param name="businessId">商户Id</param>
        /// <returns></returns>
        public IList<BusinessExpressRelation> GetBusinessExpressRelationList(int businessId)
        {
            return businessDao.GetBusinessExpressRelationList(businessId);
        }

        /// <summary>
        /// 插入骑士运行轨迹
        /// </summary>
        /// <UpdateBy>caoheyang</UpdateBy>
        /// <UpdateTime>20150519</UpdateTime>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> InsertLocaltion(BusinessPushLocaltionPM model)
        {
            try
            {
                long id = businessDao.InsertLocation(model.BusinessId, model.Latitude, model.Latitude, model.Platform);
                return ResultModel<object>.Conclude(SystemState.Success,
                    new { PushTime = GlobalConfigDao.GlobalConfigGet(0).BusinessUploadTimeInterval });
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                return ResultModel<object>.Conclude(SystemState.SystemError,
                      new { PushTime = 0 });
            }
        }


        /// <summary>
        /// 更新商家余额、可提现余额     
        /// 胡灵波
        /// 2015年8月13日 16:41:11
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="myOrderInfo"></param>
        public void UpdateBBalanceAndWithdraw(BusinessMoneyPM businessMoneyPM)
        {
            ///这里验证一下，如果不减商户余额时加IsRetainValue该属性就可以了，但是流水会正常记，当然流水会扣除，但账户流水总余额不会变
            if (businessMoneyPM.IsRetainValue == 0)
            {
                //更新商户余额、可提现
                businessDao.UpdateForWithdrawC(new UpdateForWithdrawPM()
                {
                    Id = businessMoneyPM.BusinessId,
                    Money = businessMoneyPM.Amount
                });
            }

            //更新商户余额流水          
            businessBalanceRecordDao.Insert(new BusinessBalanceRecord()
            {
                BusinessId = businessMoneyPM.BusinessId,
                Amount = businessMoneyPM.Amount,
                Status = businessMoneyPM.Status,
                RecordType = businessMoneyPM.RecordType,
                Operator = businessMoneyPM.Operator,
                WithwardId = businessMoneyPM.WithwardId,
                RelationNo = businessMoneyPM.RelationNo,
                Remark = businessMoneyPM.Remark
            });
        }

        public bool UpdateBusinessIsEnable(BusinessAuditModel bam)
        {
            return businessDao.UpdateBusinessIsEnable(bam);
        }

        /// <summary>
        /// 好厨师导入店铺
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string ImporBusinssExcel(List<BusinessModel> list, int groupid,int decid)
        {
            string existid = "";
            for (int i = 0; i < list.Count; i++)
            {
               
                var bussinesTemp = businessDao.CheckExistBusiness(ParseHelper.ToInt(list[i].OriginalBusiId), groupid);
                if (bussinesTemp == null)
                {
                    string storecodeInfo = new AreaProvider().GetOpenCode(new ParaAreaNameInfo()
                    {
                        ProvinceName = list[i].Province.Trim(),
                        CityName = list[i].City.Trim(),
                        AreaName = list[i].district.Trim()
                    });
                    string[] storeCodes = storecodeInfo.Split('_');
                    list[i].ProvinceCode = storeCodes[0];
                    list[i].CityCode = storeCodes[1];
                    list[i].districtId = storeCodes[2];

                    Tuple<decimal, decimal> la = BaiDuHelper.GeoCoder((list[i].Province ?? "") + (list[i].City ?? "") + (list[i].AreaCode ?? "") +
                                       (list[i].Address ?? ""));

                    OrderDao o = new OrderDao();
                    int bid= o.CreateToSqlAddBusiness(new CreatePM_OpenApi()
                    {
                        store_info = new Store()
                        {
                            store_id = ParseHelper.ToInt(list[i].OriginalBusiId),
                            store_name = list[i].Name,
                            group = groupid,
                            id_card = "",
                            phone = list[i].PhoneNo,
                            phone2 = list[i].PhoneNo,
                            address = list[i].Address,
                            province_code = list[i].ProvinceCode,
                            city_code = list[i].CityCode,
                            area_code = list[i].districtId,
                            province = list[i].Province,
                            city = list[i].City,
                            area = list[i].district,
                            longitude = la.Item1,
                            latitude = la.Item2,
                            businesscommission = 0
                        },
                        CommissionType = 2,
                        CommissionFixValue = 5,
                        BusinessGroupId = 0
                    });

                    var berm = new BusinessExpressRelationModel
                    {
                        ExpressId = Convert.ToInt32(decid),
                        IsEnable = Convert.ToInt32(1),
                        BusinessId = bid,
                        OptName = "admin"
                    };
                    businessDao.EditBusinessExpressRelation(berm);
    }
                else
                {
                    existid = existid + list[i].OriginalBusiId;
                }

            }
            return existid;
        }
    }
}