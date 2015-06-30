using Ets.Model.Common;

namespace Ets.Service.IProvider.Finance
{
    /// <summary>
    ///  商家余额流水表
    /// <UpdateBy>hulingbo</UpdateBy>
    /// <UpdateTime>20150629</UpdateTime>
    /// </summary>
    public interface IBusinessBalanceRecordProvider
    {
        /// <summary>
        ///  商户交易流水API add by caoheyang 20150512
        /// </summary>
        /// <returns></returns>
        ResultModel<object> GetRecords(int businessId);
    }
}
