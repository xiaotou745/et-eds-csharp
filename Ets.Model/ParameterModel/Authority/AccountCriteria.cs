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
    }
}
