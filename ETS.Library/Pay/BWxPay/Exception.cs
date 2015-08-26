using System;
using System.Collections.Generic;
using System.Web;

namespace ETS.Library.Pay.BWxPay
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}