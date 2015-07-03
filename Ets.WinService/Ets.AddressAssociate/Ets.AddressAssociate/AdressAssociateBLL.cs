using System;
using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD
=======
using System.Net.Mime;
>>>>>>> 60667e98cadf7fbde80a00cb894de57d7aa8fe87
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Common.Logging;
using Ets.Dao.Order;
using ETS.Util;

namespace Ets.AddressAssociate
{
    public class AdressAssociateBLL : Quartz.IJob
    {
        private ILog logger = LogManager.GetCurrentClassLogger();
        private static bool threadSafe = true;//线程安全
        public void Execute(Quartz.IJobExecutionContext context)
        {
            if (!threadSafe)
            {
                return;
            }
            threadSafe = false;
            try
            {
<<<<<<< HEAD
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(@"date.xml");
=======
                LogHelper.LogWriterString("扫表开始:时间:" + DateTime.Now.ToString());
                XmlDocument xdoc = new XmlDocument();
                string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\date.xml";
                logger.Info("时间XML路径:"+path);
                xdoc.Load(path);
>>>>>>> 60667e98cadf7fbde80a00cb894de57d7aa8fe87
                XmlNode dateNode = xdoc.SelectSingleNode("date");
                DateTime lastdate = DateTime.Now;
                // 按照 sqldate 查询数据库
                var receviceAddressDao=new ReceviceAddressDao();
                if (receviceAddressDao.GetAddress(lastdate) > 0)
                {
                    //本次查询的ID大于上次查询的ID查询成功
                    XmlElement dateElement = (XmlElement)dateNode;
                    dateElement.InnerText = lastdate.ToString();
<<<<<<< HEAD
                } 
                xdoc.Save(@"date.xml");
=======
                }
                xdoc.Save(path);
                LogHelper.LogWriterString("扫表结束:时间:" + DateTime.Now.ToString());
>>>>>>> 60667e98cadf7fbde80a00cb894de57d7aa8fe87
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

    }
}
