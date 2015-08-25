using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{
    /// <summary>
    /// 请求记录model
    /// 茹化肖
    /// 2015年8月25日11:06:05
    /// </summary>
    public class HttpModel
    {
        public HttpModel()
        {
            this.Url = "";
            this.Htype = 0;
            this.RequestBody = "";
            this.Msg = "";
            this.Status = 0;
            this.Remark = "";
        }
        public string Url { get; set; }
        public int Htype { get; set; }
        public string RequestBody { get; set; }
        public int ResponseType { get; set; }
        public string Msg { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
    }
}
