using System.Net;

namespace ETS.Library.JPush.common.resp
{
    public  class DefaultResult:ETS.Library.JPush.common.BaseResult
    {

        public static DefaultResult fromResponse(ETS.Library.JPush.common.ResponseWrapper responseWrapper)
        {
            DefaultResult result = null;

            if (responseWrapper.isServerResponse())
            {
                result = new DefaultResult();
            }

            result.ResponseResult=responseWrapper;

            return result;
        }
        public override bool isResultOK()
        {
            if (Equals(ResponseResult.responseCode, HttpStatusCode.OK))
            {
                return true;
            }
            return false;
        }
    }
}
