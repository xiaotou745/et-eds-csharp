
namespace Ets.Model.DataModel.Authority
{
    /// <summary>
    /// 角色权限对应关系实体--平扬,2013.3.19
    /// </summary>
    public class AuthorityRoleMentMenuSet
    {
        /// <summary>
        /// 关系id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 权限id
        /// </summary>
        public int MenuId { get; set; }
    }
}
