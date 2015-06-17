using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Const;
using Ets.Dao.Message;
using ETS.Data.PageData;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.DataModel.Message;
using Ets.Model.ParameterModel.Message;
using Ets.Service.IProvider.Message;
using Ets.Model.DomainModel.Message;

namespace Ets.Service.Provider.Message
{
    /// <summary>
    /// 消息模块  add by caoheyang 20150615
    /// </summary>
    public class MessageProvider : IMessageProvider
    {
        private readonly BusinessMessageDao businessMessageDao = new BusinessMessageDao();
        private readonly ClienterMessageDao clienterMessageDao = new ClienterMessageDao();
        private readonly MessageDao messageDao = new MessageDao();

        /// <summary>
        /// 商户阅读接口更新消息状态接口 add by caoheyang 20150615
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> ReadB(ReadBPM model)
        {
            return ResultModel<object>.Conclude(SystemEnum.Success, businessMessageDao.ReadB(model.MessageId));
        }

        /// <summary>
        /// 骑士阅读接口更新消息状态接口 add by caoheyang 20150615
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> ReadC(ReadCPM model)
        {
            return ResultModel<object>.Conclude(SystemEnum.Success, clienterMessageDao.ReadC(model.MessageId));
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

        /// <summary>
        /// web后台列表页功能 add by caoheyang 20150616
        /// </summary>
        public PageInfo<MessageModel> WebList(WebListSearch model)
        {
            return messageDao.WebList(model);
        }

        /// <summary>
        /// 添加消息任务
        /// danny-20150617
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DealResultInfo EditMessageTask(MessageModelDM model)
        {
            var dealReg = new DealResultInfo
            {
                DealFlag = false
            };
            if (model.DealType == 1)
            {
                dealReg.DealFlag = messageDao.AddMessageTask(model);
                dealReg.DealMsg = dealReg.DealFlag ? "消息任务添加成功！" : "消息任务添加失败！";
            }
            return dealReg;

        }
        /// <summary>
        /// 根据消息Id获取消息信息
        /// danny-20150617
        /// </summary>
        /// <param name="messageId">消息Id</param>
        /// <returns></returns>
        public MessageModel GetMessageById(int messageId)
        {
            return messageDao.GetMessageById(messageId);
        }
    }
}
