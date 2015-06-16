using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Finance;
namespace Ets.Model.DomainModel.Business
{
    [Serializable]
    public class BusinessBindClienterDM
    {
        public BusinessBindClienterDM() { }
		/// <summary>
		/// 行号
		/// </summary>
		public int RowCount { get; set; }
		/// <summary>
		/// 名称
		/// </summary>
        public string ClienterName { get; set; }

		/// <summary>
		/// 骑士电话
		/// </summary>
        public string ClienterPhoneNo { get; set; }

        /// <summary>
        /// 上传备注
        /// </summary>
        public string ClienterRemarks { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 是否绑定
        /// </summary>
        public bool IsBind { get; set; }
    }
   
}
