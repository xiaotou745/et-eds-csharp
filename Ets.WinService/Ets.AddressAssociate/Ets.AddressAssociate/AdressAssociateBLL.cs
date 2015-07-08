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
        private static string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "date.xml";
        public void Execute(Quartz.IJobExecutionContext context)
        {
            if (!threadSafe)
            {
                return;
            }
            threadSafe = false;
            try
            {
                //DateTime currentDate = ParseHelper.ToDatetime("2015-07-06 17:00:00");

                DateTime currentDate = DateTime.Now;
                //DateModel modelsave = new DateModel()
                //{
                //    date = currentDate
                //};
                //List<DateModel> listDate1 = new List<DateModel>()
                //    {
                //        new DateModel() {date=currentDate}
                //    };
                //XmlHelper.ToXml(filePath, listDate1);
                LogHelper.LogWriterString("扫表开始:时间:" + DateTime.Now.ToString());

                List<DateModel> dateModel = XmlHelper.ToObject<List<DateModel>>(filePath);
                //XmlDocument xdoc = new XmlDocument();

                //xdoc.Load(path);
                //XmlNode dateNode = xdoc.SelectSingleNode("date");
                //DateTime sqldate = DateTime.MinValue;
                //DateTime.TryParse(dateNode.Value, out sqldate);
                //DateTime lastdate = DateTime.Now;
                // 按照 sqldate 查询数据库
                var receviceAddressDao = new ReceviceAddressDao();
                var resultcount = receviceAddressDao.GetAddress(ParseHelper.ToDatetime(dateModel[0].date, currentDate));
                if (resultcount > 0)
                {
                    //本次查询的ID大于上次查询的ID查询成功
                    //XmlElement dateElement = (XmlElement)dateNode;
                    //dateElement.InnerText = lastdate.ToString();
                    List<DateModel> listDate = new List<DateModel>()
                    {
                        new DateModel() {date=currentDate}
                    };
                    XmlHelper.ToXml(filePath, listDate);
                }
                //xdoc.Save(path);
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
