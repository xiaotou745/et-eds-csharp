using Common.Logging;

namespace ETS.Data
{
	/// <summary>
	/// WMS.Data 日志管理类
	/// </summary>
	public class DbLogManager
	{
		/// <summary>
		/// 获取输出到ConnectionStringLog.txt文件的<see cref="ILog"/>
		/// </summary>
		/// <returns></returns>
		public static ILog GetConnectionStringLog()
		{
			return LogManager.GetLogger("ConnectionStringLog");
		}
	}
}