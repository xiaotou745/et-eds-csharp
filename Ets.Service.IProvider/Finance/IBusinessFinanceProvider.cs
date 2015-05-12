using Ets.Model.Common;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Finance;
using ETS.Data.PageData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Finance
{
    public interface IBusinessFinanceProvider
    {
        /// <summary>
        /// 根据参数获取商家提现申请单列表
        /// danny-20150509
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        PageInfo<BusinessWithdrawFormModel> GetBusinessWithdrawList(BusinessWithdrawSearchCriteria criteria);

        /// <summary>
        /// 商户提现功能 add by caoheyang 20150509
        /// </summary>
        /// <param name="withdrawBpm">参数实体</param>
        /// <returns></returns>
        SimpleResultModel WithdrawB(WithdrawBPM withdrawBpm);

        /// <summary>
        /// 商户绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardBindBpm">参数实体</param>
        /// <returns></returns>
        SimpleResultModel CardBindB(CardBindBPM cardBindBpm);


        /// <summary>
        /// 商户修改绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardModifyBpm">参数实体</param>
        /// <returns></returns>
        SimpleResultModel CardModifyB(CardModifyBPM cardModifyBpm);
        /// <summary>
        /// 根据申请单Id获取商家提现申请单
        /// danny-20150511
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        BusinessWithdrawFormModel GetBusinessWithdrawListById(string withwardId);
        /// <summary>
        /// 获取商户提款单操作日志
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId"></param>
        /// <returns></returns>
        IList<BusinessWithdrawLog> GetBusinessWithdrawOptionLog(string withwardId);
        /// <summary>
        /// 审核商户提现申请单
        /// danny-20150511
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool BusinessWithdrawAudit(BusinessWithdrawLog model);
        /// <summary>
        /// 商户提现申请单确认打款
        /// danny-20150511
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool BusinessWithdrawPayOk(BusinessWithdrawLog model);
        /// <summary>
        /// 商户提现申请单审核拒绝
        /// danny-20150511
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool BusinessWithdrawAuditRefuse(BusinessWithdrawLogModel model);
        /// <summary>
        /// 商户提现申请单打款失败
        /// danny-20150511
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool BusinessWithdrawPayFailed(BusinessWithdrawLogModel model);
    }
}
