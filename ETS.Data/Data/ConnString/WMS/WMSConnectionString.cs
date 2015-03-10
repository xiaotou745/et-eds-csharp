using ETS.Data.ConnString.Common;

namespace ETS.Data.ConnString.WMS
{
	/// <summary>
	/// 表示Vancl.WMS中的连接字符串格式
	/// </summary>
	public class WMSConnectionString : IConnectionString
	{
		private string name;

		/// <summary>
		/// 获取或设置数据库类型（主库OR只读)
		/// </summary>
		public DatabaseType DatabaseType { get; set; }

		/// <summary>
		/// 获取或设置仓库Id
		/// </summary>
		public string WarehouseId { get; set; }

		/// <summary>
		/// 获取或设置枚举名称
		/// </summary>
		public string EnumValue { get; set; }

		/// <summary>
		/// 获取或设置数据库名称
		/// </summary>
		public string DatabaseName { get; set; }

		/// <summary>
		/// 获取或设置在Cookie中的仓库值
		/// </summary>
		public string WebCookieValue { get; set; }

		#region IConnectionString Members

		/// <summary>
		/// 获取或设置唯一标识一个WMS连接字符串的字符串名称
		/// EnumValue+DatabaseType唯一标识一个字符串（一个EnumValue分主库和只读两个串）
		/// </summary>
		public string Name
		{
			get
			{
				if (string.IsNullOrEmpty(name))
				{
					return EnumValue + DatabaseType;
				}
				return name;
			}
			set { name = value; }
		}

		/// <summary>
		/// 获取或设置WMS连接字符串（解密之后的字符串）
		/// </summary>
		public string ConnectionString { get; set; }

		/// <summary>
		/// 获取或设置服务器提供商名称；
		/// </summary>
		public string ProviderName { get; set; }

		#endregion

		/// <summary>
		/// To String.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("WMSConnectionString:Name:{0} WarehouseId:{1} DbType:{2} EnumValue:{3}",
			                     Name, WarehouseId, DatabaseType, EnumValue);
		}
	}
}