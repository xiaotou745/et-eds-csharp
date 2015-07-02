using ETS.Util;
using ReceiveAddressRepair;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.ReceiveAddressRepair
{
    class Program
    {
        static void Main(string[] args)
        {
            bool b = true;
            while (b)
            {
                Console.WriteLine("开始处理数据");
                DateTime pubDate = ParseHelper.ToDatetime(ConfigurationManager.AppSettings["StartTime"]);
                ReceiveAddress ra = new ReceiveAddress();
                var kk = ra.GetNoReceiveAddress(pubDate);
                if (kk.Rows.Count == 0)
                {
                    Console.WriteLine("没有需要处理的数据");
                    break;
                }
                else
                {
                    Console.WriteLine("取到"+kk.Rows.Count+"条数据");
                    StringBuilder upStringBuilder = new StringBuilder();
                    int noaddres = 0;
                    for (int i = 0; i < kk.Rows.Count; i++)
                    {
                        DataRow dr = kk.Rows[i];
                        Console.WriteLine("订单Id："+dr[0].ToString()+"    经度：" + dr[1].ToString() + "   纬度：" + dr[2].ToString());
                        string addres = ra.GetAddress(dr[1].ToString(), dr[2].ToString());
                        Console.WriteLine("订单Id：" + dr[0].ToString() + "    地址："+addres);
                        if (!string.IsNullOrWhiteSpace(addres) && !addres.StartsWith("0"))
                        {
                            Console.WriteLine("更新地址" + "订单Id：" + dr[0].ToString());
                            ra.CreateAddressSql(ParseHelper.ToInt(dr[0]), addres);
                        }
                        else
                        {
                            noaddres++;
                        }
                    }

                    if (noaddres == kk.Rows.Count)
                    {
                        Console.WriteLine("剩余数据无法获取地址");
                        b = false;
                    }
                }
            }
            Console.WriteLine("完成所有");
            Console.Read();
        }
    }
}
