using System;

namespace Ets.Model.DataModel.Group
{
    public class GroupModel
    {
        public long Id { get; set; }
        public string GroupName { get; set; }
        public string CreateName { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string ModifyName { get; set; }
        public Nullable<System.DateTime> ModifyTime { get; set; }
        public Nullable<byte> IsValid { get; set; }
        /// <summary>
        /// 是否允许修改第三方绑定
        /// 0：否  1：是
        /// </summary>
        public int IsModifyBind { get; set; }
        
    }
}
