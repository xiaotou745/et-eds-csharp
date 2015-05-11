using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Finance;

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
        SimpleResultModel WithdrawC(WithdrawCPM withdrawCpm);

        /// <summary>
        /// 骑士绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardBindCpm">参数实体</param>
        /// <returns></returns>
        SimpleResultModel CardBindC(CardBindCPM cardBindCpm);


        /// <summary>
        /// 骑士修改绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardModifyCpm">参数实体</param>
        /// <returns></returns>
        SimpleResultModel CardModifyC(CardModifyCPM cardModifyCpm);
    }
}
