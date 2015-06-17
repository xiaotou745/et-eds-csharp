using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.ParameterModel.Common;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;

namespace Ets.ExportData
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new ExportDataService() 
            };

            IExportSqlManageProvider xx = new ExportSqlManageProvider();
            xx.QueryForWindows( new DataManageSearchCriteria());
            ServiceBase.Run(ServicesToRun);
        }
    }
}
