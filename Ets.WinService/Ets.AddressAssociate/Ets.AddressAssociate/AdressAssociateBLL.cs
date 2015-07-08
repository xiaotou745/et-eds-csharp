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
        //private static string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "date.xml";
        public void Execute(Quartz.IJobExecutionContext context)
        {
            if (!threadSafe)
            {
                return;
            }
            threadSafe = false;
            try
            {
                DateTime currentDate = DateTime.Now;
                LogHelper.LogWriterString("扫表开始:时间:" + DateTime.Now.ToString());
                string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "date.xml";
                List<DateModel> dateModel = XmlHelper.ToObject<List<DateModel>>(filePath);
                var receviceAddressDao = new ReceviceAddressDao();
                var resultcount = receviceAddressDao.GetAddress(ParseHelper.ToDatetime(dateModel[0].date, currentDate));
                if (resultcount > 0)
                {
                    //本次查询的ID大于上次查询的ID查询成功
                    List<DateModel> listDate = new List<DateModel>()
                    {
                        new DateModel() {date=currentDate}
                    };
                    XmlHelper.ToXml(filePath, listDate);
                }
                LogHelper.LogWriterString("扫表结束:时间:" + DateTime.Now.ToString() + "本次影响行数:" + resultcount);
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

    public class DateModel
    {
        public DateTime date { get; set; }
    }
}
