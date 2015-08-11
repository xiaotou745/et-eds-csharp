using ETS.Enums;
using Ets.Model.Common;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using ETS.Expand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ETS.NoSql.RedisCache;
namespace SuperManWebApi.Controllers
{
    public class CommonController : ApiController
    {
        readonly ITokenProvider iTokenProvider = new TokenProvider();
        
        /// <summary>
        /// 版本升级检测 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResultModel<AppVerionModel> VersionCheck(VersionCheckModel vcmodel)
        {
            //参数校验
            if (vcmodel == null || vcmodel.PlatForm == 0)
            {
            http://www.yy.com/90559774
                return ResultModel<AppVerionModel>.Conclude(VersionStatus.NoPlatForm);
            }
            if (vcmodel.UserType == 0)
            {
                return ResultModel<AppVerionModel>.Conclude(VersionStatus.NoUserType);
            }
            //查询数据
            AppVersionProvider appVersionProvider = new AppVersionProvider();
            var result = appVersionProvider.VersionCheck(vcmodel);
            if (result == null)
            {
                return ResultModel<AppVerionModel>.Conclude(VersionStatus.NoData);
            }
            return ResultModel<AppVerionModel>.Conclude(VersionStatus.Success, result);
        }

        [HttpPost]
        public ResultModel<string> GetToken(TokenModel model)
        {
            //参数校验
            if (string.IsNullOrEmpty(model.Ssid))
            {
                return ResultModel<string>.Conclude(TokenStatus.NoSsid);
            }
            if (string.IsNullOrEmpty(model.Appkey))
            {
                return ResultModel<string>.Conclude(TokenStatus.NoAppkey);
            }
            string cacheValue = iTokenProvider.GetToken(model);

            return ResultModel<string>.Conclude(TokenStatus.Success, cacheValue);
            
        }
        
    }
}
