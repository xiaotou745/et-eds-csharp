using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DomainModel.DeliveryCompany;

namespace Ets.Model.ParameterModel.DeliveryCompany
{
    /// <summary>
    /// 物流公司批量导入骑士  参数实体  add by caoheyang 20150707
    /// </summary>
    public class DoBatchImportClienterPM
    {
        /// <summary>
        /// 登录人id
        /// </summary>
        public int OptId { get; set; }
        /// <summary>
        /// 登录人名称
        /// </summary>
        public string OptName { get; set; }
        /// <summary>
        /// 公司id
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 骑士集合
        /// </summary>
        public List<BatchImportClienterExcelDM> Datas { get; set; }
        
    }
}
