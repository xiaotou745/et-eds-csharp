using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Business;

namespace Ets.Model.DomainModel.Business
{
    public class ClienterBindOptionLogModel : ClienterBindOptionLog
    {
        /// <summary>
        /// 是否绑定(0:否 1:是)
        /// </summary>
        public int IsBind { get; set; }
    }
}
