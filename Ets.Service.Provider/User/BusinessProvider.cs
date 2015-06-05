using System.Data;
using System.Text;
using ETS.Const;
using Ets.Dao.User;
using Ets.Model.Common;
using Ets.Model.DomainModel.Bussiness;
using Ets.Service.IProvider.User;
using ETS.Data.PageData;
using System;
using System.Collections.Generic;
using CalculateCommon;
using Ets.Model.ParameterModel.Bussiness;
using System.Linq;
using ETS.Enums;
using Ets.Model.DataModel.Bussiness;
using ETS.Util;
using ETS.Cacheing;
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
namespace Ets.Service.Provider.User
{

    /// <summary>
    /// 商户业务逻辑接口实现类  add by caoheyang 20150311
    /// </summary>
    public class BusinessProvider : IBusinessProvider
    {
        readonly Ets.Service.IProvider.Common.IAreaProvider iAreaProvider = new Ets.Service.Provider.Common.AreaProvider();
        BusinessDao dao = new BusinessDao();
        /// <summary>
        /// app端商户获取订单   add by caoheyang 20150311
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IList<Ets.Model.DomainModel.Bussiness.BusiGetOrderModel> GetOrdersApp(Ets.Model.ParameterModel.Bussiness.BussOrderParaModelApp paraModel)
        {
            PageInfo<BusiOrderSqlModel> pageinfo = dao.GetOrdersAppToSql<BusiOrderSqlModel>(paraModel);
            IList<BusiOrderSqlModel> list = pageinfo.Records;

            List<Ets.Model.DomainModel.Bussiness.BusiGetOrderModel> listOrder = new List<Ets.Model.DomainModel.Bussiness.BusiGetOrderModel>();
            foreach (BusiOrderSqlModel from in list)
            {
                Ets.Model.DomainModel.Bussiness.BusiGetOrderModel model = new Ets.Model.DomainModel.Bussiness.BusiGetOrderModel();
                model.ActualDoneDate = from.ActualDoneDate;
                model.Amount = from.Amount;
                model.IsPay = from.IsPay;
                model.OrderNo = from.OrderNo;
                model.PickUpAddress = from.PickUpAddress;
                if (from.PubDate != null)
                {
                    model.PubDate = from.PubDate;
                }
                model.PickUpName = from.BusinessName;
                model.ReceviceAddress = from.ReceviceAddress;
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
                //修改该接口时需要把orderfrom的id join出name来，本次为了改一个聚网客居然要单独上线。。。。
                if (from.OrderFrom == 0)
                {
                    model.OrderFromName = "B端";
                }
                else if (from.OrderFrom == 1)
                {
                    model.OrderFromName = "聚网客";
                }
                else if (from.OrderFrom == 2)
                {
                    model.OrderFromName = "万达";
                }
                else if (from.OrderFrom == 3)
                {
                    model.OrderFromName = "全时";
                }
                else if (from.OrderFrom == 4)
                {
                    model.OrderFromName = "美团";
                }
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
        public ResultInfo<IList<BusinessCommissionModel>> GetBusinessCommission(DateTime t1, DateTime t2, string name, string phoneno, int groupid, string BusinessCity, string authorityCityNameListStr)
        {
            var result = new ResultInfo<IList<BusinessCommissionModel>> { Data = null, Result = false, Message = "" };
            try
            {
                if (t1 > t2)
                {
                    result.Result = false;
                    result.Message = "开始时间不能大于结束时间";
                    return result;
                }
                var list = dao.GetBusinessCommission(t1, t2, name, phoneno, groupid, BusinessCity, authorityCityNameListStr);
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
                bool res = dao.setCommission(id, price, waisongfei);
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
                bool res = dao.ModifyCommission(busListResultModel);
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
            strBuilder.AppendLine(string.Format("<td>{0}</td>", paraModel.T1.ToShortDateString()));
            strBuilder.AppendLine(string.Format("<td>{0}</td>", paraModel.T2.ToShortDateString()));
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
        public ResultModel<BusiRegisterResultModel> PostRegisterInfo_B(Model.ParameterModel.Bussiness.RegisterInfoModel model)
        {
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            var code = redis.Get<string>("PostRegisterInfo_B_" + model.phoneNo);
            Enum returnEnum = null;
            if (string.IsNullOrEmpty(model.phoneNo))
            {
                returnEnum = CustomerRegisterStatusEnum.PhoneNumberEmpty; //手机号非空验证
            }
            else if (string.IsNullOrEmpty(model.passWord))
            {
                returnEnum = CustomerRegisterStatusEnum.PasswordEmpty;//密码非空验证 
            }
            else if (string.IsNullOrEmpty(code) || code != model.verifyCode) //验证码正确性验证
            {
                returnEnum = CustomerRegisterStatusEnum.IncorrectCheckCode; //判断验证法录入是否正确
            }
            else if (dao.CheckBusinessExistPhone(model.phoneNo))
            {
                returnEnum = CustomerRegisterStatusEnum.PhoneNumberRegistered;//判断该手机号是否已经注册过
            }
            else if (string.IsNullOrEmpty(model.city) || string.IsNullOrEmpty(model.CityId)) //城市以及城市编码非空验证
                returnEnum = CustomerRegisterStatusEnum.cityIdEmpty;
            if (returnEnum != null)
            {
                return ResultModel<BusiRegisterResultModel>.Conclude(returnEnum);
            }

            //转换 编码
            try
            {
                if (!string.IsNullOrWhiteSpace(model.city))
                {
                    Model.DomainModel.Area.AreaModelTranslate areaModel = iAreaProvider.GetNationalAreaInfo(new Model.DomainModel.Area.AreaModelTranslate() { Name = model.city.Trim(), JiBie = 2 });
                    if (areaModel != null)
                    {
                        model.CityId = areaModel.NationalCode.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("商户注册异常转换区域：", new { ex = ex });
            }

            BusiRegisterResultModel resultModel = new BusiRegisterResultModel()
            {
                userId = dao.InsertBusiness(model)
            };
            return ResultModel<BusiRegisterResultModel>.Conclude(CustomerRegisterStatusEnum.Success, resultModel);// CustomerRegisterStatusEnum.Success;//默认是成功状态

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
                returnEnum = CustomerRegisterStatusEnum.PhoneNumberEmpty; //手机号非空验证
            else if (string.IsNullOrEmpty(model.passWord))
                returnEnum = CustomerRegisterStatusEnum.PasswordEmpty;//密码非空验证  

            else if (dao.CheckBusinessExistPhone(model.phoneNo))
                returnEnum = CustomerRegisterStatusEnum.PhoneNumberRegistered;//判断该手机号是否已经注册过

            else if (string.IsNullOrEmpty(model.city) || string.IsNullOrEmpty(model.CityId)) //城市以及城市编码非空验证
                returnEnum = CustomerRegisterStatusEnum.cityIdEmpty;
            if (returnEnum != null)
            {
                return ResultModel<BusiRegisterResultModel>.Conclude(returnEnum);
            }
            model.passWord = ETS.Security.MD5.Encrypt(model.passWord);
            BusiRegisterResultModel resultModel = new BusiRegisterResultModel()
            {
                userId = dao.addBusiness(model)
            };
            return ResultModel<BusiRegisterResultModel>.Conclude(CustomerRegisterStatusEnum.Success, resultModel);// CustomerRegisterStatusEnum.Success;//默认是成功状态

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
            Business busi = dao.CheckExistBusiness(model.B_OriginalBusiId, model.B_GroupId);
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
            var _province = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModelTranslate() { Name = model.B_Province, JiBie = 1 });
            if (_province != null)
            {
                model.B_ProvinceCode = _province.NationalCode.ToString();
            }
            //转换市 
            var _city = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModelTranslate() { Name = model.B_City, JiBie = 2 });
            if (_city != null)
            {
                model.B_CityCode = _city.NationalCode.ToString();
            }
            //转换区
            var _area = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModelTranslate() { Name = model.B_Area, JiBie = 3 });
            if (_area != null)
            {
                model.B_AreaCode = _area.NationalCode.ToString();
            }
            #endregion
            var business = NewRegisterInfoModelTranslator.Instance.Translate(model);
            business.Status = ConstValues.BUSINESS_AUDITPASS;
            int businessid = dao.InsertOtherBusiness(business);
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

        public Business CheckExistBusiness(int originalId, int groupId)
        {
            return dao.CheckExistBusiness(originalId, groupId);
        }

        /// <summary>
        /// B端登录
        /// 窦海超
        /// 2015年3月16日 16:11:59
        /// </summary>
        /// <param name="model">用户名，密码对象</param>
        /// <returns>登录后返回实体对象</returns>
        public ResultModel<BusiLoginResultModel> PostLogin_B(LoginModel model)
        {
            try
            {
                BusiLoginResultModel resultMode = new BusiLoginResultModel();
                DataTable dt = dao.LoginSql(model);
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
                return ResultModel<BusiLoginResultModel>.Conclude(LoginModelStatus.Success, resultMode);
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
            return dao.GetBusiness(busiId);
        }

        /// <summary>
        /// 根据商户Id获取商户信息
        /// </summary>
        /// <param name="busiId"></param>
        /// <returns></returns>
        public BusListResultModel GetBusiness(int originalBusiId, int groupId)
        {
            return dao.GetBusiness(originalBusiId, groupId);
        }
        /// <summary>
        /// 获取商户信息
        /// danny-20150316
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<BusListResultModel> GetBusinesses(BusinessSearchCriteria criteria)
        {
            PageInfo<BusListResultModel> pageinfo = dao.GetBusinesses<BusListResultModel>(criteria);
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
            PageInfo<BusinessesDistributionModel> pageinfo = dao.GetBusinessesDistributionStatisticalInfo<BusinessesDistributionModel>(criteria);
            return pageinfo;
        }
        /// <summary>
        /// 更新审核状态
        /// danny-20150317
        /// </summary>
        /// <param name="id"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public bool UpdateAuditStatus(int id, EnumStatusType enumStatusType)
        {
            ETS.NoSql.RedisCache.RedisCache redis = new ETS.NoSql.RedisCache.RedisCache();
            string cacheKey = string.Format(RedissCacheKey.BusinessProvider_GetUserStatus, id);
            redis.Delete(cacheKey);
            return dao.UpdateAuditStatus(id, enumStatusType);
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
            return dao.GetBussinessByCityInfo(criteria);
        }

        /// <summary>
        /// 修改商户密码
        /// 窦海超
        /// 2015年3月23日 19:11:54
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel<BusiModifyPwdResultModel> PostForgetPwd_B(BusiForgetPwdInfoModel model)
        {
            if (string.IsNullOrEmpty(model.password))
            {
                //密码非空验证
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.NewPwdEmpty);
            }
            if (string.IsNullOrEmpty(model.checkCode)) //验证码非空验证
            {
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.checkCodeIsEmpty);
            }
            // var code = CacheFactory.Instance[model.phoneNumber];
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            var code = redis.Get<string>("CheckCodeFindPwd_" + model.phoneNumber);
            if (string.IsNullOrEmpty(code) || code != model.checkCode) //验证码正确性验证
            { return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.checkCodeWrong); }

            BusinessDao businessDao = new BusinessDao();
            var business = businessDao.GetBusinessByPhoneNo(model.phoneNumber);
            if (business == null) //用户是否存在
            {
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.ClienterIsNotExist);
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
            return dao.GetOrderCountDataSql(BusinessId);
        }
        /// <summary>
        /// 验证 商户 手机号 是否注册
        /// wc
        /// </summary>
        /// <param name="PhoneNo"></param>
        /// <returns></returns>
        //public bool CheckBusinessExistPhone(string PhoneNo)
        //{
        //    return dao.CheckBusinessExistPhone(PhoneNo);
        //}
        /// <summary>
        /// 判断该 商户是否有资格 
        /// wc
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public bool HaveQualification(int businessId)
        {
            return dao.HaveQualification(businessId);
        }
        /// <summary>
        /// 根据集团id获取集团名称
        /// danny-20150324
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public string GetGroupNameById(int groupId)
        {
            return dao.GetGroupNameById(groupId);
        }
        /// <summary>
        /// 获取所有可用的集团信息数据
        /// danny-20150324
        /// </summary>
        /// <returns></returns>
        public IList<GroupModel> GetGroups()
        {
            return dao.GetGroups();
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
            return dao.ModifyWaiMaiPrice(businessId, waiSongFei);
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
            Business business = TranslateBusiness(businessModel);

            var busi = dao.GetBusiness(businessModel.userId); //查询商户信息
            if (busi.Status == ConstValues.BUSINESS_NOADDRESS)  //如果商户的状态 为未审核未添加地址，则修改商户状态为 未审核
            {
                business.Status = ConstValues.BUSINESS_NOAUDIT;
            }
            int upResult = dao.UpdateBusinessAddressInfo(business);
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
                return ResultModel<BusiModifyResultModelDM>.Conclude(UploadIconStatus.InvalidUserId);
            }
            if (string.IsNullOrWhiteSpace(model.phoneNo))
            {
                return ResultModel<BusiModifyResultModelDM>.Conclude(BusiAddAddressStatus.PhoneNumberEmpty);
            }
            if (string.IsNullOrWhiteSpace(model.Address))
            {
                return ResultModel<BusiModifyResultModelDM>.Conclude(BusiAddAddressStatus.AddressEmpty);
            }
            if (string.IsNullOrWhiteSpace(model.businessName))
            {
                return ResultModel<BusiModifyResultModelDM>.Conclude(BusiAddAddressStatus.BusinessNameEmpty);
            }
            UpdateBusinessInfoBPM business = UpdateBusinessInfoBTranslate(model);
            var busi = dao.GetBusiness(model.userId); //查询商户信息
            if (busi == null)
            {
                return ResultModel<BusiModifyResultModelDM>.Conclude(UploadIconStatus.InvalidUserId);
            }
            if (busi.Status == ConstValues.BUSINESS_NOADDRESS)  //如果商户的状态 为未审核未添加地址，则修改商户状态为 未审核
            {
                business.Status = ConstValues.BUSINESS_NOAUDIT;
            }

            int upResult = dao.UpdateBusinessInfoB(business);
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            string cacheKey = string.Format(RedissCacheKey.BusinessProvider_GetUserStatus, model.userId);
            redis.Delete(cacheKey);
            return ResultModel<BusiModifyResultModelDM>.Conclude(upResult == -1 ? BusiAddAddressStatus.UpdateFailed : BusiAddAddressStatus.Success,
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
            to.Address = businessModel.Address.Trim(); //地址
            to.Name = businessModel.businessName.Trim(); //商户名称
            to.Landline = businessModel.landLine; //座机
            to.PhoneNo2 = businessModel.phoneNo.Trim(); //手机号2
            to.City = businessModel.City.Trim(); //市
            to.district = businessModel.districtName.Trim();  //区域名称
            to.Province = businessModel.Province.Trim();  //省份名称
            //修改地址转换 区域编码
            if (!string.IsNullOrWhiteSpace(businessModel.districtName))
            {
                AreaModelTranslate areaModel = iAreaProvider.GetNationalAreaInfo(new AreaModelTranslate() { Name = businessModel.districtName.Trim(), JiBie = 3 });
                to.districtId = areaModel != null ? areaModel.NationalCode.ToString() : "";
                to.AreaCode = areaModel != null ? areaModel.NationalCode.ToString() : "";
            }
            //修改地址转换 市编码
            if (!string.IsNullOrWhiteSpace(businessModel.City))
            {
                AreaModelTranslate areaModel = iAreaProvider.GetNationalAreaInfo(new AreaModelTranslate() { Name = businessModel.City.Trim(), JiBie = 2 });
                to.CityId = areaModel != null ? areaModel.NationalCode.ToString() : "";
                to.CityCode = areaModel != null ? areaModel.NationalCode.ToString() : "";
            }
            //修改地址转换 省份编码
            if (!string.IsNullOrWhiteSpace(businessModel.Province))
            {
                AreaModelTranslate areaModel = iAreaProvider.GetNationalAreaInfo(new AreaModelTranslate() { Name = businessModel.Province.Trim(), JiBie = 1 });
                to.ProvinceCode = areaModel != null ? areaModel.NationalCode.ToString() : "";
            }

            to.Longitude = businessModel.longitude; //经度
            to.Latitude = businessModel.latitude; //纬度 
            to.Status = ConstValues.BUSINESS_NOAUDIT;  //用户状态待审核
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
            int upResult = dao.UpdateBusinessPicInfo(busiId, picName);
            return upResult;

        }
        /// <summary>
        /// 将入口参数转换为对应的 数据库 Business 实体
        /// wc 注意这里有商户状态的转换
        /// </summary>
        /// <param name="businessModel"></param>
        /// <returns></returns>
        private Business TranslateBusiness(BusiAddAddressInfoModel businessModel)
        {
            var to = new Business();
            to.Id = businessModel.userId;
            to.Address = businessModel.Address.Trim();
            to.Name = businessModel.businessName.Trim();
            to.Landline = businessModel.landLine;
            to.PhoneNo2 = businessModel.phoneNo.Trim();
            //修改地址转换 区域编码
            if (!string.IsNullOrWhiteSpace(businessModel.districtName))
            {
                Ets.Model.DomainModel.Area.AreaModelTranslate areaModel = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModelTranslate() { Name = businessModel.districtName.Trim(), JiBie = 3 });
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
            to.Status = ConstValues.BUSINESS_NOAUDIT;
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
            if (!dao.CheckBusinessExistPhone(PhoneNumber))
            {
                //账号不存在 
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.NotExists);
            }
            string randomCode = new Random().Next(100000).ToString("D6");
            var msg = string.Format(Config.SmsContentFindPassword, randomCode, Ets.Model.Common.ConstValues.MessageBusiness);
            try
            {
                var redis = new ETS.NoSql.RedisCache.RedisCache();
                redis.Add("CheckCodeFindPwd_" + PhoneNumber, randomCode, DateTime.Now.AddHours(1));
                // CacheFactory.Instance.AddObject(PhoneNumber, randomCode);
                // 更新短信通道 
                Task.Factory.StartNew(() =>
                {
                    SendSmsHelper.SendSendSmsSaveLog(PhoneNumber, msg, Ets.Model.Common.ConstValues.SMSSOURCE);
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
            string randomCode = new Random().Next(100000).ToString("D6");  //生成短信验证码
            var msg = string.Format(Config.SmsContentCheckCode, randomCode, Ets.Model.Common.ConstValues.MessageBusiness);  //获取提示用语信息
            try
            {
                if (dao.CheckBusinessExistPhone(PhoneNumber))  //判断该手机号是否已经注册过  .CheckBusinessExistPhone(PhoneNumber)
                    return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.AlreadyExists);
                else
                {
                    var redis = new ETS.NoSql.RedisCache.RedisCache();
                    redis.Add("PostRegisterInfo_B_" + PhoneNumber, randomCode, DateTime.Now.AddHours(1));
                    //CacheFactory.Instance.AddObject(PhoneNumber, randomCode);
                    //更新短信通道 
                    Task.Factory.StartNew(() =>
                    {
                        SendSmsHelper.SendSendSmsSaveLog(PhoneNumber, msg, Ets.Model.Common.ConstValues.SMSSOURCE);
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

            PageInfo<BusinessViewModel> pageinfo = dao.GetBusinessesCount<BusinessViewModel>(criteria);
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
            order myOrder = dao.GetOrderByOrderNoAndOrderFrom(model.OriginalOrderNo, model.OrderFrom, model.OrderType);
            if (myOrder == null)//订单不存在
            {
                return ResultModel<OrderCancelResultModel>.Conclude(CancelOrderStatus.OrderIsNotExist);
            }

            bool b = dao.UpdateOrder(model.OriginalOrderNo, model.OrderFrom, OrderStatus.订单已取消);
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
                //ETS.NoSql.RedisCache.RedisCache redis = new ETS.NoSql.RedisCache.RedisCache();
                //string cacheKey = string.Format(RedissCacheKey.BusinessProvider_GetUserStatus, userid);
                //var cacheValue = redis.Get<string>(cacheKey);
                //if (!string.IsNullOrEmpty(cacheValue))
                //{
                //    return Letao.Util.JsonHelper.ToObject<BussinessStatusModel>(cacheValue);
                //}
                var UserInfo = dao.GetUserStatus(userid);
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
        /// 修改商户信息
        /// danny-20150417
        /// </summary>
        /// <param name="model"></param>
        /// <param name="orderOptionModel"></param>
        /// <returns></returns>
        public bool ModifyBusinessInfo(Business model, OrderOptionModel orderOptionModel)
        {
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            redis.Delete(string.Format(ETS.Const.RedissCacheKey.OtherBusinessIdInfo,  //清空之前的关系缓存
                ParseHelper.ToInt(model.oldGroupId), ParseHelper.ToInt(model.oldOriginalBusiId)));
            bool result = dao.ModifyBusinessInfo(model, orderOptionModel);
            if (result == true && ParseHelper.ToInt(model.GroupId) != 0)
            { //添加到缓存

                redis.Set(string.Format(ETS.Const.RedissCacheKey.OtherBusinessIdInfo,
                    ParseHelper.ToInt(model.GroupId), ParseHelper.ToInt(model.OriginalBusiId)), model.Id.ToString());
            }

            return result;
        }


        /// <summary>
        /// 请求语音验证码
        /// 平扬
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
            var randomCode = new Random().Next(100000).ToString("D6");
            string msg = string.Empty;
            string key = "";
            string tempcode = randomCode.Aggregate("", (current, c) => current + (c.ToString() + ','));

            if (model.Stype == "0")//注册
            {
                if (dao.CheckBusinessExistPhone(model.PhoneNumber))  //判断该手机号是否已经注册过  .CheckBusinessExistPhone(PhoneNumber)
                    return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.AlreadyExists);
                key = RedissCacheKey.PostRegisterInfoSoundCode_B + model.PhoneNumber;
                msg = string.Format(ETS.Util.SupermanApiConfig.Instance.SmsContentCheckCodeVoice, tempcode, ConstValues.MessageClinenter);
            }
            else //修改密码
            {
                key = RedissCacheKey.PostForgetPwdSoundCode_B + model.PhoneNumber;
                msg = string.Format(ETS.Util.SupermanApiConfig.Instance.SmsContentCheckCodeFindPwdVoice, tempcode, ConstValues.MessageClinenter);
            }
            try
            {
                var redis = new ETS.NoSql.RedisCache.RedisCache();
                redis.Add(key, randomCode, DateTime.Now.AddHours(1));

                // 更新短信通道 
                Task.Factory.StartNew(() =>
                {
                    ETS.Sms.SendSmsHelper.SendSmsSaveLogNew(model.PhoneNumber, msg, ConstValues.SMSSOURCE);
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
            var to = new Business();
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
            to.Status = ConstValues.BUSINESS_NOAUDIT;  //商户默认未审核

            return InsertOtherBusiness(to);
        }

        /// <summary>
        /// 添加商户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertOtherBusiness(Business model)
        {
            return dao.InsertOtherBusiness(model);
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
            return dao.GetDetails(id);
        }

        /// <summary>
        /// 获取商户外送费        
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="id">商户id</param>
        /// <returns></returns>
        public BusinessInfo GetDistribSubsidy(int id)
        {
            return dao.GetDistribSubsidy(id);
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
            return dao.IsExist(id);
        }
        /// <summary>
        /// 获取商户详细信息
        /// </summary>
        /// <param name="businessId">商户Id</param>
        /// <returns></returns>
        public BusinessDetailModel GetBusinessDetailById(string businessId)
        {
            return dao.GetBusinessDetailById(businessId);
        }
        /// <summary>
        /// 获取商户第三方绑定关系记录
        /// danny-20150602
        /// </summary>
        /// <param name="businessId">商户Id</param>
        /// <returns></returns>
        public IList<BusinessThirdRelationModel> GetBusinessThirdRelation(int businessId)
        {
            return dao.GetBusinessThirdRelation(businessId);
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
                if (!dao.ModifyBusinessDetail(model))
                {
                    dealResultInfo.DealMsg = "修改商户信息失败！";
                    return dealResultInfo;
                }
                //if (!string.IsNullOrWhiteSpace(model.ThirdBindListStr))
                //{
                //    dao.DeleteBusinessThirdRelation(model.Id);
                //    var thirdBindModel = model.ThirdBindListStr.TrimEnd(';').Split(';');
                //    foreach (var item in thirdBindModel)
                //    {
                //        var businessThirdRelationModel = new BusinessThirdRelationModel
                //        {
                //            OriginalBusiId = ParseHelper.ToInt(item.Split(',')[0]),
                //            BusinessId = model.Id,
                //            GroupId = ParseHelper.ToInt(item.Split(',')[1]),
                //            GroupName = item.Split(',')[2],
                //            AuditStatus = ParseHelper.ToInt(item.Split(',')[3])
                //        };
                //        if (!dao.AddBusinessThirdRelation(businessThirdRelationModel))
                //        {
                //            dealResultInfo.DealMsg = "插入商户第三方绑定关系失败！";
                //            return dealResultInfo;
                //        }
                //    }
                //}
                tran.Complete();
                dealResultInfo.DealMsg = "修改商户信息成功！";
                dealResultInfo.DealFlag = true;
                return dealResultInfo;
            }
        }
    }
}