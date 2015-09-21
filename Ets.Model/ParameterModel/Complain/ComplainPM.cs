using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Complain
{   
    public class ComplainPM
    {      
        /// <summary>
        /// Id
        /// </summary>     
        public int Id { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 是否处理
        /// </summary>
        public int IsHandle
        {
            get;
            set;
        }
        
        /// 处理意见
        /// </summary>        
        public string HandleOpinion { get; set; }
    }

}
