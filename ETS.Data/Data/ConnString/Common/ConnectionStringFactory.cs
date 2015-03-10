using System.Collections.Generic;
using Common.Logging;
using ETS.Util;

namespace ETS.Data.ConnString.Common
{
	/// <summary>
	/// 连接字符串工厂类
	/// </summary>
	internal class ConnectionStringFactory
	{
		#region Logger Definition.

		private static readonly ILog logger = DbLogManager.GetConnectionStringLog();

		#endregion

		/// <summary>
		/// 连接字符串列表
		/// </summary>
		private IList<IConnectionString> lstConnStrings;

		/// <summary>
		/// Create a instance of the class <see cref="ConnectionStringFactory"/>.
		/// </summary>
		/// <param name="connStringCreator">a instance of the interface <see cref="IConnectionStringCreator"/></param>
		/// <returns>return a instance of the class <see cref="ConnectionStringFactory"/>.</returns>
		public static ConnectionStringFactory Create(IConnectionStringCreator connStringCreator)
		{
			AssertUtils.ArgumentNotNull(connStringCreator, "connStringCreator");

			var connStringFactory = new ConnectionStringFactory();

			logger.InfoFormat("[ConnectionStringFactory] Create ConnectionStrings by Creator:{0}", connStringCreator);
			connStringFactory.lstConnStrings = connStringCreator.CreateConnStrings();

			return connStringFactory;
		}

		/// <summary>
		/// get list of connectionString created by the Creator.
		/// </summary>
		/// <returns></returns>
		public IList<IConnectionString> GetConnectionStrings()
		{
			return lstConnStrings;
		}

		/// <summary>
		/// get the <see cref="IConnectionString"/> by the connString name.
		/// </summary>
		/// <param name="name">a connectionString name.</param>
		/// <returns></returns>
		public IConnectionString GetConnectionString(string name)
		{
			AssertUtils.StringNotNullOrEmpty(name, "name");

			if (lstConnStrings == null || lstConnStrings.Count == 0)
			{
				return null;
			}
			foreach (IConnectionString connString in lstConnStrings)
			{
				if (connString.Name == name)
				{
					return connString;
				}
			}
			return null;
		}
	}
}