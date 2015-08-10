using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Statistics
{
   public class ActiveUserQuery
    {
       public DateTime StartDate { get; set; }
       public DateTime EndDate { get; set; }
       public int UserType { get; set; }
       public int InfoType { get; set; }
       public string UserInfo { get; set; }
       public int PageIndex { get; set; }

    }
}
