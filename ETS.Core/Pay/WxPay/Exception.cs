using System;
using System.Collections.Generic;
using System.Web;

namespace ETS.Pay.WxPay
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}