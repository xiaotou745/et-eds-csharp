using ETS.Data.PageData;
using System.Collections.Generic;
using Ets.Model.ParameterModel.Bussiness;
using System;
using Ets.Model.Common;
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.DataModel.Bussiness;
using ETS.Enums;
using System.Data;
using Ets.Model.DataModel.Group;

namespace Ets.Service.IProvider.User
{
    /// <summary>
    /// 商户业务逻辑接口 add by caoheyang 20150311
    /// </summary>
    public interface IBusinessProvider
    {
        /// <summary>
        /// 商户获取订单   add by caoheyang 20150311
        /// </summary>
        /// <returns></returns>
        IList<BusiGetOrderModel> GetOrdersApp(Ets.Model.ParameterModel.Bussiness.BussOrderParaModelApp paraModel);

        /// <summary>
        /// 生成商户结算excel文件2015.3.12 平扬
        /// </summary>
        /// <returns></returns>
        string CreateExcel(BusinessCommissionModel paraModel);

        /// <summary>
        /// 设置结算比例2015.3.12 平扬
        /// </summary>
        /// <returns></returns>
        bool SetCommission(int id, decimal price);

        /// <summary>
        /// 商户结算列表--2015.3.12 平扬
        /// </summary>
        /// <param name="t1">开始计算日期</param>
        /// <param name="t2">结束日期</param>
        /// <param name="name">商户姓名</param>
        /// <returns></returns>
        ResultInfo<IList<BusinessCommissionModel>> GetBusinessCommission(DateTime t1, DateTime t2, string name, int groupid);

        /// <summary>
        /// B端注册 
        /// 窦海超
        /// 2015年3月16日 10:19:45
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel<BusiRegisterResultModel> PostRegisterInfo_B(RegisterInfoModel model);

        /// <summary>
        /// B端登录
        /// 窦海超
        /// 2015年3月16日 16:11:59
        /// </summary>
        /// <param name="model">用户名，密码对象</param>
        /// <returns>登录后返回实体对象</returns>
        ResultModel<BusiLoginResultModel> PostLogin_B(LoginModel model);
        /// <summary>
        /// 根据商户Id获取商户信息  
        /// </summary>
        /// <param name="busiId"></param>
        /// <returns></returns>
        BusListResultModel GetBusiness(int busiId);
        /// <summary>
        /// 获取商户信息
        /// danny-20150316
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        PageInfo<BusListResultModel> GetBusinesses(BusinessSearchCriteria criteria);
        /// <summary>
        /// 更新审核状态
        /// danny-20150317
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enumStatusType"></param>
        /// <returns></returns>
        bool UpdateAuditStatus(int id, EnumStatusType enumStatusType);
        /// <summary>
        /// 根据城市信息查询当前城市下该集团的所有商户信息
        ///  danny-20150317
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IList<BusListResultModel> GetBussinessByCityInfo(BusinessSearchCriteria criteria);

        /// <summary>
        /// 修改商户密码
        /// 窦海超
        /// 2015年3月23日 19:11:54
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel<BusiModifyPwdResultModel> PostForgetPwd_B(BusiForgetPwdInfoModel model);

        /// <summary>
        /// 获取商户端的统计数量
        /// 窦海超
        /// 2015年3月23日 20:19:02
        /// </summary>
        /// <param name="BusinessId">商户ID</param>
        /// <returns></returns>
        BusiOrderCountResultModel GetOrderCountData(int BusinessId);
        ///// <summary>
        ///// 验证商户手机号 是否 注册
        ///// wc
        ///// </summary>
        ///// <param name="PhoneNo"></param>
        ///// <returns></returns>
        //bool CheckBusinessExistPhone(string PhoneNo);
        /// <summary>
        /// 判断该 商户是否有资格 
        /// wc
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        bool HaveQualification(int businessId);
        /// <summary>
        /// 根据集团id获取集团名称
        /// danny-20150324
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        string GetGroupNameById(int groupId);
        /// <summary>
        /// 获取所有可用的集团信息数据
        /// danny-20150324
        /// </summary>
        /// <returns></returns>
        IList<GroupModel> GetGroups();
        /// <summary>
        /// 商户修改外送费
        /// wc
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="waiSongFei"></param>
        /// <returns></returns>
        int ModifyWaiMaiPrice(int businessId, decimal waiSongFei);
        /// <summary>
        /// 修改商户地址信息
        /// wc
        /// </summary>
        /// <param name="businessModel"></param>
        /// <returns></returns>
        int UpdateBusinessAddressInfo(BusiAddAddressInfoModel businessModel);

        /// <summary>
        /// 请求动态验证码  (找回密码)
        /// 窦海超
        /// 2015年3月26日 17:16:02
        /// </summary>
        /// <param name="PhoneNumber">手机号码</param>
        /// <returns></returns>
        Ets.Model.Common.SimpleResultModel CheckCodeFindPwd(string PhoneNumber);

        /// <summary>
        /// 请求动态验证码  (注册)
        /// 窦海超
        /// 2015年3月26日 17:46:08
        /// </summary>
        /// <param name="PhoneNumber">手机号码</param>
        /// <returns></returns>
        Ets.Model.Common.SimpleResultModel CheckCode(string PhoneNumber);
        /// <summary>
        /// 商户统计
        /// danny-20150326
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        PageInfo<BusinessViewModel> GetBusinessesCount(BusinessSearchCriteria criteria);
    }
}
