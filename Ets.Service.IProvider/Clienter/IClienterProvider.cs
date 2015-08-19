using System.Collections.Generic;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Clienter;
using ETS.Data.PageData;
using Ets.Model.DomainModel.Business;
using Ets.Model.ParameterModel.Order;
using Ets.Model.DomainModel.Order;

namespace Ets.Service.IProvider.Clienter
{
    public interface IClienterProvider
    {
        /// <summary>
        /// 骑士上下班功能 add by caoheyang 20150312
        /// </summary>
        /// <param name="paraModel"></param>
        /// <returns></returns>
        ETS.Enums.ChangeWorkStatusEnum ChangeWorkStatus(Ets.Model.ParameterModel.Clienter.ChangeWorkStatusPM paraModel);


        /// <summary>
        /// 获取我的任务   根据状态判断是已完成任务还是我的任务
        /// </summary>
        /// <returns></returns>
        IList<ClientOrderResultModel> GetMyOrders(ClientOrderSearchCriteria clientOrderModel);

        /// <summary>
        ///  C端登录
        ///  窦海超
        ///  2015年3月17日 15:13:28
        /// </summary>
        /// <param name="model">用户名称，用户密码</param>
        /// <returns>用户信息</returns>
        ResultModel<ClienterLoginResultModel> PostLogin_C(LoginCPM model);

        /// <summary>
        /// 获取当前配送员的流水信息
        /// 窦海超
        /// 2015年3月20日 17:12:11
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        ClienterRecordsListModel WtihdrawRecords(int UserId);

        /// <summary>
        ///  修改密码
        ///  窦海超
        ///  2015年3月23日 18:45:54
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel<ClienterModifyPwdResultModel> PostForgetPwd_C(Ets.Model.DataModel.Clienter.ModifyPwdInfoModel model);
        /// <summary>
        /// 判断 骑士端 手机号 是否注册过
        /// wc
        /// </summary>
        /// <param name="PhoneNo"></param>
        /// <returns></returns>
        bool CheckClienterExistPhone(string PhoneNo);
        /// <summary>
        /// 根据骑士Id 验证该骑士是否有资格
        /// wc
        /// </summary>
        /// <param name="clienterId"></param>
        /// <returns></returns>
        bool HaveQualification(int clienterId);

        /// <summary>
        /// 骑士注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel<ClientRegisterResultModel> PostRegisterInfo_C(ClientRegisterInfoModel model);
        /// <summary>
        /// 抢单 平扬 2015.3.30
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        bool RushOrder(int userId, string orderNo);
        /// <summary>
        /// 根据骑士Id判断骑士是否存在
        /// danny-20150530
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool CheckClienterExistById(int Id);
        /// <summary>
        /// 更新骑士照片信息
        /// danny-10150330
        /// </summary>
        /// <param name="clienter"></param>
        /// <returns></returns>
        bool UpdateClientPicInfo(ClienterModel clienter);
        /// <summary>
        /// 根据电话获取当前用户的信息
        /// danny-20150330
        /// </summary>
        /// <param name="PhoneNo"></param>
        /// <returns></returns>
        ClienterModel GetUserInfoByUserPhoneNo(string PhoneNo);
        /// <summary>
        /// 根据用户ID更新密码
        /// danny-20150330
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="UserPwd"></param>
        /// <returns></returns>
        bool UpdateClienterPwdByUserId(int UserId, string UserPwd);

        /// <summary>
        /// 根据用户ID获取用户状态
        /// 窦海超-20150331
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        ClienterStatusModel GetUserStatus(int UserId);
        /// <summary>
        /// 超人 完成订单
        /// wc 
        /// 修改人：胡灵波
        /// 2015年8月13日 18:13:55
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        /// <param name="pickupCode">取货码 可空</param>
        /// <returns></returns>
        FinishOrderResultModel FinishOrder(OrderCompleteModel parModel);

        ClienterModel GetUserInfoByUserId(int UserId);
        /// <summary>
        /// 骑士配送统计
        /// danny-20150408
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        PageInfo<BusinessesDistributionModel> GetClienterDistributionStatisticalInfo(OrderSearchCriteria criteria);
        /// <summary>
        /// 骑士门店抢单统计
        /// danny-20150408
        /// </summary>
        /// <returns></returns>
        IList<BusinessesDistributionModel> GetClienteStorerGrabStatisticalInfo();
        /// <summary>
        /// 骑士门店抢单统计
        /// 胡灵波-20150424
        /// </summary>
        /// <param name="daysAgo">几天前</param>
        /// <returns></returns>
        IList<BusinessesDistributionModel> GetClienteStorerGrabStatisticalInfo(int daysAgo);
        /// <summary>
        /// 骑士门店抢单统计,过一个月后删除该代码
        /// danny-20150408
        /// </summary>
        /// <returns></returns>
        IList<BusinessesDistributionModelOld> GetClienteStorerGrabStatisticalInfoOld(int NewCount);
        /// <summary>
        /// 上传小票
        /// wc
        /// </summary>
        /// <param name="uploadReceiptModel"></param>
        /// <returns></returns>
        OrderOther UpdateClientReceiptPicInfo(UploadReceiptModel uploadReceiptModel);
        ///// <summary>
        ///// 删除小票
        ///// wc
        ///// </summary>
        ///// <param name="uploadReceiptModel"></param>
        ///// <returns></returns>
        //OrderOther DeleteReceipt(UploadReceiptModel uploadReceiptModel);
        /// <summary>
        /// 根据订单Id获取小票信息
        /// </summary>
        /// <param name="uploadReceiptModel"></param>
        /// <returns></returns>
        OrderOther GetReceipt(int orderId);

        /// <summary>
        ///  C端抢单
        ///  窦海超
        ///  2015年5月6日 20:40:56
        /// </summary>
        /// <param name="userId">骑士ID</param>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        ResultModel<RushOrderResultModel> RushOrder_C(int userId, string orderNo);

        /// <summary>
        /// 获取骑士详情  
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="id">骑士Id</param>
        /// <returns></returns>
        ClienterDM GetDetails(int id);

        /// <summary>
        /// 判断骑士是否存在 
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="id">骑士Id</param>
        /// <returns></returns>
        bool IsExist(int id);

        /// <summary>
        /// 根据订单Id和子订单Id获取信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderChildId"></param>
        /// <returns></returns>
        List<OrderChildForTicket> GetOrderChildInfo(int orderId, int orderChildId); 
        /// <summary>
        /// 获取骑士详细信息
        /// danny-20150513
        /// </summary>
        /// <param name="clienterId">骑士Id</param>
        /// <returns></returns>
        ClienterDetailModel GetClienterDetailById(string clienterId);

        /// <summary>
        /// 获取骑士用户名
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        string GetName(string phoneNo);
        /// <summary>
        /// 获取骑士列表
        /// danny-20150608
        /// </summary>
        /// <param name="model"></param>
        IList<ClienterListModel> GetClienterList(ClienterListModel model);
        /// <summary>
        /// 获取骑士Id
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150609</UpdateTime>
        int GetId(string phoneNo, string trueName);
		/// <summary>
        /// 查询骑士列表
        /// danny-20150609
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        PageInfo<ClienterListModel> GetClienterList(ClienterSearchCriteria criteria);

        /// <summary>
        /// 修改骑士详细信息
        /// danny-20150707
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        DealResultInfo ModifyClienterDetail(ClienterDetailModel model);

        /// <summary>
        /// 更新骑士余额
        /// 胡灵波
        /// 2015年8月13日 09:53:33
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="myOrderInfo"></param>
        void UpdateCAccountBalance(ClienterMoneyPM clienterMoneyPM);

        /// <summary>
        /// 更新骑士可提现余额
        /// 胡灵波
        /// 2015年8月13日 10:38:31
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="myOrderInfo"></param>
        void UpdateCAllowWithdrawPrice(ClienterMoneyPM clienterMoneyPM);

        /// <summary>
        /// 更新骑士余额、可提现余额      
        /// 胡灵波
        /// 2015年8月13日 18:11:23
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="myOrderInfo"></param>
        void UpdateCBalanceAndWithdraw(ClienterMoneyPM clienterMoneyPM);   

    }
}
