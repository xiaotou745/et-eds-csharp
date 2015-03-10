using System;
using System.Data;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;
using ETS.Dao;
using ETS.Util;

namespace ETS.Data.Core
{
	/// <summary>
	/// 表示一组方法，这些方法用于创建提供程序对数据源类的实现的实例。
	/// 通过使用<see cref="DbProviderFactory"/>来实现<see cref="IDbProvider"/>接口
	/// </summary>
	public class DbProvider : IDbProvider
	{
		private readonly DbProviderFactory dbProviderFactory;

		private readonly string providerInvariantName;
		private string connString = string.Empty;

		/// <summary>
		/// 使用指定的providerInvariantName实现，如System.Data.SqlClient
		/// </summary>
		/// <param name="providerInvariantName"></param>
		public DbProvider(string providerInvariantName)
		{
			this.providerInvariantName = providerInvariantName;
			dbProviderFactory = DbProviderFactories.GetFactory(providerInvariantName);
		}

		#region IDbProvider Members

		/// <summary>
		/// 指定特定的 <see cref="DbProviderFactory"/> 是否支持 <see cref="DbDataSourceEnumerator"/> 类。
		/// </summary>
		public bool CanCreateDataSourceEnumerator
		{
			get { return dbProviderFactory.CanCreateDataSourceEnumerator; }
		}

		/// <summary>
		/// 返回实现 <see cref="DbCommand"/> 类的提供程序的类的一个新实例。
		/// </summary>
		/// <returns></returns>
		public IDbCommand CreateCommand()
		{
			return dbProviderFactory.CreateCommand();
		}

		/// <summary>
		/// 返回实现 <see cref="DbCommandBuilder"/> 类的提供程序的类的一个新实例。
		/// </summary>
		/// <returns></returns>
		public DbCommandBuilder CreateCommandBuilder()
		{
			return dbProviderFactory.CreateCommandBuilder();
		}

		/// <summary>
		/// 返回实现 <see cref="DbConnection"/> 类的提供程序的类的一个新实例。
		/// </summary>
		/// <param name="connectionString">连接字符串</param>
		/// <returns></returns>
		public IDbConnection CreateConnection(string connectionString)
		{
			connString = connectionString;
			IDbConnection dbConnection = CreateConnection();
			if (dbConnection != null)
			{
				dbConnection.ConnectionString = connString;
			}
			return dbConnection;
		}

		/// <summary>
		/// 返回实现 <see cref="DbConnection"/> 类的提供程序的类的一个新实例。
		/// </summary>
		/// <returns></returns>
		public IDbConnection CreateConnection()
		{
			return dbProviderFactory.CreateConnection();
		}

		/// <summary>
		/// 返回实现 <see cref="DbConnectionStringBuilder"/> 类的提供程序的类的一个新实例。
		/// </summary>
		/// <returns></returns>
		public DbConnectionStringBuilder CreateConnectionStringBuilder()
		{
			return dbProviderFactory.CreateConnectionStringBuilder();
		}

		/// <summary>
		/// 返回实现 <see cref="DbDataAdapter"/> 类的提供程序的类的一个新实例。
		/// </summary>
		/// <returns></returns>
		public IDbDataAdapter CreateDataAdapter()
		{
			return dbProviderFactory.CreateDataAdapter();
		}

		/// <summary>
		/// 返回实现 <see cref="DbParameter"/> 类的提供程序的类的一个新实例。
		/// </summary>
		/// <returns></returns>
		public IDataParameter CreateParameter()
		{
			return dbProviderFactory.CreateParameter();
		}

		/// <summary>
		/// 返回提供程序的类的新实例，该实例可实现提供程序的 <see cref="CodeAccessPermission"/> 类的版本。
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public CodeAccessPermission CreatePermission(PermissionState state)
		{
			return dbProviderFactory.CreatePermission(state);
		}

		/// <summary>
		/// 返回实现 <see cref="DbDataSourceEnumerator"/> 类的提供程序的类的一个新实例。
		/// </summary>
		/// <returns></returns>
		public DbDataSourceEnumerator CreateDataSourceEnumerator()
		{
			return dbProviderFactory.CreateDataSourceEnumerator();
		}

		/// <summary>
		/// 获取连接字符串
		/// </summary>
		public string ConnectionString
		{
			get { return connString; }
			set { connString = value; }
		}

		public string CreateParameterNameForCollection(string paramName)
		{
			return DbParameterManager.Create(providerInvariantName).GetParameterName(paramName);
		}

		public void CheckedParameterType(string paramName, Enum paramType)
		{
			DbParameterManager.Create(providerInvariantName).CheckedParameterType(paramName, paramType);
		}

		#endregion

		#region Nested type: DbParameterManager

		private class DbParameterManager
		{
			private static string providerInvariantName;

			private DbParameterManager()
			{
			}

			public static DbParameterManager Create(string providerName)
			{
				providerInvariantName = providerName;
				return new DbParameterManager();
			}

			public string GetParameterName(string parameterName)
			{
				AssertUtils.StringNotNullOrEmpty(parameterName, "parameterName");
				switch (providerInvariantName)
				{
					case DbProviderFactoryObj.SQL_CLIENT_PROVIDERINVARIANT_NAME:
						if (parameterName.StartsWith("@"))
						{
							return parameterName;
						}
						return "@" + parameterName;
				}
				return parameterName;
			}

			public void CheckedParameterType(string paramName, Enum parameterType)
			{
				Type paramType;
				switch (providerInvariantName)
				{
					case DbProviderFactoryObj.SQL_CLIENT_PROVIDERINVARIANT_NAME:
						paramType = typeof (SqlDbType);
						break;
					default:
						paramType = typeof (SqlDbType);
						break;
				}
				if (parameterType.GetType() != paramType)
				{
					throw new TypeMismatchDataAccessException("Invalid parameter type specified for parameter name ["
					                                          + paramName + "].  ["
					                                          + parameterType.GetType().AssemblyQualifiedName +
					                                          "] is not of expected type ["
					                                          + paramType.AssemblyQualifiedName + "]");
				}
			}
		}

		#endregion
	}
}