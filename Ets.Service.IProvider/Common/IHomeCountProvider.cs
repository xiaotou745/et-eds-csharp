using Ets.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Common
{
    public interface IHomeCountProvider
    {
        HomeCountTitleModel GetHomeCountTitle();
    }
}
