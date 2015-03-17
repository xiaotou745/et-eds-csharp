using Ets.Model.Common;

namespace Ets.Model.ParameterModel.Group
{
    /// <summary>
    /// 集团查询条件实体-平扬 2015.3.16
    /// </summary>
    public class GroupParaModel
    {
        /// <summary>
        /// app_key唯一值
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 集团名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 分页
        /// </summary>
        public NewPagingResult PagingRequest { get; set; }
    }
}
