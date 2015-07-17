using Ets.Model.Common;
using Ets.Model.Common.YeePay;
using Ets.Model.DataModel.Business;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Finance;
using ETS.Data.PageData;
using System.Collections.Generic;

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
        ResultModel<object> WithdrawB(WithdrawBPM withdrawBpm);

        /// <summary>
        /// 商户提现功能 后台
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150626</UpdateTime>
        /// <param name="withdrawBBackPM"></param>
        /// <returns></returns>
        ResultModel<object> WithdrawB(WithdrawBBackPM withdrawBBackPM);              

        /// <summary>
        /// 根据申请单Id获取商家提现申请单
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <returns></returns>
        BusinessWithdrawFormModel GetBusinessWithdrawListById(string withwardId);
        /// <summary>
        /// 获取商户提款单操作日志
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
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

        /// <summary>
        /// 易宝打款失败回调处理逻辑 
        /// add by caoheyang  20150716
        /// </summary>
        /// <param name="model"></param>
        ///  <param name="callback"></param>
        /// <returns></returns>
        bool BusinessWithdrawPayFailed(BusinessWithdrawLogModel model, CashTransferCallback callback);
 		/// <summary>
        /// 获取商户提款收支记录列表
        /// danny-20150512
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IList<BusinessBalanceRecord> GetBusinessBalanceRecordList(BusinessBalanceRecordSerchCriteria criteria);
         /// <summary>
        /// 获取要导出的商户提现申请单
        /// danny-20150512
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IList<BusinessWithdrawFormModel> GetBusinessWithdrawForExport(BusinessWithdrawSearchCriteria criteria);
        /// <summary>
        /// 获取要导出的商户提款收支记录列表
        /// danny-20150512
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IList<BusinessBalanceRecordModel> GetBusinessBalanceRecordListForExport(BusinessBalanceRecordSerchCriteria criteria);
        /// <summary>
        /// 生成excel文件
        /// 导出字段：商户名称、电话、开户行、账户名、卡号、提款金额
        /// danny-20150512
        /// </summary>
        /// <returns></returns>
        string CreateBusinessWithdrawFormExcel(List<BusinessWithdrawFormModel> list);
        /// <summary>
        /// 生成excel文件
        /// 导出字段：商户名称、电话、开户行、账户名、卡号、提款金额
        /// danny-20150512
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        string CreateBusinessBalanceRecordExcel(List<BusinessBalanceRecordModel> list);

        /// <summary>
        /// 商户充值增加商家余额可提现和插入商家余额流水
        /// danny-20150526
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
         bool BusinessRecharge(BusinessOptionLog model);
         /// <summary>
         /// 获取商户提款收支记录列表分页版
         /// danny-20150604
         /// </summary>
         /// <param name="criteria"></param>
         /// <returns></returns>
         PageInfo<BusinessBalanceRecord> GetBusinessBalanceRecordListOfPaging(BusinessBalanceRecordSerchCriteria criteria);

        /// <summary>
        /// 根据单号查询充值详情
        /// </summary>
         /// <UpdateBy>zhaohailong</UpdateBy>
         /// <UpdateTime>20150624</UpdateTime>
        /// <param name="orderNo"></param>
        /// <returns></returns>
         BusinessRechargeDetail GetBusinessRechargeDetailByNo(string orderNo);

        /// <summary>
        /// 商户提现申请单确认打款调用易宝接口
        /// danny-20150716
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        DealResultInfo BusinessWithdrawPaying(BusinessWithdrawLog model);
    }
}
