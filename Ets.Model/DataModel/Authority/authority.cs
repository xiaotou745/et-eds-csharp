using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Authority
{
    public class authority
    {
        public authority()
        {
            this.accountauthority = new HashSet<accountauthority>();
        }

        public int Id { get; set; }
        public int AuthorityType { get; set; }
        public string Name { get; set; }

        public virtual ICollection<accountauthority> accountauthority { get; set; }
    }
}
