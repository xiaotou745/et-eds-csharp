using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Message;

namespace SuperManWebApi.Controllers
{
    /// <summary>
    /// 消息中心模块接口
    /// </summary>
    public class MessageController : ApiController
    {

        /// <summary>
        /// 商户阅读接口更新消息状态接口 add by caoheyang 20150615
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> ReadB(ReadBPM model)
        {
            return null;
        }

        /// <summary>
        /// 骑士阅读接口更新消息状态接口 add by caoheyang 20150615
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> ReadC(ReadCPM model)
        {
            return null;
        }

        /// <summary>
        /// 商户端获取消息列表接口 add by caoheyang 20150615
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> ListB(ListBPM model)
        {
            return null;
        }

        /// <summary>
        /// 骑士端获取消息列表接口 add by caoheyang 20150615
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> ListC(ListCPM model)
        {
            return null;
        }
    }
}
