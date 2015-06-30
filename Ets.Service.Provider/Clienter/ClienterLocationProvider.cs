using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Clienter;
using Ets.Dao.GlobalConfig;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.ParameterModel.Business;
using Ets.Service.IProvider.Clienter;
using ETS.Util;

namespace Ets.Service.Provider.Clienter
{
    public class ClienterLocationProvider : IClienterLocationProvider
    {

        readonly ClienterLocationDao clienterLocationDao = new ClienterLocationDao();

        /// <summary>
        /// 插入骑士运行轨迹
        /// </summary>
        /// <UpdateBy>caoheyang</UpdateBy>
        /// <UpdateTime>20150519</UpdateTime>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> InsertLocaltion(ClienterPushLocaltionPM model)
        {
            try
            {
                long id = clienterLocationDao.Insert(TranslateInsertModel(model));
                return ResultModel<object>.Conclude(SystemEnum.Success,
                    new { PushTime = GlobalConfigDao.GlobalConfigGet(0).UploadTimeInterval });
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                return ResultModel<object>.Conclude(SystemEnum.SystemError,
                      new { PushTime = 0});
            }

        }

        /// <summary>
        /// 翻译插入骑士运行轨迹实体
        /// </summary>
        /// <UpdateBy>caoheyang</UpdateBy>
        /// <UpdateTime>20150519</UpdateTime>
        /// <param name="clienterPushLocaltionPm">参数实体</param>
        /// <returns></returns>
        private ClienterLocation TranslateInsertModel(ClienterPushLocaltionPM clienterPushLocaltionPm)
        {
            ClienterLocation temp = new ClienterLocation();
            temp.ClienterId = clienterPushLocaltionPm.ClienterId;
            temp.Longitude = clienterPushLocaltionPm.Longitude;
            temp.Latitude = clienterPushLocaltionPm.Latitude;
            return temp;
        }

    }

}
