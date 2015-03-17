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
    }
}
