//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SuperManDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class account
    {
        public account()
        {
            this.accountauthority = new HashSet<accountauthority>();
        }
    
        public int Id { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> AccountType { get; set; }
        public Nullable<System.DateTime> FADateTime { get; set; }
        public string FAUser { get; set; }
        public Nullable<System.DateTime> LCDateTime { get; set; }
        public string LCUser { get; set; }
    
        public virtual ICollection<accountauthority> accountauthority { get; set; }
    }
}
