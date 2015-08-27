using System;
using System.Collections.Generic;
using System.Net;
using cn.jpush.api.common;
using Newtonsoft.Json;
using BaseResult = ETS.Library.JPush.common.BaseResult;
using ResponseWrapper = ETS.Library.JPush.common.ResponseWrapper;

namespace ETS.Library.JPush.device
{
  public  class TagAliasResult:BaseResult
    {
        public List<String> tags;
        public String alias;

        public TagAliasResult()
        {
            tags = null;
            alias = null;
        }
        public override bool isResultOK()
        {
            if (Equals(ResponseResult.responseCode, HttpStatusCode.OK))
            {
                return true;
            }
            return false;
        }
        public static TagAliasResult fromResponse(ResponseWrapper responseWrapper)
        {
            TagAliasResult tagAliasResult = new TagAliasResult();
            if (responseWrapper.isServerResponse())
            {
                tagAliasResult = JsonConvert.DeserializeObject<TagAliasResult>(responseWrapper.responseContent);
            }
            tagAliasResult.ResponseResult = responseWrapper;
            return tagAliasResult;
        }

    }
}
