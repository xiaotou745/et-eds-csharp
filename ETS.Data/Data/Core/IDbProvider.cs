using System;
using System.Data;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace ETS.Data.Core
{
	/// <summary>
	/// 表示一组方法，这些方法用于创建提供程序对数据源类的实现的实例。
	/// </summary>
	public interface IDbProvider
	{
		/// <summary>
		/// 获取或设置连接字符串
		/// </summary>
		string ConnectionString { get; set; }

		/// <summary>
		/// 指定特定的 <see cref="DbProviderFactory"/> 是否支持 <see cref="DbDataSourceEnumerator"/> 类。
		/// </summary>
		bool CanCreateDataSourceEnumerator { get; }

		/// <summary>
		/// 返回实现 <see cref="DbConnection"/> 类的提供程序的类的一个新实例。
		/// </summary>
		/// <param name="connectionString">连接字符串</param>
		/// <returns></returns>
		IDbConnection CreateConnection(string connectionString);

		/// <summary>
		/// 返回实现 <see cref="DbCommand"/> 类的提供程序的类的一个新实例。
		/// </summary>
		/// <returns></returns>
		IDbCommand CreateCommand();

		/// <summary>
		/// 返回实现 <see cref="DbCommandBuilder"/> 类的提供程序的类的一个新实例。
		/// </summary>
		/// <returns></returns>
		DbCommandBuilder CreateCommandBuilder();

		/// <summary>
		/// 返回实现 <see cref="DbConnection"/> 类的提供程序的类的一个新实例。
		/// </summary>
		/// <returns></returns>
		IDbConnection CreateConnection();

		/// <summary>
		/// 返回实现 <see cref="DbConnectionStringBuilder"/> 类的提供程序的类的一个新实例。
		/// </summary>
		/// <returns></returns>
		DbConnectionStringBuilder CreateConnectionStringBuilder();

		/// <summary>
		/// 返回实现 <see cref="DbDataAdapter"/> 类的提供程序的类的一个新实例。
		/// </summary>
		/// <returns></returns>
		IDbDataAdapter CreateDataAdapter();

		/// <summary>
		/// 返回实现 <see cref="DbParameter"/> 类的提供程序的类的一个新实例。
		/// </summary>
		/// <returns></returns>
		IDataParameter CreateParameter();

		/// <summary>
		/// 返回提供程序的类的新实例，该实例可实现提供程序的 <see cref="CodeAccessPermission"/> 类的版本。
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		CodeAccessPermission CreatePermission(PermissionState state);

		/// <summary>
		/// 返回实现 <see cref="DbDataSourceEnumerator"/> 类的提供程序的类的一个新实例。
		/// </summary>
		/// <returns></returns>
		DbDataSourceEnumerator CreateDataSourceEnumerator();

		/// <summary>
		/// 根据指定的providerInvariantName创建参数名称
		/// </summary>
		/// <param name="paramName"></param>
		/// <returns></returns>
		string CreateParameterNameForCollection(string paramName);

		/// <summary>
		/// 检测参数类型是否正确
		/// </summary>
		/// <param name="paramName"></param>
		/// <param name="paramType"></param>
		void CheckedParameterType(string paramName, Enum paramType);
	}
}