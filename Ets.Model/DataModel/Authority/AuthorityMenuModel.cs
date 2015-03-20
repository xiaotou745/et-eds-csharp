namespace Ets.Model.DataModel.Authority
{
    /// <summary>
    /// 权限菜单实体--平扬,2013.3.19
    /// </summary>
    public class AuthorityMenuModel
    {
        /// <summary>
        /// 权限id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        public int ParId { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool BeLock { get; set; }
        /// <summary>
        /// 菜单地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 是否按钮
        /// </summary>
        public bool IsButton { get; set; }

    }
}
