using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperMan.Models
{
    public class ClienterModel : clienter
    {
        /// <summary>
        /// 所选商家Id集合
        /// </summary>
        public string CheckBusiList { get; set; }
    }
}