using Ets.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Common
{
    public class OpenCitySearchCriteria : ListParaBase
    {
        public NewPagingResult PagingRequest { get; set; }
        public string CityName { get; set; }
    }
}
