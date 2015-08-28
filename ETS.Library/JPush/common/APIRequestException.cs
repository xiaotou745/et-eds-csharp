using System;
using System.Net;
using JpushError = ETS.Library.JPush.push.JpushError;

namespace ETS.Library.JPush.common
{
    public class APIRequestException:Exception
    {

        private ETS.Library.JPush.common.ResponseWrapper responseRequest;
        public APIRequestException(ETS.Library.JPush.common.ResponseWrapper responseRequest)
            : base(responseRequest.exceptionString)
        {
            this.responseRequest = responseRequest;
        }
        public HttpStatusCode Status
        {
            get
            {
                return this.responseRequest.responseCode; 
            }
        }
        public long MsgId
        {
            get
            {
                return responseRequest.jpushError.msg_id; 
            }
        }
        public int ErrorCode
        {
            get
            {
                return responseRequest.jpushError.error.code;
            }
        }

        public String ErrorMessage 
        {
            get
            {
                return responseRequest.jpushError.error.message;
            }
        }
        private JpushError ErrorObject()
        {
            return responseRequest.jpushError;
        }
        public int RateLimitQuota()
        {
            return responseRequest.rateLimitQuota;
        }
        public int RateLimitRemaining()
        {
            return responseRequest.rateLimitRemaining;
        }
        public int RateLimitReset()
        {
            return responseRequest.rateLimitReset;
        }
    }
    
}
