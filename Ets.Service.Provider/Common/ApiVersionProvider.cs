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
        public bool AddApiRecords(ApiVersionStatisticModel model)
        {
            try
            {
                return new ApiVersionDao().Add(model);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                return false;
            }
        }
    }
}
