using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Data.PageData;
using Ets.Model.Common;
using Ets.Model.DataModel.Message;
using Ets.Model.ParameterModel.Message;
using Ets.Model.DomainModel.Message;

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

        /// <summary>
        /// web后台列表页功能 add by caoheyang 20150616
        /// </summary>
        PageInfo<MessageModel> WebList(WebListSearch model);
        /// <summary>
        /// 添加消息任务
        /// danny-20150617
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        DealResultInfo EditMessageTask(MessageModelDM model);


        /// <summary>
        /// 取消发布 add by caoheyang  20150617
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateby"></param>
        /// <returns></returns>
        bool CanelMessage(long id, string updateby);
    }
}
