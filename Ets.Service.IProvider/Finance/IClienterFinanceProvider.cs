using System.Collections.Generic;
using Ets.Model.Common;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Finance;
using ETS.Data.PageData;

namespace Ets.Service.IProvider.Finance
{
    /// <summary>
    /// 骑士财务业务逻辑 add by caoheyang 20150509
    /// </summary>
    public interface IClienterFinanceProvider
    {
        /// <summary>
        /// 骑士提现功能 add by caoheyang 20150509
        /// </summary>
        /// <param name="withdrawCpm">参数实体</param>
        /// <returns></returns>
        ResultModel<object> WithdrawC(WithdrawCPM withdrawCpm);

        /// <summary>
        /// 骑士绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardBindCpm">参数实体</param>
        /// <returns></returns>
        ResultModel<object> CardBindC(CardBindCPM cardBindCpm);


        /// <summary>
        /// 骑士修改绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardModifyCpm">参数实体</param>
        /// <returns></returns>
        ResultModel<object> CardModifyC(CardModifyCPM cardModifyCpm);


        /// <summary>
        /// 骑士交易流水API add by caoheyang 20150511
        /// </summary> 
        /// <param name="clienterId">骑士id</param>
        /// <returns></returns>
        ResultModel<object> GetRecords(int clienterId);

        /// <summary>
        /// 根据参数获取骑士提现申请单列表
        /// danny-20150513
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        PageInfo<ClienterWithdrawFormModel> GetClienterWithdrawList(ClienterWithdrawSearchCriteria criteria);
        /// <summary>
        /// 根据申请单Id获取骑士提现申请单
        /// danny-20150513
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <returns></returns>
        ClienterWithdrawFormModel GetClienterWithdrawListById(string withwardId);
        /// <summary>
        /// 获取骑士提款单操作日志
        /// danny-20150513
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <returns></returns>
        IList<ClienterWithdrawLog> GetClienterWithdrawOptionLog(string withwardId);
        /// <summary>
        /// 审核骑士提现申请单
        /// danny-20150513
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool ClienterWithdrawAudit(ClienterWithdrawLog model);
        /// <summary>
        /// 骑士提现申请单确认打款
        /// danny-20150513
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool ClienterWithdrawPayOk(ClienterWithdrawLog model);
        /// <summary>
        /// 骑士提现申请单审核拒绝
        /// danny-20150513
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool ClienterWithdrawAuditRefuse(ClienterWithdrawLogModel model);
        /// <summary>
        /// 骑士提现申请单打款失败
        /// danny-20150513
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool ClienterWithdrawPayFailed(ClienterWithdrawLogModel model);
        /// <summary>
        /// 获取骑士提款收支记录列表
        /// danny-20150513
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IList<ClienterBalanceRecord> GetClienterBalanceRecordList(ClienterBalanceRecordSerchCriteria criteria);
        /// <summary>
        /// 获取要导出的骑士提现申请单
        /// danny-20150513
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IList<ClienterWithdrawFormModel> GetClienterWithdrawForExport(ClienterWithdrawSearchCriteria criteria);
        /// <summary>
        /// 获取要导出的骑士提款收支记录列表
        /// danny-20150513
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IList<ClienterBalanceRecordModel> GetClienterBalanceRecordListForExport(ClienterBalanceRecordSerchCriteria criteria);
        /// <summary>
        /// 生成excel文件
        /// 导出字段：骑士姓名、电话、开户行、账户名、卡号、提款金额
        /// danny-20150513
        /// </summary>
        /// <returns></returns>
        string CreateClienterWithdrawFormExcel(List<ClienterWithdrawFormModel> list);
        /// <summary>
        /// 生成excel文件
        /// 导出字段：骑士姓名、电话、开户行、账户名、卡号、提款金额
        /// danny-20150513
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        string CreateClienterBalanceRecordExcel(List<ClienterBalanceRecordModel> list);
    }
}
