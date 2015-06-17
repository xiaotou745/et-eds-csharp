﻿using Common.Logging;
using Ets.Dao.Business;
using Ets.Dao.Clienter;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ets.Dao.Message;
using Ets.Model.DataModel.Message;
using ETS.Enums;
using System.Data;
using ETS.Sms;
using Ets.Service.Provider.MyPush;
using Ets.Model.Common;
namespace Ets.SendMessage
{
    public class MessageBLL : Quartz.IJob
    {
        //使用Common.Logging.dll日志接口实现日志记录        
        private ILog logger = LogManager.GetCurrentClassLogger();
        private static bool threadSafe = true;//线程安全
        MessageDao messageDao = new MessageDao();
        BusinessMessageDao businessMessageDao = new BusinessMessageDao();
        ClienterMessageDao clienterMessageDao = new ClienterMessageDao();

        #region IJob 成员

        public void Execute(Quartz.IJobExecutionContext context)
        {
            if (!threadSafe)
            {
                return;
            }
            threadSafe = false;
            try
            {
                LogHelper.LogWriter("执行啦:" + DateTime.Now);
           
                //获取未发送短信集合
                IList<MessageModel> list=messageDao.GetMessageList(0);
                foreach (var item in list)
                {
                    switch(item.PushWay)
                    {
                        case (int)MessagePushWay.Message:
                            {
                                SendMessageTarget(item);                                                              
                            }
                            break;
                        case (int)MessagePushWay.App:
                            {
                                SendAppTarget(item);
                            }
                            break;
                        case (int)MessagePushWay.MessageAndApp:
                            {
                                SendMessageTarget(item);
                                SendAppTarget(item);
                            }
                            break;              
                        default:break;
                    }                  
                }             

            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex);
            }
            finally
            {
                threadSafe = true;
            }

        }

        #region 短信
        //发送短信
        void SendMessageTarget(MessageModel model)
        {
            switch (model.PushTarget)
            {
                case (int)MessagePushTarget.Business:
                    {
                        SendMessageBusiness(model);    
                        //更新发布状态和完成时间
                        messageDao.Update(model.Id);
                    }
                    break;
                case (int)MessagePushTarget.Clienter:
                    {
                        SendMessagClienter(model);
                        //更新发布状态和完成时间
                        messageDao.Update(model.Id);
                    }
                    break;
                case (int)MessagePushTarget.BusinessClienter:
                    {
                        SendMessageBusiness(model);
                        SendMessagClienter(model);
                        //更新发布状态和完成时间
                        messageDao.Update(model.Id);
                    }
                    break;
                case (int)MessagePushTarget.Import:
                    {
                        SendMessImport(model);
                        //更新发布状态和完成时间
                        messageDao.Update(model.Id);
                    }
                    break;
                default: break;
            }                  
        }

        //发送商家短信
        void SendMessageBusiness(MessageModel model)
        {
            BusinessDao businessDao = new BusinessDao();
            DataTable dt= businessDao.GetPhoneNoList(model.PushCity);
            for (int i = 0; i < dt.Rows.Count; i++)
            {                
                Task.Factory.StartNew(() =>
                {
                    SendSmsHelper.SendSendSmsSaveLog(dt.Rows[i]["PhoneNo"].ToString(), model.Content, Ets.Model.Common.ConstValues.SMSSOURCE);
                    //写日志
                });
            }
        }
        //发送骑士短信
        void SendMessagClienter(MessageModel model)
        {
            ClienterDao clienterDao = new ClienterDao();
            DataTable dt = clienterDao.GetPhoneNoList(model.PushCity);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    SendSmsHelper.SendSendSmsSaveLog(dt.Rows[i]["PhoneNo"].ToString(), model.Content, Ets.Model.Common.ConstValues.SMSSOURCE);
                    //写日志
                });
            }
        }
        //发送批量导入短信
        void SendMessImport(MessageModel model)
        {
            if (!string.IsNullOrEmpty(model.PushPhone))
            {
                string []sp =model.PushPhone.Split(',');
                for(int i=0;i<sp.Length;i++)
                {
                    Task.Factory.StartNew(() =>
                    {
                        SendSmsHelper.SendSendSmsSaveLog(sp[i].ToString(), model.Content, Ets.Model.Common.ConstValues.SMSSOURCE);
                        //写日志
                    });
                }
            }
        }

        #endregion

        #region App
        //发送app
        void SendAppTarget(MessageModel model)
        {
            switch (model.PushTarget)
            {
                case (int)MessagePushTarget.Business:
                    {
                        SendAppBusiness(model);
                        //更新发布状态和完成时间
                        messageDao.Update(model.Id);
                    }
                    break;
                case (int)MessagePushTarget.Clienter:
                    {
                        SendAPPClienter(model);
                        //更新发布状态和完成时间
                        messageDao.Update(model.Id);
                    }
                    break;
                case (int)MessagePushTarget.BusinessClienter:
                    {
                        SendAppBusiness(model);
                        SendAPPClienter(model);
                        //更新发布状态和完成时间
                        messageDao.Update(model.Id);
                    }
                    break;
                case (int)MessagePushTarget.Import:
                    {

                    }
                    break;
                default: break;
            }
        }
        //发送商家app
        void SendAppBusiness(MessageModel model)
        {
            BusinessDao businessDao = new BusinessDao();
            DataTable dt = businessDao.GetPhoneNoList(model.PushCity);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string businessId = dt.Rows[i]["id"].ToString();
                businessMessageDao.Insert(new BusinessMessage
                                        {
                                            BusinessId = Convert.ToInt32(businessId),
                                            Content = model.Content,
                                            IsRead = 0
                                        }
                            );

                Task.Factory.StartNew(() =>
                {
                    JPushModel jpushModel = new JPushModel()
                    {
                        Alert = "发送App消息完成！",
                        City = string.Empty,
                        Content = model.Content,//string.Concat(model.orderId, "_", model.orderChildId, "_", orderChildPayModel.PayStatus),
                        RegistrationId = businessId,
                        TagId = 1,
                        Title = "消息提醒"
                    };
                   Push.PushMessage(jpushModel);
                });                
            }
        }

        //发送骑士app
        void SendAPPClienter(MessageModel model)
        {
            ClienterDao clienterDao = new ClienterDao();
            DataTable dt = clienterDao.GetPhoneNoList(model.PushCity);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string clienterId = dt.Rows[i]["id"].ToString();
                clienterMessageDao.Insert(new ClienterMessage
                                        {
                                            ClienterId = Convert.ToInt32(clienterId),
                                            Content = model.Content,
                                            IsRead = 0
                                        }
                            );

                Task.Factory.StartNew(() =>
                {
                    JPushModel jpushModel = new JPushModel()
                    {
                        Alert = "发送App消息完成！",
                        City = string.Empty,
                        Content = model.Content,//string.Concat(model.orderId, "_", model.orderChildId, "_", orderChildPayModel.PayStatus),
                        RegistrationId = clienterId,
                        TagId = 1,
                        Title = "消息提醒"
                    };
                    Push.PushMessage(jpushModel);
                });
            }
        }
        #endregion

        #endregion
    }
}
