using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Business
{
    public class NewPostPublishOrderResultModel
    {
        public string OrderNo { get; set; }

        public string OriginalOrderNo { get; set; }

        public int PubOrderStatus { get; set; }

        public string Remark { get; set; }

    }
}