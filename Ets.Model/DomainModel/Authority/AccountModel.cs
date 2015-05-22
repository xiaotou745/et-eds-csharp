using Ets.Model.DataModel.Authority;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Authority
{
    public class AccountModel:account
    {
        /// <summary>
        /// 用户所有权限城市Code集合
        /// </summary>
        public string CityCodeList { get; set; }
    }
}
