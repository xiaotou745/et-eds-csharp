using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Const;
using Ets.Dao.Message;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Message;
using Ets.Service.IProvider.Message;

namespace Ets.Service.Provider.Message
{
    /// <summary>
    /// 消息模块  add by caoheyang 20150615
    /// </summary>
    public class MessageProvider : IMessageProvider
    {
        private readonly BusinessMessageDao businessMessageDao = new BusinessMessageDao();
        private readonly ClienterMessageDao clienterMessageDao = new ClienterMessageDao();

        /// <summary>
        /// 商户阅读接口更新消息状态接口 add by caoheyang 20150615
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> ReadB(ReadBPM model)
        {
            businessMessageDao.Update(model.MessageId);
            return ResultModel<object>.Conclude(SystemEnum.Success);
        }

        /// <summary>
        /// 骑士阅读接口更新消息状态接口 add by caoheyang 20150615
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> ReadC(ReadCPM model)
        {
            clienterMessageDao.Update(model.MessageId);
            return ResultModel<object>.Conclude(SystemEnum.Success);
        }

        /// <summary>
        /// 商户端获取消息列表接口 add by caoheyang 20150615
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> ListB(ListBPM model)
        {
            return ResultModel<object>.Conclude(SystemEnum.Success, businessMessageDao.Query(model).Records);
        }

        /// <summary>
        /// 骑士端获取消息列表接口 add by caoheyang 20150615
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> ListC(ListCPM model)
        {
            return ResultModel<object>.Conclude(SystemEnum.Success, clienterMessageDao.Query(model).Records);
        }
    }
}
