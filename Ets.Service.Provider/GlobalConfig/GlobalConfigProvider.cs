using Ets.Dao.GlobalConfig;
using Ets.Model.DomainModel.GlobalConfig;
using Ets.Service.IProvider.GlobalConfig;
namespace Ets.Service.Provider.GlobalConfig
{
    public class GlobalConfigProvider : IGlobalConfigProvider
    {
        readonly GlobalConfigDao globalConfigDao = new GlobalConfigDao();

        /// <summary>
        /// 获取公共配置(从数据库读取数据) 
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150707</UpdateTime>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        public GlobalConfigModel GetGlobalConfig(int GroupId)
        {
            GlobalConfigModel model = null;

            model = globalConfigDao.GlobalConfigMethod(GroupId);

            return model;
        }
    }
}
