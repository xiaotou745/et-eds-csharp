using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.DeliveryCompany
{
    /// <summary>
    /// 批量导入骑士返回页面实体（异步确人excel数据是否合法部分）  excel  处理  add by caoheyang 20150706
    /// </summary>
    public class BatchImportClienterExcelDM
    {
        /// <summary>
        /// 手机号码    
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 名称  
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
