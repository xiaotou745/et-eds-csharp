using System.Collections.Generic;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Clienter;
using ETS.Data.PageData;
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.ParameterModel.Order;

namespace Ets.Service.IProvider.Clienter
{
    public interface IClienterProvider
    {
        /// <summary>
        /// 更新添加骑士佣金金额
        /// wc
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="myOrderInfo"></param>
        void UpdateClienterAccount(int userId, OrderListModel myOrderInfo);
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
        ResultModel<ClienterLoginResultModel> PostLogin_C(LoginModel model);

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
        /// 平扬-20150331
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        ClienterStatusModel GetUserStatus(int UserId, double version);
        /// <summary>
        /// 超人 完成订单
        /// wc
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        /// <param name="pickupCode">取货码 可空</param>
        /// <returns></returns>
        string FinishOrder(int userId, string orderNo, string pickupCode = null);

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
        /// <summary>
        /// 删除小票
        /// wc
        /// </summary>
        /// <param name="uploadReceiptModel"></param>
        /// <returns></returns>
        OrderOther DeleteReceipt(UploadReceiptModel uploadReceiptModel);
        /// <summary>
        /// 根据订单Id获取小票信息
        /// </summary>
        /// <param name="uploadReceiptModel"></param>
        /// <returns></returns>
        OrderOther GetReceipt(int orderId);
        /// <summary>
        /// 根据订单Id获取小票信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        order GetOrderInfoByOrderId(int orderId);

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
        /// hulingbo 20150511
        /// </summary>
        /// <param name="id">骑士Id</param>
        /// <returns></returns>
        ClienterDM GetDetails(int id);

        /// <summary>
        /// 判断骑士是否存在
        /// hulingbo 20150511
        /// </summary>
        /// <param name="id">骑士Id</param>
        /// <returns></returns>
        bool IsExist(int id);

        /// <summary>
        /// 根据订单Id和子订单Id获取信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderChildId"></param>
        /// <returns></returns>
        OrderChild GetOrderChildInfo(int orderId, int orderChildId);
    
    }
}
