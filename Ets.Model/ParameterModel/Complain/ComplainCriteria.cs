using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;

namespace Ets.Model.ParameterModel.Complain
{
    public class ComplainCriteria : ListParaBase
    {
        private int isHandle = -1; //默认查询所有状态
        public string OrderNo { get; set; }

        public int ComplainType { get; set; }

        public string ComplainStartTime { get; set; }

        public string ComplainEndTime { get; set; }
        public string ClienterName { get; set; }
        public string BusinessName { get; set; }
        /// <summary>
        /// 城市id
        /// </summary>
        public string CityId { get; set; }

        public string businessCity { get; set; }

        public int IsHandle 
        {
            get { return isHandle; }
            set { isHandle = value; }
        }

        public string HandleOpinion { get; set; }        
    }
}
