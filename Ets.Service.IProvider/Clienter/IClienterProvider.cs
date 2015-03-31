using System.Collections.Generic;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Clienter;

namespace Ets.Service.IProvider.Clienter
{
    public interface IClienterProvider
    {
        List<order> GetOrdersNoLoginLatest(ClientOrderSearchCriteria criteria);

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
        /// <returns></returns>
        string FinishOrder(int userId, string orderNo);
        /// <summary>
        /// 骑士完成订单 wc
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        string FinishOrder_C(int userId, string orderNo);
    }
}
