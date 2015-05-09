using Ets.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    public class BusinessWithdrawSearchCriteria : ListParaBase
    {
        private int status = 0; //默认查询所有状态
        /// <summary>
        /// 商户名称
        /// </summary>
        public string BusinessName { get; set; }
        /// <summary>
        /// 商户电话
        /// </summary>
        public string BusinessPhoneNo { get; set; }
        /// <summary>
        /// 商户所在城市
        /// </summary>
        public string BusinessCity { get; set; }

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
    }
}
