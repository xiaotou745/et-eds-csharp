using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Util
{
    public class ImageTools
    {
        public string GetFileName(string fileExt)
        {
            Random rdom = new Random();
            DateTime dtime = DateTime.Now;
            string filename = dtime.Year + "_" + dtime.Month + "_" + dtime.Day + "_" + dtime.Hour + "_" + dtime.Minute + "_" + dtime.Second + "_" + rdom.Next(10000) + "." + fileExt;
            return filename;
        }
    }
}
