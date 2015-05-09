using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Finance;

namespace SuperManWebApi.Controllers
{
    public class FinanceController : ApiController
    {
        /// <summary>
        /// 骑士提现功能 add by caoheyang 20150509
        /// </summary>
        /// <param name="withdrawCpm">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> WithdrawC(WithdrawCPM withdrawCpm)
        {
            return null;
        }

    }
}
