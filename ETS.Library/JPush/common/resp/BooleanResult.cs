using Newtonsoft.Json;

namespace ETS.Library.JPush.common.resp
{
    public class BooleanResult : ETS.Library.JPush.common.resp.DefaultResult 
    {
	     public bool result;
         new public static BooleanResult fromResponse(ETS.Library.JPush.common.ResponseWrapper responseWrapper)
         {
             BooleanResult tagListResult = new BooleanResult();
             if (responseWrapper.isServerResponse())
             {
                 tagListResult = JsonConvert.DeserializeObject<BooleanResult>(responseWrapper.responseContent);
             }
             tagListResult.ResponseResult = responseWrapper;
             return tagListResult;
         }
    }

}
