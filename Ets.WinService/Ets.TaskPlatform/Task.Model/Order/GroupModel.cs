using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task.Model.Order
{
    public class GroupModel : QueryResult
    {
        public long Id { get; set; }
        public string GroupName { get; set; }
        public string CreateName { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string ModifyName { get; set; }
        public Nullable<System.DateTime> ModifyTime { get; set; }
        public Nullable<byte> IsValid { get; set; }
    }
}
