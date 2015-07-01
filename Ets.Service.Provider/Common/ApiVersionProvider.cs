using System;
using Ets.Dao.Common;
using Ets.Model.Common;
using ETS.Util;

namespace Ets.Service.Provider.Common
{
    public class ApiVersionProvider
    {
        /// <summary>
        /// 添加接口调用记录
        /// 平扬-20150401
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddApiRecords(ApiVersionStatisticModel model)
        {
            int id = 0;
            try
            {
                id = new ApiVersionDao().Add(model);               
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);                
            }
            return id;
        }
    }
}
