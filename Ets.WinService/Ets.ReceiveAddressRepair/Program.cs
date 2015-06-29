using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Util;

namespace ReceiveAddressRepair
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
                    StringBuilder upStringBuilder = new StringBuilder();
                    int noaddres = 0;
                    for (int i = 0; i < kk.Rows.Count; i++)
                    {
                        DataRow dr = kk.Rows[i];
                        string addres = ra.GetAddress(dr[1].ToString(), dr[2].ToString());
                        if (!string.IsNullOrWhiteSpace(addres) && !addres.StartsWith("0"))
                        {
                            upStringBuilder.Append(ra.CreateAddressSql(ParseHelper.ToInt(dr[0]), addres));
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
            Console.Read();
        }
    }
}
