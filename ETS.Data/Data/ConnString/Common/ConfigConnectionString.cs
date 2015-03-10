namespace ETS.Data.ConnString.Common
{
	/// <summary>
	/// 表示应用程序配置文件中的ConnectionString配置节定义的字符串
	/// </summary>
	/// <example>
	/// <code>
	/// <connectionStrings>
	///		<add name="dbConfigConnStringTest" connectionString="" providerName="System.Data.SqlClient"/>
	/// </connectionStrings>
	/// </code>
	/// </example>
	public class ConfigConnectionString : IConnectionString
	{
		#region IConnectionString Members

		/// <summary>
		/// 获取或设置唯一定义一个连接字符串的字符串名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 获取或设置连接字符串内容
		/// </summary>
		public string ConnectionString { get; set; }

		/// <summary>
		/// 获取或设置Provider
		/// </summary>
		public string ProviderName { get; set; }

		#endregion

		public override string ToString()
		{
			return string.Format("ConfigConnString:[Name:{0}]-[ConnectionString:{1}]-[ProviderName:{2}]", Name, ConnectionString,
			                     ProviderName);
		}
	}
}