
using Ets.Model.Common;
using Ets.Model.ParameterModel.Finance;
namespace Ets.Service.IProvider.Finance
{
    /// <summary>
    ///   商家金融账号表 
    /// <UpdateBy>hulingbo</UpdateBy>
    /// <UpdateTime>20150629</UpdateTime>
    /// </summary>
    public interface IBusinessFinanceAccountProvider
    {
        int GetBFinanceAccountId(int businessId);

         /// <summary>
        /// 商户绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardBindBpm">参数实体</param>
        /// <returns></returns>
        ResultModel<object> CardBindB(CardBindBPM cardBindBpm);


        /// <summary>
        /// 商户修改绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardModifyBpm">参数实体</param>
        /// <returns></returns>
        ResultModel<object> CardModifyB(CardModifyBPM cardModifyBpm);
    }

}
