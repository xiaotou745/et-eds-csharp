using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Common;
using ETS.Data.PageData;
using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DomainModel.Common;
using Ets.Model.ParameterModel.Clienter;
using Ets.Model.ParameterModel.Common;
using Ets.Service.IProvider.Common;

namespace Ets.Service.Provider.Common
{
    public class AppVersionProvider : IAppVersionProvider
    {
        AppVersionDao appVersionDao = new AppVersionDao();
        /// <summary>
        /// 版本检查
        /// </summary>
        /// <param name="vcmodel"></param>
        /// <returns></returns>
        public AppVerionModel VersionCheck(VersionCheckModel vcmodel)
        {
            return appVersionDao.VersionCheck(vcmodel);
        }

        /// <summary>
        /// 分页查询App版本信息列表
        /// dannny-20150715
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<AppVerionModel> GetAppVersionList(AppVerionSearchCriteria criteria)
        {
            return appVersionDao.GetAppVersionList<AppVerionModel>(criteria);
        }
        /// <summary>
        /// 编辑APP版本信息（新增和修改）
        /// danny-20150715
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DealResultInfo EditAppVersion(AppVersionModel model)
        {
            var dealReg = new DealResultInfo
            {
                DealFlag = false
            };
            if (model.DealType == 0)
            {
                dealReg.DealFlag = appVersionDao.AddAppVersion(model);
                dealReg.DealMsg = dealReg.DealFlag ? "App版本添加成功！" : "APP版本添加失败！";
            }
            else
            {
                dealReg.DealFlag = appVersionDao.ModifyAppVersion(model);
                dealReg.DealMsg = dealReg.DealFlag ? "App版本修改成功！" : "App版本修改失败！";
            }
            return dealReg;

        }

        /// <summary>
        /// 用Id查询升级信息
        /// danny-20150715
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AppVerionModel GetAppVersionById(int Id)
        {
            return appVersionDao.GetAppVersionById(Id);
        }

        /// <summary>
        /// 修改App版本的发布状态为取消
        /// danny-20150715
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CancelAppVersion(AppVersionModel model)
        {
            return appVersionDao.CancelAppVersion(model);
        }

    }
}
