using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.GlobalConfig
{
    public class GlobalConfigSubsidies
    {
        public int Id { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public int GroupId { get; set; }
        public int StrategyId { get; set; }
    }
}
