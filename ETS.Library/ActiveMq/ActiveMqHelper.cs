using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System.Threading;
using ETS.Util;

namespace ETS.Library.ActiveMq
{
    /// <summary>
    /// mq帮助类
    /// </summary>
   public static  class ActiveMqHelper
    {

        //初始化工厂，这里默认的URL是不需要修改的
       static IConnectionFactory factory = new ConnectionFactory(ConfigurationManager.AppSettings["activemq.url"]);

       private static void SendMessage(object messageStr)
        {
            try
            {
                //通过工厂建立连接
                using (IConnection connection = factory.CreateConnection())
                {
                    //通过连接创建Session会话
                    using (ISession session = connection.CreateSession())
                    {
                        //通过会话创建生产者，方法里面new出来的是MQ中的Queue
                        IMessageProducer prod = session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.
                            ActiveMQQueue(ConfigurationManager.AppSettings["activemq.queue"]));
                        //创建一个发送的消息对象
                        ITextMessage message = prod.CreateTextMessage();
                        message.Text = messageStr.ToString();
                        //设置消息对象的属性，这个很重要哦，是Queue的过滤条件，也是P2P消息的唯一指定属性
                        //message.Properties.SetString("filter", "demo");
                        //生产者把消息发送出去，几个枚举参数MsgDeliveryMode是否长链，MsgPriority消息优先级别，发送最小单位，当然还有其他重载
                        prod.Send(message, MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.MinValue);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);//发送错误邮件
            }
            
        }
        public static void AsynSendMessage(string messageStr)
        {
            ThreadPool.SetMaxThreads(5, 5);
            ThreadPool.QueueUserWorkItem(new WaitCallback(ActiveMqHelper.SendMessage), messageStr);
        }
      
    }
}
