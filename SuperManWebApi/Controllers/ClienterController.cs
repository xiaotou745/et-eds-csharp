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
using Ets.Model.DomainModel.Clienter;
using Ets.Model.ParameterModel.Order;
using Ets.Service.Provider.Clienter;
using Ets.Service.IProvider.Clienter;
using ETS.Enums;
using SuperManWebApi.App_Start.Filters;

namespace SuperManWebApi.Controllers
{
    /// <summary>
    /// 骑士相关接口 add by caoheyang
    /// </summary>
    [ExecuteTimeLog]
    public class ClienterController : ApiController
    {
        private readonly IClienterFinanceProvider _iClienterFinanceProvider = new ClienterFinanceProvider();
        private readonly IClienterProvider _iClienterProvider = new ClienterProvider();

        /// <summary>
        /// 骑士交易流水API caoheyang 20150512
        /// </summary>
        /// <param name="model">查询参数实体</param>
        /// <returns></returns>
        [HttpPost]
        [Validate]
        [ApiVersion]
        public ResultModel<object> Records(ClienterRecordsPM model)
        {
            return _iClienterFinanceProvider.GetRecords(model.ClienterId);
        }

        /// <summary>
        /// 获取骑士详情              
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="model">骑士参数</param>
        /// <returns></returns>        
       [HttpPost]
       public ResultModel<ClienterDM> Get(ClienterPM model)
       {
           #region 验证
           var version = HttpContext.Current.Request.Form["Version"];
           if (string.IsNullOrWhiteSpace(version)) //版本号 
           {
               return ResultModel<ClienterDM>.Conclude(GetClienterStatus.NoVersion);
           }
           if (model.ClienterId < 0)//骑士Id不合法
           {
               return ResultModel<ClienterDM>.Conclude(GetClienterStatus.ErrOderNo);
           }
           if (!_iClienterProvider.IsExist(model.ClienterId)) //骑士不存在
           {
               return ResultModel<ClienterDM>.Conclude(GetClienterStatus.ErrOderNo);
           }
           #endregion

           ClienterDM clienterDM = _iClienterProvider.GetDetails(model.ClienterId);
           return Ets.Model.Common.ResultModel<ClienterDM>.Conclude(GetClienterStatus.Success, clienterDM);
       }     
        
    }
}
