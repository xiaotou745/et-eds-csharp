﻿using System;
using System.Collections.Generic;
using System.Net;
using cn.jpush.api.common;
using BaseResult = ETS.Library.JPush.common.BaseResult;

namespace ETS.Library.JPush.report
{
    public class ReceivedResult : BaseResult
    {

        private List<Received> receivedList = new List<Received>();

        public List<Received> ReceivedList
        {
            get { return receivedList; }
            set { receivedList = value; }
        }
	
	    public class Received {
	        public long msg_id;
	        public String android_received;
	        public String ios_apns_sent;
	    }

        public override bool isResultOK()
        {
            if (Equals(ResponseResult.responseCode, HttpStatusCode.OK))
            {
                return true;
            }
            return false;
        }

        public HttpStatusCode getErrorCode()
        {
            if (null != ResponseResult)
            {
                return ResponseResult.responseCode;
            }
            return 0;
        }

        public  string getErrorMessage()
        {
            if (null != ResponseResult)
            {
                return ResponseResult.exceptionString;
            }
            return "";
        }
    }
}
