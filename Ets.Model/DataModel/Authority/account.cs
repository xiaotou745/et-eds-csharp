using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Authority
{
    public class account
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
        public Nullable<int> GroupId { get; set; }
        public int RoleId { get; set; }
        public string GroupName { get; set; }

        public virtual ICollection<accountauthority> accountauthority { get; set; }
    }
}
