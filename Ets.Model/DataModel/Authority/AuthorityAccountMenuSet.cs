
namespace Ets.Model.DataModel.Authority
{
    /// <summary>
    /// 个人账户权限对应关系实体--平扬,2013.3.19
    /// </summary>
    public class AuthorityAccountMenuSet
    {
        /// <summary>
        /// 关系id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 账户id
        /// </summary>
        public int AccoutId { get; set; }
        /// <summary>
        /// 权限id
        /// </summary>
        public int MenuId { get; set; }
    }
}
