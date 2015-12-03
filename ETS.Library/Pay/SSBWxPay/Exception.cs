using System;
using System.Collections.Generic;
using System.Web;

namespace ETS.Library.Pay.SSBWxPay
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}