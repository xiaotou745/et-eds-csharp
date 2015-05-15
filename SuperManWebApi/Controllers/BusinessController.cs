﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Ets.Model.Common;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Service.IProvider.Finance;
using Ets.Service.Provider.Finance;
using ETS.Util;
using Ets.Model.Common;
using ETS.Enums;
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.ParameterModel.Order;
using Ets.Service.Provider.Finance;
using Ets.Service.IProvider.User;
using Ets.Service.Provider.User;
using SuperManWebApi.App_Start.Filters;

namespace SuperManWebApi.Controllers
{
    /// <summary>
    /// 商户相关接口 add by caoheyang
    /// </summary>
    [ExecuteTimeLog]
    [Validate]
    [ApiVersion]
    public class BusinessController : ApiController
    {
        private readonly IBusinessFinanceProvider _businessFinanceProvider = new BusinessFinanceProvider();
        private readonly IBusinessProvider _iBusinessProvider = new BusinessProvider();

        /// <summary>
        /// 商户交易流水API caoheyang 20150512
        /// </summary>
        /// <param name="model">查询参数实体</param>
        /// <returns></returns>
        [HttpPost]
        [Validate]
        [ApiVersion]
        public ResultModel<object> Records(BussinessRecordsPM model)
        {
            return _businessFinanceProvider.GetRecords(model.BusinessId);
        }

        /// <summary>
        /// 获取商户详情       
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="model">商户参数</param>
        /// <returns></returns>        
        [HttpPost]
        public ResultModel<BusinessDM> Get(BussinessPM model)
        {
            #region 验证
            var version = HttpContext.Current.Request.Form["Version"];
            if (string.IsNullOrWhiteSpace(version)) //版本号 
            {
                return ResultModel<BusinessDM>.Conclude(GetBussinessStatus.NoVersion);
            }
            if (model.BussinessId < 0)//商户Id不合法
            {
                return ResultModel<BusinessDM>.Conclude(GetBussinessStatus.ErrOderNo);
            }
            if (!_iBusinessProvider.IsExist(model.BussinessId)) //商户不存在
            {
                return ResultModel<BusinessDM>.Conclude(GetBussinessStatus.ErrOderNo);
            }

            #endregion

            try
            {
                BusinessDM businessDM = _iBusinessProvider.GetDetails(model.BussinessId);
                return Ets.Model.Common.ResultModel<BusinessDM>.Conclude(GetBussinessStatus.Success, businessDM);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("ResultModel<BusinessDM> Get", new { obj = "时间：" + DateTime.Now.ToString() + ex.Message });
                return ResultModel<BusinessDM>.Conclude(GetBussinessStatus.Failed);
            }    
        }
    }
}
