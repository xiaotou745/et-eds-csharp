using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    /// <summary>
    ///  商户修改绑定银行卡功能 参数实体 add by caoheyang 20150511
    /// </summary>
    public class CardModifyBPM
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 商家ID(business表)
        /// </summary>
        public int BusinessId { get; set; }

        /// <summary>
        /// 户名
        /// </summary>
        public string TrueName { get; set; }
         
        /// <summary>
        /// 卡号(需要DES加密)
        /// </summary>
        public string AccountNo { get; set; } 

        /// <summary>
        /// 第二次录入卡号(需要DES加密)
        /// </summary>
        public string AccountNo2 { get; set; }


        /// <summary>
        /// 开户行
        /// </summary>
        public string OpenBank { get; set; }

        /// <summary>
        /// 开户支行
        /// </summary>
        public string OpenSubBank { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        public string UpdateBy { get; set; }

    }
}
