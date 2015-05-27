using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Authority;

namespace Ets.Model.ParameterModel.Authority
{
    public class AccountCriteria:account
    {
        /// <summary>
        /// 用户所有权限城市Code集合
        /// </summary>
        public string CityCodeList { get; set; }
        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OptUserName { get; set; }
        /// <summary>
        /// 操作类型（0：添加 1：修改）
        /// </summary>
        public string OptionType { get; set; }
    }
}
