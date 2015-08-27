using System;
using System.Collections.Generic;
using System.Net;
using cn.jpush.api.common;
using Newtonsoft.Json;
using BaseResult = ETS.Library.JPush.common.BaseResult;
using ResponseWrapper = ETS.Library.JPush.common.ResponseWrapper;

namespace ETS.Library.JPush.device
{
    public  class AliasDeviceListResult:BaseResult
    {
        public List<String> registration_ids ;
        public AliasDeviceListResult()
        {
            registration_ids = null;
        }
        public override bool isResultOK()
        {
            if (Equals(ResponseResult.responseCode, HttpStatusCode.OK))
            {
                return true;
            }
            return false;
        }
        public static AliasDeviceListResult fromResponse(ResponseWrapper responseWrapper)
        {
            AliasDeviceListResult aliasDeviceListResult = new AliasDeviceListResult();
            if (responseWrapper.isServerResponse())
            {
                aliasDeviceListResult = JsonConvert.DeserializeObject<AliasDeviceListResult>(responseWrapper.responseContent);
            }
            aliasDeviceListResult.ResponseResult = responseWrapper;
            return aliasDeviceListResult;
        }
    }
}
