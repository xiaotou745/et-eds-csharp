using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.AccountCheck
{
    class ErrorService
    {
        static string file = "error.txt";
        public static void Delete()
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        public static void Add(string line)
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);

            if (!File.Exists(fullPath))
            {
                using (File.Create(fullPath))
                {

                }
            }
            File.AppendAllLines(fullPath, new string[1] { line });
        }
        public static string[] Read()
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
            if (!File.Exists(fullPath))
            {
                return new string[0];
            }
            return File.ReadAllLines(fullPath);
        }
    }
}
