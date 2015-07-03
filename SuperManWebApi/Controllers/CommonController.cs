using ETS.Enums;
using Ets.Model.Common;
using Ets.Service.Provider.Common;
using ETS.Expand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SuperManWebApi.Controllers
{
    public class CommonController : ApiController
    {
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
    }
}
