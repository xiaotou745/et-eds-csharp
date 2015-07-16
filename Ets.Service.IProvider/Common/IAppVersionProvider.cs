using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Data.PageData;
using Ets.Model.Common;
using Ets.Model.DomainModel.Common;
using Ets.Model.ParameterModel.Common;

namespace Ets.Service.IProvider.Common
{
    public interface IAppVersionProvider
    {
        /// <summary>
        /// 版本检查
        /// </summary>
        /// <param name="vcmodel"></param>
        /// <returns></returns>
        AppVerionModel VersionCheck(VersionCheckModel vcmodel);

        /// <summary>
        /// 分页查询App版本信息列表
        /// dannny-20150715
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        PageInfo<AppVerionModel> GetAppVersionList(AppVerionSearchCriteria criteria);

        /// <summary>
        /// 编辑APP版本信息（新增和修改）
        /// danny-20150715
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        DealResultInfo EditAppVersion(AppVersionModel model);

        /// <summary>
        /// 用Id查询升级信息
        /// danny-20150715
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        AppVerionModel GetAppVersionById(int Id);

        /// <summary>
        /// 修改App版本的发布状态为取消
        /// danny-20150715
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool CancelAppVersion(AppVersionModel model);
    }
}
