namespace Ets.Model.DataModel.Authority
{
    /// <summary>
    /// 部门实体类--平扬,2013.3.18
    /// </summary>
    public class AuthorityDepartmentModel
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        public int ParId { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string DepartName { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool BeLock { get; set; }
    }
}
