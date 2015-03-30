using System.Data;
using System.Text;
using Ets.Dao.User;
using Ets.Model.Common;
using Ets.Model.DomainModel.Bussiness;
using Ets.Service.IProvider.User;
using ETS.Data.PageData;
using System;
using System.Collections.Generic;
using CalculateCommon;
using Ets.Model.ParameterModel.Bussiness;
using Ets.Model.DataModel.Order;
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
        public IList<BusiGetOrderModel> GetOrdersApp(Ets.Model.ParameterModel.Bussiness.BussOrderParaModelApp paraModel)
        {
            PageInfo<BusiOrderSqlModel> pageinfo = dao.GetOrdersAppToSql<BusiOrderSqlModel>(paraModel);
            IList<BusiOrderSqlModel> list = pageinfo.Records;

            List<BusiGetOrderModel> listOrder = new List<BusiGetOrderModel>();
            foreach (BusiOrderSqlModel from in list)
            {
                BusiGetOrderModel model = new BusiGetOrderModel();
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
                model.ReceviceName = from.ReceviceName == null ? "匿名" : from.ReceviceName.Trim();
                model.RecevicePhoneNo = from.RecevicePhoneNo;
                model.Remark = from.Remark;
                model.Status = from.Status;
                model.superManName = from.SuperManName;
                model.superManPhone = from.SuperManPhone;
                if (from.BusinessId > 0 && from.ReceviceLongitude != null && from.ReceviceLatitude != null)
                {
                    var d1 = new Degree(from.Longitude.Value, from.Latitude.Value);
                    var d2 = new Degree(from.ReceviceLongitude.Value, from.ReceviceLatitude.Value);
                    model.distanceB2R = CoordDispose.GetDistanceGoogle(d1, d2);
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
        /// <returns></returns>
        public ResultInfo<IList<BusinessCommissionModel>> GetBusinessCommission(DateTime t1, DateTime t2, string name, int groupid)
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
                var list = dao.GetBusinessCommission(t1, t2, name, groupid);
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
        /// 设置结算比例2015.3.12 平扬
        /// </summary>
        /// <returns></returns>
        public bool SetCommission(int id, decimal price,decimal waisongfei)
        {
            return dao.setCommission(id, price, waisongfei);
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
            Enum returnEnum = null;
            if (string.IsNullOrEmpty(model.phoneNo))
                returnEnum = CustomerRegisterStatusEnum.PhoneNumberEmpty; //手机号非空验证
            else if (string.IsNullOrEmpty(model.passWord))
                returnEnum = CustomerRegisterStatusEnum.PasswordEmpty;//密码非空验证

            else if (model.verifyCode != ETS.Cacheing.CacheFactory.Get(model.phoneNo))
                returnEnum = CustomerRegisterStatusEnum.IncorrectCheckCode; //判断验证法录入是否正确
            else if (dao.CheckBusinessExistPhone(model.phoneNo))
                returnEnum = CustomerRegisterStatusEnum.PhoneNumberRegistered;//判断该手机号是否已经注册过

            else if (string.IsNullOrEmpty(model.city) || string.IsNullOrEmpty(model.CityId)) //城市以及城市编码非空验证
                returnEnum = CustomerRegisterStatusEnum.cityIdEmpty;
            if (returnEnum != null)
            {
                return ResultModel<BusiRegisterResultModel>.Conclude(returnEnum);
            }

            //转换 编码
            if (!string.IsNullOrWhiteSpace(model.city))
            {
                Model.DomainModel.Area.AreaModel areaModel = iAreaProvider.GetNationalAreaInfo(new Model.DomainModel.Area.AreaModel() { Name = model.city.Trim(), JiBie = 2 });
                if (areaModel != null)
                {
                    model.city = areaModel.Name;
                    model.CityId = areaModel.Code.ToString();
                }
            }
          
            BusiRegisterResultModel resultModel = new BusiRegisterResultModel()
            {
                userId = dao.InsertBusiness(model)
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
            if (dao.CheckExistBusiness(model.B_OriginalBusiId, model.B_GroupId))
                return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.OriginalBusiIdRepeat);

            if (string.IsNullOrWhiteSpace(model.B_City) || string.IsNullOrWhiteSpace(model.B_CityCode.ToString())) //城市以及城市编码非空验证
                return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.cityIdEmpty);
            if (string.IsNullOrEmpty(model.B_Name.Trim())) //商户名称
                return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.BusiNameEmpty);
            if (string.IsNullOrWhiteSpace(model.Address) || string.IsNullOrWhiteSpace(model.B_Province) || string.IsNullOrWhiteSpace(model.B_City) || string.IsNullOrWhiteSpace(model.B_Area) || string.IsNullOrWhiteSpace(model.B_AreaCode) || string.IsNullOrWhiteSpace(model.B_CityCode) || string.IsNullOrWhiteSpace(model.B_ProvinceCode))  //商户地址 省市区 不能为空
                return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.BusiAddressEmpty);
            if (model.CommissionTypeId == 0)
            {
                return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.BusiAddressEmpty);
            }
            model.B_Password = MD5Helper.MD5(string.IsNullOrEmpty(model.B_Password) ? "abc123" : model.B_Password);
            //转换省
            var _province = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModel() { Name= model.B_Province, JiBie= 1 });
            if (_province != null)
            {
                model.B_Province = _province.Name;
                model.B_ProvinceCode = _province.Code.ToString();
            }
            //转换市
            var _city = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModel() { Name = model.B_City, JiBie = 2 });
            if (_city != null)
            {
                model.B_City = _city.Name;
                model.B_CityCode = _city.Code.ToString();
            }
            //转换区
            var _area = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModel() { Name = model.B_Area, JiBie = 3 });
            if (_area != null)
            {
                model.B_Area = _area.Name;
                model.B_AreaCode = _area.Code.ToString();
            } 
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


        /// <summary>
        /// B端登录
        /// 窦海超
        /// 2015年3月16日 16:11:59
        /// </summary>
        /// <param name="model">用户名，密码对象</param>
        /// <returns>登录后返回实体对象</returns>
        public ResultModel<BusiLoginResultModel> PostLogin_B(Model.ParameterModel.Bussiness.LoginModel model)
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
                resultMode.userId = ParseHelper.ToInt(row["userId"]);
                resultMode.status = Convert.ToByte(row["status"]);
                resultMode.city = row["city"].ToString();
                resultMode.Address = row["Address"].ToString();
                resultMode.districtId = row["districtId"].ToString();
                resultMode.district = row["district"].ToString();
                resultMode.Landline = row["Landline"].ToString();
                resultMode.Name = row["Name"].ToString();
                resultMode.cityId = row["cityId"].ToString();
                resultMode.phoneNo = row["PhoneNo2"] == null ? row["PhoneNo"].ToString() : row["PhoneNo2"].ToString();
                resultMode.DistribSubsidy = row["DistribSubsidy"] == null ? 0 : ParseHelper.ToDecimal(row["DistribSubsidy"]);
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
        /// 更新审核状态
        /// danny-20150317
        /// </summary>
        /// <param name="id"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public bool UpdateAuditStatus(int id, EnumStatusType enumStatusType)
        {
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
            if (string.IsNullOrEmpty(model.password))  //密码非空验证
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.NewPwdEmpty);
            if (string.IsNullOrEmpty(model.checkCode)) //验证码非空验证
            {
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.checkCodeIsEmpty);
            }
            var code = CacheFactory.Instance[model.phoneNumber];

            if (code == null || code.ToString() != model.checkCode) //验证码正确性验证
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.checkCodeWrong);

            BusinessDao businessDao = new BusinessDao();
            var business = businessDao.GetBusinessByPhoneNo(model.phoneNumber);
            if (business == null)  //用户是否存在
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.ClienterIsNotExist);
            if (businessDao.UpdateBusinessPwdSql(business.Id, model.password))
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.Success);
            else
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.FailedModifyPwd);
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

            return upResult;
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
            to.districtId = businessModel.districtId;
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
            var randomCode = new Random().Next(100000).ToString("D6");
            var msg = string.Format(Config.SmsContentFindPassword, randomCode, Ets.Model.Common.ConstValues.MessageBusiness);
            try
            {
                CacheFactory.Instance.AddObject(PhoneNumber, randomCode);
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
            var randomCode = new Random().Next(100000).ToString("D6");  //生成短信验证码
            var msg = string.Format(Config.SmsContentCheckCode, randomCode, Ets.Model.Common.ConstValues.MessageBusiness);  //获取提示用语信息
            try
            {
                if (dao.CheckBusinessExistPhone(PhoneNumber))  //判断该手机号是否已经注册过  .CheckBusinessExistPhone(PhoneNumber)
                    return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.AlreadyExists);
                else
                {
                    //SupermanApiCaching.Instance.Add(PhoneNumber, randomCode);
                    CacheFactory.Instance.AddObject(PhoneNumber, randomCode);
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
            bool isorder = dao.GetOrderByOrderNoAndOrderFrom(model.OriginalOrderNo, model.OrderFrom, model.OrderType);
            if (!isorder)//订单不存在
            {
                return ResultModel<OrderCancelResultModel>.Conclude(CancelOrderStatus.OrderIsNotExist);
            } 
            bool b = dao.UpdateOrder(model.OriginalOrderNo, model.OrderFrom, OrderStatus.订单已取消); 
            if (b)
            {
                return ResultModel<OrderCancelResultModel>.Conclude(CancelOrderStatus.Success);
            }
            return ResultModel<OrderCancelResultModel>.Conclude(CancelOrderStatus.NotCancelOrder, new OrderCancelResultModel { Remark = "取消失败" });
        }
    }
}