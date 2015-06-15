using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Message;

namespace Ets.Service.IProvider.Message
{
    /// <summary>
    /// 消息模块  add by caoheyang 20150615
    /// </summary>
    public interface IMessageProvider
    {
        /// <summary>
        /// 商户阅读接口更新消息状态接口 add by caoheyang 20150615
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        ResultModel<object> ReadB(ReadBPM model);

        /// <summary>
        /// 骑士阅读接口更新消息状态接口 add by caoheyang 20150615
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        ResultModel<object> ReadC(ReadCPM model);

        /// <summary>
        /// 商户端获取消息列表接口 add by caoheyang 20150615
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        ResultModel<object> ListB(ListBPM model);

        /// <summary>
        /// 骑士端获取消息列表接口 add by caoheyang 20150615
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        ResultModel<object> ListC(ListCPM model);
    }
}
