using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Common.Logging;
using ETS.Dao;

namespace ETS.Data.ConnString.Common
{
	/// <summary>
	/// ��������Ӧ�ó��������ļ��е�ConnectionString���ý��ж�����ַ���
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
		/// ����Ӧ�ó��������ļ��е�ConnectionString���ý��ж�����ַ���
		/// </summary>
		/// <returns>����ConnectionString���ý��ж���������ַ����б�.</returns>
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
					logText.AppendLine(string.Format("�����ļ���[connectionStrings]���ý��й����ַ���������{0}.", result.Count));
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