using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Clienter
{
    public class RushOrderResultModel
    {
        public string userId { get; set; }
    }
    public class FinishOrderResultModel1
    {
        public int userId { get; set; }
        public decimal balanceAmount { get; set; }
    }
}