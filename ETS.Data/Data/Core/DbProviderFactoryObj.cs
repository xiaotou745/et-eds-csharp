using System.Collections;
using Common.Logging;

namespace ETS.Data.Core
{
	/// <summary>
	/// 表示一组静态方法，这些方法用于创建 <see cref="IDbProvider"/> 类的一个或多个实例。
	/// </summary>
	public class DbProviderFactoryObj
	{
		#region Logging Definition

		/// <summary>
		/// get the <see cref="ILog"/> with a WYC.Data.txt file appender
		/// </summary>
		protected static readonly ILog Logger = DbLogManager.GetConnectionStringLog();

		#endregion

		/// <summary>
		/// 用来缓存<see cref="DbProvider"/>实例
		/// </summary>
		private static readonly Hashtable hashtable = new Hashtable();

		/// <summary>
		/// SQL Server ProviderName
		/// </summary>
		public const string SQL_CLIENT_PROVIDERINVARIANT_NAME = "System.Data.SqlClient";

        /// <summary>
        /// MySql ProviderName
        /// </summary>
        public const string MYSQL_CLIENT_PROVIDERINVARIANT_NAME = "MySql.Data.MySqlClient";


		/// <summary>
		/// 返回默认的<see cref="DbProvider"/>实例，该实例以<see cref="System.Data.SqlClient"/>为providerInvariantName
		/// </summary>
		/// <returns></returns>
		public static IDbProvider GetProvider()
		{
			return GetProvider(SQL_CLIENT_PROVIDERINVARIANT_NAME);
		}

		/// <summary>
		/// 返回一个<see cref="DbProvider"/>实例，该实例以指定参数<paramref name="providerInvariantName"/>为ProviderName
		/// </summary>
		/// <param name="providerInvariantName"></param>
		/// <returns></returns>
		public static IDbProvider GetProvider(string providerInvariantName)
		{
			IDbProvider dbProvider = null;
			if (hashtable.Contains(providerInvariantName))
			{
				Logger.DebugFormat(
					"[DbProviderFactoryObj.GetProvider]:get dbprovider from the cach of hashtable,providerInvariantName[{0}].",
					providerInvariantName);
				dbProvider = hashtable[providerInvariantName] as IDbProvider;
			}
			if (dbProvider == null)
			{
				Logger.InfoFormat("[DbProviderFactoryObj.GetProvider]:Creating dbprovider with the providerInvariantName[{0}].",
				                   providerInvariantName);
				dbProvider = new DbProvider(providerInvariantName);
				hashtable[providerInvariantName] = dbProvider;
			}
			return dbProvider;
		}
	}
}