using System.Collections.Generic;
using Ets.Model.Common;
using Ets.Model.DomainModel.Group;

namespace Ets.Model.ParameterModel.Group
{
    /// <summary>
    /// 集团分页列表实体对象-平扬 2015.3.16
    /// </summary>
    public class GroupListModel
    {
        public GroupListModel(IList<GroupApiConfigModel> _list, PagingResult pagingResult)
        {
            GroupApiConfigListModel = _list;
            PagingResult = pagingResult;
        }
        /// <summary>
        /// 集团列表
        /// </summary>
        public IList<GroupApiConfigModel> GroupApiConfigListModel { get; set; }
        /// <summary>
        /// 分页参数
        /// </summary>
        public PagingResult PagingResult { get; set; }
    }
}
