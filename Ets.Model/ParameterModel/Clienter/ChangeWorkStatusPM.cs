using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Clienter
{
    public class ChangeWorkStatusPM
    {
        /// <summary>
        ///超人id
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        ///目标工作状态
        /// </summary>
        public int? WorkStatus { get; set; }
        /// <summary>
        /// 订单状态    
        /// </summary>
        public int? OrderStatus { get; set; }
    }
}
