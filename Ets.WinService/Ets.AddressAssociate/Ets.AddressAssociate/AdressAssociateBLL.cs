using System;
using System.Collections.Generic;
using System.Linq;
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
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(@"date.xml");
                XmlNode dateNode = xdoc.SelectSingleNode("date");
                DateTime lastdate = DateTime.Now;
                // 按照 sqldate 查询数据库
                var receviceAddressDao=new ReceviceAddressDao();
                if (receviceAddressDao.GetAddress(lastdate) > 0)
                {
                    //本次查询的ID大于上次查询的ID查询成功
                    XmlElement dateElement = (XmlElement)dateNode;
                    dateElement.InnerText = lastdate.ToString();
                } 
                xdoc.Save(@"date.xml");
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
