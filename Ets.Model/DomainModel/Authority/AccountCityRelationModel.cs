using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Authority;

namespace Ets.Model.DomainModel.Authority
{
    public class AccountCityRelationModel : AccountCityRelation
    {
        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }
    }
}
