using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Bussiness
{

    /// <summary>
    /// 根据骑士id，查询骑士商家对应关系  查询条件实体 add by caoheyang  20150608 
    /// </summary>
    public class BCRelationGetByClienterIdPM
    {
        private int _pageIndex;
        public int PageIndex
        {
            get
            {
                return _pageIndex == 0 ? 1 : _pageIndex;
            }
            set { _pageIndex = value; }
        }
        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 骑士id
        /// </summary>
        public int ClienterId { get; set; }
    }
}
