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
namespace Ets.Service.Provider.User
{

    /// <summary>
    /// 商户业务逻辑接口实现类  add by caoheyang 20150311
    /// </summary>
    public class BusinessProvider : IBusinessProvider
    {
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
                model.ReceviceName = from.ReceviceName;
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
        public bool SetCommission(int id, decimal price)
        {
            return dao.setCommission(id, price);
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

            BusiRegisterResultModel resultModel = new BusiRegisterResultModel()
            {
                userId = dao.InsertBusiness(model)
            };
            return ResultModel<BusiRegisterResultModel>.Conclude(CustomerRegisterStatusEnum.Success, resultModel);// CustomerRegisterStatusEnum.Success;//默认是成功状态

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
        public BusinessManage GetBusinesses(BusinessSearchCriteria criteria)
        {
            var pagedQuery = new BusinessManage();
            PageInfo<BusListResultModel> pageinfo = dao.GetBusinesses<BusListResultModel>(criteria);
            NewPagingResult pr = new NewPagingResult() { PageIndex = criteria.PagingRequest.PageIndex, PageSize = criteria.PagingRequest.PageSize, RecordCount = pageinfo.All, TotalCount = pageinfo.All };
            List<BusListResultModel> list = pageinfo.Records.ToList();
            var businesslists = new BusinessManageList(list, pr);
            pagedQuery.businessManageList = businesslists;
            return pagedQuery;
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
    }
}