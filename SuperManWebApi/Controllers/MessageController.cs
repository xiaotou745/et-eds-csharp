using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Message;
using Ets.Service.IProvider.Message;
using Ets.Service.Provider.Message;
using SuperManWebApi.App_Start.Filters;

namespace SuperManWebApi.Controllers
{
    /// <summary>
    /// 消息中心模块接口
    /// </summary>
    [ExecuteTimeLog]
    [Validate]
    [ApiVersion]
    public class MessageController : ApiController
    {
        readonly IMessageProvider messageProvider = new MessageProvider();
        /// <summary>
        /// 商户阅读接口更新消息状态接口 add by caoheyang 20150615
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public async Task<ResultModel<object>> ReadB([FromBody]ReadBPM model)
        {
            return await messageProvider.ReadB(model);
        }

        /// <summary>
        /// 骑士阅读接口更新消息状态接口 add by caoheyang 20150615
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public async Task<ResultModel<object>> ReadC([FromBody]ReadCPM model)
        {
            return await messageProvider.ReadC(model);
        }

        /// <summary>
        /// 商户端获取消息列表接口 add by caoheyang 20150615
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public async Task<ResultModel<object>> ListB([FromBody]ListBPM model)
        {
            return await messageProvider.ListB(model);
        }

        /// <summary>
        /// 骑士端获取消息列表接口 add by caoheyang 20150615
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public async Task<ResultModel<object>> ListC([FromBody]ListCPM model)
        {
            return await  messageProvider.ListC(model);
        }
    }
}
