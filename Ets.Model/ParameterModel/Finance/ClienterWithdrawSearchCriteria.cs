using Ets.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    public class ClienterWithdrawSearchCriteria : ListParaBase
    {
        private int status = 0; //默认查询所有状态
        /// <summary>
        /// 骑士名称
        /// </summary>
        public string ClienterName { get; set; }
        /// <summary>
        /// 骑士电话
        /// </summary>
        public string ClienterPhoneNo { get; set; }

        public int WithdrawStatus  //商家提款状态
        {
            get { return status; }
            set { status = value; }
        }
        /// <summary>
        /// 申请提款时间起
        /// </summary>
        public string WithdrawDateStart { get; set; }
        /// <summary>
        /// 申请提款时间止
        /// </summary>
        public string WithdrawDateEnd { get; set; }
        /// <summary>
        /// 提款单号
        /// </summary>
        public string WithwardNo { get; set; }
        /// <summary>
        /// 商户城市
        /// </summary>
        public string businessCity { get; set; }
        /// <summary>
        /// 提现、审核、提款日期类型
        /// </summary>
        public int ClientWithdrawDate { get; set; }
    }
}
