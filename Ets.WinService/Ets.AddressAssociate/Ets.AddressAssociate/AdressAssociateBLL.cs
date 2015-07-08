using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Ets.Dao.Order;
using ETS.Util;

namespace Ets.AddressAssociate
{
    public class AdressAssociateBLL : Quartz.IJob
    {
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
                LogHelper.LogWriterString("扫表开始:时间:" + DateTime.Now.ToString());
                XmlDocument xdoc = new XmlDocument();
                string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\date.xml";
                xdoc.Load(path);
                XmlNode dateNode = xdoc.SelectSingleNode("date");
                DateTime sqldate = DateTime.MinValue;
                DateTime.TryParse(dateNode.Value, out sqldate);
                DateTime lastdate = DateTime.Now;
                // 按照 sqldate 查询数据库
                var receviceAddressDao=new ReceviceAddressDao();
                var resultcount = receviceAddressDao.GetAddress(sqldate);
                if ( resultcount> 0)
                {
                    //本次查询的ID大于上次查询的ID查询成功
                    XmlElement dateElement = (XmlElement)dateNode;
                    dateElement.InnerText = lastdate.ToString();
                }
                xdoc.Save(path);
                LogHelper.LogWriterString("扫表结束:时间:" + DateTime.Now.ToString()+"本次影响行数:"+resultcount);
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
