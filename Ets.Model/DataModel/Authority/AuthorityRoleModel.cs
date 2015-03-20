
namespace Ets.Model.DataModel.Authority
{
    /// <summary>
    /// 部门角色实体--平扬,2013.3.18
    /// </summary>
    public class AuthorityRoleModel
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool BeLock { get; set; }
    }
}
