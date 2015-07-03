using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Common;
using Ets.Model.Common;

namespace Ets.Service.Provider.Common
{
    public class AppVersionProvider
    {
        /// <summary>
        /// 版本检查
        /// </summary>
        /// <param name="vcmodel"></param>
        /// <returns></returns>
        public AppVerionModel VersionCheck(VersionCheckModel vcmodel)
        {
            AppVersionDao appVersionDao=new AppVersionDao();
            return appVersionDao.VersionCheck(vcmodel);
        }
    }
}
