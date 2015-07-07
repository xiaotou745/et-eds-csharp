using Ets.Model.DomainModel.GlobalConfig;

namespace Ets.Service.IProvider.GlobalConfig
{
    public interface IGlobalConfigProvider
    {
        /// <summary>
        /// 获取公共配置(从数据库读取数据) 
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150707</UpdateTime>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        GlobalConfigModel GetGlobalConfig(int GroupId);       
    }
}
