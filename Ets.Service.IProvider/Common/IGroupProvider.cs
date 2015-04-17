using Ets.Model.Common;
using Ets.Model.DataModel.Group;
using Ets.Model.DomainModel.Group;
using Ets.Model.ParameterModel.Group;
using ETS.Data.PageData;

namespace Ets.Service.IProvider.Common
{
    /// <summary>
    /// 集团操作接口-平扬 2015.3.16添加
    /// </summary>
    public interface IGroupProvider
    {
        /// <summary>
        /// 获取集团分页列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        PageInfo<GroupApiConfigModel> GetGroupList(GroupParaModel criteria);
        /// <summary>
        /// 创建集团Api配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns>配置id</returns>
        ResultInfo<bool> CreateGroupApiConfig(GroupApiConfigModel config);
        /// <summary>
        /// 创建集团
        /// </summary>
        /// <param name="config"></param>
        /// <returns>配置id</returns>
        ResultInfo<int> AddGroup(GroupModel config);
        /// <summary>
        /// 修改集团api配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        ResultInfo<bool> UpdateGroupApiConfig(GroupApiConfigModel config);
        /// <summary>
        /// 根据appkey和版本查询配置信息
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        GroupApiConfigModel GetGroupApiConfigByAppKey(string appkey, string version);

        /// <summary>
        /// 更新集团状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultInfo<bool> UpdateGroupStatus(GroupModel model);
        /// <summary>
        /// 更新集团名称
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultInfo<bool> UpdateGroupName(GroupModel model);
        /// <summary>
        /// 判断集团是否已经存在
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultInfo<bool> HasExistsGroup(GroupModel model);
    }
}
