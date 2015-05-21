using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Ets.Model.Common;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Bussiness;
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
        private readonly IClienterFinanceProvider clienterFinanceProvider = new ClienterFinanceProvider();
        private readonly IClienterProvider clienterProvider = new ClienterProvider();
        private readonly IClienterLocationProvider clienterLocationProvider = new ClienterLocationProvider();

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
            return clienterFinanceProvider.GetRecords(model.ClienterId);
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
            var version = model.Version;
            if (string.IsNullOrWhiteSpace(version)) //版本号 
            {
                return ResultModel<ClienterDM>.Conclude(GetClienterStatus.NoVersion);
            }
            if (model.ClienterId < 0)//骑士Id不合法
            {
                return ResultModel<ClienterDM>.Conclude(GetClienterStatus.ErrNo);
            }
            if (!clienterProvider.IsExist(model.ClienterId)) //骑士不存在
            {
                return ResultModel<ClienterDM>.Conclude(GetClienterStatus.FailedGet);
            }
            #endregion
            try
            {
                ClienterDM clienterDM = clienterProvider.GetDetails(model.ClienterId);
                if (clienterDM.Status == GetClienterStatus.Refuse.GetHashCode())
                {
                    return Ets.Model.Common.ResultModel<ClienterDM>.Conclude(GetClienterStatus.Refuse, clienterDM);
                }
                if (clienterDM.Status == GetClienterStatus.Audit.GetHashCode())
                {
                    return Ets.Model.Common.ResultModel<ClienterDM>.Conclude(GetClienterStatus.Audit, clienterDM);
                }
                if (clienterDM.Status == GetClienterStatus.Auditing.GetHashCode())
                {
                    return Ets.Model.Common.ResultModel<ClienterDM>.Conclude(GetClienterStatus.Auditing, clienterDM);
                }
                return Ets.Model.Common.ResultModel<ClienterDM>.Conclude(GetClienterStatus.Success, clienterDM);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("ResultModel<BusinessDM> Get", new { obj = "时间：" + DateTime.Now.ToString() + ex.Message });
                return ResultModel<ClienterDM>.Conclude(GetClienterStatus.Failed);
            }
        }

        /// <summary>
        /// 骑士坐标上传              
        /// </summary>
        /// <UpdateBy>caoheyang</UpdateBy>
        /// <UpdateTime>20150519</UpdateTime>
        /// <returns></returns>     
        [HttpPost]
        [ApiVersion]
        public ResultModel<object> PushLocaltion(ClienterPushLocaltionPM model)
        {
            return clienterLocationProvider.InsertLocaltion(model);
        }
    }
}
