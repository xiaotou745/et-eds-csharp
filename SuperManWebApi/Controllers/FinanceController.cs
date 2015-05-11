using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Finance;
using Ets.Service.IProvider.Finance;
using Ets.Service.Provider.Finance;

namespace SuperManWebApi.Controllers
{
    /// <summary>
    /// 财务相关功能点   涉及 B、C端
    /// </summary>
    [ExecuteTimeLog]
    public class FinanceController : ApiController
    {
        #region 声明对象
        private readonly IClienterFinanceProvider _clienterFinanceProvider = new ClienterFinanceProvider();
        private readonly IBusinessFinanceProvider _iBusinessFinanceProvider = new BusinessFinanceProvider();
        
        #endregion

        #region C端

        /// <summary>
        /// 骑士提现功能 add by caoheyang 20150509
        /// </summary>
        /// <param name="withdrawCpm">参数实体</param>
        /// <returns></returns>
        [HttpPost]
        public SimpleResultModel WithdrawC(WithdrawCPM withdrawCpm)
        {
            return _clienterFinanceProvider.WithdrawC(withdrawCpm);
        }

        /// <summary>
        /// 骑士绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardBindCpm">参数实体</param>
        /// <returns></returns>
        [HttpPost]
        public SimpleResultModel CardBindC(CardBindCPM cardBindCpm)
        {
            return _clienterFinanceProvider.CardBindC(cardBindCpm);
        }

        /// <summary>
        /// 骑士修改绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardModifyCpm">参数实体</param>
        /// <returns></returns>
        [HttpPost]
        public SimpleResultModel CardModifyC(CardModifyCPM cardModifyCpm)
        {
            return _clienterFinanceProvider.CardModifyC(cardModifyCpm);
        }

        #endregion

        #region B端

        /// <summary>
        /// 商户提现功能 add by caoheyang 20150509
        /// </summary>
        /// <param name="withdrawBpm">参数实体</param>
        /// <returns></returns>
        [HttpPost]
        public SimpleResultModel WithdrawB(WithdrawBPM withdrawBpm)
        {
            return _iBusinessFinanceProvider.WithdrawB(withdrawBpm);
        }

        /// <summary>
        /// 商户绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardBindBpm">参数实体</param>
        /// <returns></returns>
        [HttpPost]
        public SimpleResultModel CardBindB(CardBindBPM cardBindBpm)
        {
            return _iBusinessFinanceProvider.CardBindB(cardBindBpm);
        }

        /// <summary>
        /// 商户修改绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardModifyBpm">参数实体</param>
        /// <returns></returns>
        [HttpPost]
        public SimpleResultModel CardModifyB(CardModifyBPM cardModifyBpm)
        {
            return _iBusinessFinanceProvider.CardModifyB(cardModifyBpm);
        }

        #endregion
    }
}
