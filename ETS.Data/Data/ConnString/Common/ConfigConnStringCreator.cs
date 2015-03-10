using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Common.Logging;
using ETS.Dao;

namespace ETS.Data.ConnString.Common
{
	/// <summary>
	/// 用来创建应用程序配置文件中的ConnectionString配置节中定义的字符串
	/// </summary>
	public class ConfigConnStringCreator : IConnectionStringCreator
	{
		#region Logger Definition

		private readonly ILog logger = DbLogManager.GetConnectionStringLog();

		#endregion

		#region Private Constructor.

		private ConfigConnStringCreator()
		{
		}

		#endregion

		#region IConnectionStringCreator Members

		/// <summary>
		/// 创建应用程序配置文件中的ConnectionString配置节中定义的字符串
		/// </summary>
		/// <returns>返回ConnectionString配置节中定义的所有字符串列表.</returns>
		public IList<IConnectionString> CreateConnStrings()
		{
			try
			{
				IList<IConnectionString> result = new List<IConnectionString>();

				foreach (ConnectionStringSettings connectionString in ConfigurationManager.ConnectionStrings)
				{
					var connString = new ConfigConnectionString
					{
						Name = connectionString.Name,
						ConnectionString = connectionString.ConnectionString,
						ProviderName = connectionString.ProviderName
					};
					result.Add(connString);
				}

				if (logger.IsInfoEnabled)
				{
					StringBuilder logText = new StringBuilder();
					logText.AppendLine(string.Format("配置文件的[connectionStrings]配置节中共有字符串数量：{0}.", result.Count));
					foreach (IConnectionString connectionString in result)
					{
						logText.AppendLine(string.Format("ConfigConnString:{0}", connectionString));
					}
					logger.Info(logText.ToString());
				}
				return result;
			}
			catch(Exception exception)
			{
				string errorMsg = string.Format("[ConfigConnStringCreator.CreateConnStrings] Error:{0}", exception.Message);
				logger.Error(errorMsg);
				throw new ConnStringCreateException(errorMsg, exception);
			}
		}

		#endregion

		/// <summary>
		/// Create a instance of class <see cref="ConfigConnStringCreator"/>
		/// </summary>
		/// <returns></returns>
		public static ConfigConnStringCreator Create()
		{
			return new ConfigConnStringCreator();
		}

		/// <summary>
		/// ToString Override.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return typeof (ConfigConnStringCreator).ToString();
		}
	}
}