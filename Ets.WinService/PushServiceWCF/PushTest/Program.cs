using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PushTest.ServiceReference1;

namespace PushTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (ServiceReference1.PushSerClient c = new PushSerClient())
                {
                    while (true)
                    {
                        Console.WriteLine("A.Pad及时推送，B.Pad延时推送。C.Mobile及时推送。D.Mobile延时推送");
                        string str = Console.ReadLine();
                        
                         if (str.Equals("C") || str.Equals("c"))
                        {
                            Console.WriteLine("请输入推送消息：");
                            string info = Console.ReadLine();
                            c.PushForMobile(info);
                        }
                       
                    }
                    Console.WriteLine("OK");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                //throw;
            }
            Console.ReadKey();
        }


    }
}
