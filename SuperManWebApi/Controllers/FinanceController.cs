#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.DomainModel.Area;
using Ets.Model.ParameterModel.Finance;
using Ets.Service.IProvider.Common;
using Ets.Service.IProvider.Finance;
using Ets.Service.Provider.Common;
using Ets.Service.Provider.Finance;
using SuperManWebApi.App_Start.Filters; 
#endregion

namespace SuperManWebApi.Controllers
{
    /// <summary>
    /// 财务相关功能点   涉及 B、C端
    /// </summary>
    [ExecuteTimeLog]
    [Validate]
    [ApiVersion]
    public class FinanceController : ApiController
    {
        #region 声明对象
        readonly IClienterFinanceProvider iClienterFinanceProvider = new ClienterFinanceProvider();
        readonly IBusinessFinanceProvider iBusinessFinanceProvider = new BusinessFinanceProvider();
        readonly IBusinessFinanceAccountProvider iBusinessFinanceAccountProvider = new BusinessFinanceAccountProvider();
        
        #endregion

        #region C端
        /// <summary>
        /// 骑士提现功能 add by caoheyang 20150509
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        [Token]
        [HttpPost]
        public ResultModel<object> WithdrawC([FromBody]WithdrawCriteria model)
        {
            return iClienterFinanceProvider.WithdrawC(model);
        }

        /// <summary>
        /// 骑士绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardBindCpm">参数实体</param>
        /// <returns></returns>
        [Token]
        [HttpPost]
        public ResultModel<object> CardBindC([FromBody]CardBindCPM cardBindCpm)
        {
            return iClienterFinanceProvider.CardBindC(cardBindCpm);
        }

        /// <summary>
        /// 骑士修改绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardModifyCpm">参数实体</param>
        /// <returns></returns>
        [Token]
        [HttpPost]
        public ResultModel<object> CardModifyC([FromBody]CardModifyCPM cardModifyCpm)
        {
            return iClienterFinanceProvider.CardModifyC(cardModifyCpm);
        }

        #endregion

        #region B端

        /// <summary>
        /// 商户提现功能 add by caoheyang 20150509
        /// </summary>
        /// <param name="withdrawBpm">参数实体</param>
        /// <returns></returns>
        [Token]
        [HttpPost]
        public ResultModel<object> WithdrawB([FromBody]WithdrawBPM withdrawBpm)
        {
            return iBusinessFinanceProvider.WithdrawB(withdrawBpm);
        }

        /// <summary>
        /// 商户绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardBindBpm">参数实体</param>
        /// <returns></returns>
        [HttpPost]
        //[Token]
        public ResultModel<object> CardBindB([FromBody]CardBindBPM cardBindBpm)
        {
            return iBusinessFinanceAccountProvider.CardBindB(cardBindBpm);
        }

        /// <summary>
        /// 商户修改绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardModifyBpm">参数实体</param>
        /// <returns></returns>
        //[Token]
        [HttpPost]
        public ResultModel<object> CardModifyB([FromBody]CardModifyBPM cardModifyBpm)
        {
            return iBusinessFinanceAccountProvider.CardModifyB(cardModifyBpm);
        }

        #endregion

        /// <summary>
        /// 获取银行省市
        /// 彭宜
        /// 2015年7月15日
        /// </summary>
        /// <param name="bankProvinceCityPM"></param>
        /// <returns></returns>        
        [Token]
        [HttpPost]
        [ApiVersionStatistic]
        public ResultModel<AreaModelList> GetBankProvinceCity([FromBody]BankProvinceCityPM bankProvinceCityPM)
        {
            IAreaProvider area = new AreaProvider();

            return area.GetPublicBankCity(bankProvinceCityPM.DataVersion, false);
        }
    }
}
