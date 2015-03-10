using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Common.Logging;
using ETS.Data.ConnString.Common;
using ETS.Util;

namespace ETS.Data.ConnString.WMS
{
	/// <summary>
	/// 
	/// </summary>
	public class WMSConnStringUtil
	{
		/// <summary>
		/// 重试次数
		/// </summary>
		private const int RETRY_COUNT = 5;

		/// <summary>
		/// [key:connStringName,value:connectionString]
		/// </summary>
		private static readonly IDictionary<string, string> connStringCache = new Dictionary<string, string>();

		/// <summary>
		/// [key:connStringName,value:dbProviderName]
		/// </summary>
		private static readonly IDictionary<string, string> providerNameCache = new Dictionary<string, string>();

		/// <summary>
		/// [key:warehouseId,value:connStringName]
		/// </summary>
		private static readonly IDictionary<string, string> warehouseIdCache = new Dictionary<string, string>();

        /// <summary>
        /// [key:warehouseId,value:connStringName]
        /// </summary>
        private static readonly IDictionary<string, string> warehouseIdCacheReadonly = new Dictionary<string, string>();
		/// <summary>
		/// lock object.
		/// </summary>
		private static readonly object DbListObj = new object();

		/// <summary>
		/// the WMS Connection Strings.
		/// </summary>
		private static IList<WMSConnectionString> lstWMSConnStrings;

		#region Logger Definition.

		private static readonly ILog logger = DbLogManager.GetConnectionStringLog();

		#endregion

		#region Init WMSConnectionStrings.

		/// <summary>
		/// 初始化WMS连接字符串
		/// </summary>
		public static void InitConnList()
		{
			//获取连接字符串是否成功
			bool getConnectionSuccess = false;
			//获取连接字符串次数
			int getConnectionCounter = 0;

			while (!getConnectionSuccess && getConnectionCounter <= RETRY_COUNT)
			{
				try
				{
					logger.InfoFormat("第{0}次获取WMS连接字符串开始...", getConnectionCounter + 1);
					lock (DbListObj)
					{
						IList<WMSConnectionString> lstWmsConnStrings = new List<WMSConnectionString>();

						IList<IConnectionString> connStringDefinitions = ConnectionStringFactory.Create(WMSConnStringCreator.Create())
							.GetConnectionStrings();

						foreach (IConnectionString connStringDefinition in connStringDefinitions)
						{
							lstWmsConnStrings.Add(connStringDefinition as WMSConnectionString);
						}

						if (lstWmsConnStrings.Count == 0)
						{
							throw new Exception("没有要使用的WMS字符串.WMS数据库连接表查询异常");
						}

						lstWMSConnStrings = lstWmsConnStrings;
						//日志
						if (logger.IsInfoEnabled)
						{
							var stringBuilder = new StringBuilder();
							stringBuilder.Append(string.Format("WMSConnectionString初始化完成，共有数据库连接字符串：{0}个", lstWMSConnStrings.Count));
							foreach (WMSConnectionString databaseModel in lstWMSConnStrings)
							{
								stringBuilder.AppendLine(databaseModel.ToString());
							}
							logger.Info(stringBuilder.ToString());
						}
					}
					getConnectionSuccess = true;
				}
				catch (Exception ex)
				{
					logger.Error(ex.Message, ex);
					Thread.Sleep(500);
					getConnectionCounter++;
				}
			}
			foreach (WMSConnectionString wmsConnString in lstWMSConnStrings)
			{
				if (!connStringCache.ContainsKey(wmsConnString.Name))
				{
					connStringCache.Add(wmsConnString.Name, wmsConnString.ConnectionString);
				}
				if (!providerNameCache.ContainsKey(wmsConnString.Name))
				{
					providerNameCache.Add(wmsConnString.Name, wmsConnString.ProviderName);
				}
				if (!string.IsNullOrEmpty(wmsConnString.WarehouseId))
				{
                    if (wmsConnString.DatabaseType == DatabaseType.Execute && !warehouseIdCache.ContainsKey(wmsConnString.WarehouseId))
                    {
                        warehouseIdCache.Add(wmsConnString.WarehouseId, wmsConnString.Name);
                    }
                    if (wmsConnString.DatabaseType == DatabaseType.Readonly && !warehouseIdCacheReadonly.ContainsKey(wmsConnString.WarehouseId))
                    {
                        warehouseIdCacheReadonly.Add(wmsConnString.WarehouseId, wmsConnString.Name);
                    }
				}
			}
		}

		/// <summary>
		/// 如果没有初始化，则初始化之
		/// </summary>
		private static void CheckedInited()
		{
			if (lstWMSConnStrings == null)
			{
				logger.Info("WMS ConnectionStringUtil not be initional, init it.");
				InitConnList();
			}
		}

		#endregion

		/// <summary>
		/// 获取所有WMS连接字符串
		/// </summary>
		/// <returns></returns>
		public static IList<WMSConnectionString> GetAllList()
		{
			CheckedInited();
			return lstWMSConnStrings;
		}

		public static string GetDbProviderName(string connName)
		{
			AssertUtils.StringNotNullOrEmpty(connName, "connName");

			CheckedInited();

			if (providerNameCache.ContainsKey(connName))
			{
				return providerNameCache[connName];
			}

			foreach (IConnectionString connString in lstWMSConnStrings)
			{
				if (connString.Name == connName)
				{
					return connString.ProviderName;
				}
			}
			return null;
		}

		public static string GetDbProviderName(string enumValue, DatabaseType databaseType)
		{
			AssertUtils.ArgumentNotNull(databaseType, "databaseType");
			AssertUtils.StringNotNullOrEmpty(enumValue, "enumValue");

			CheckedInited();
			return GetDbProviderName(enumValue + databaseType);
		}

		#region GetConnectionString.

		/// <summary>
		/// 根据字符串名称获取连接字符串
		/// </summary>
		/// <param name="connName">A ConnStringName.</param>
		/// <returns></returns>
		public static string GetConnectionString(string connName)
		{
			AssertUtils.StringNotNullOrEmpty(connName, "connName");

			CheckedInited();

			if (connStringCache.ContainsKey(connName))
			{
				logger.Debug(string.Format("从connStringCache中获取字符串，字符串名称：{0}", connName));
				return connStringCache[connName];
			}

			foreach (IConnectionString connString in lstWMSConnStrings)
			{
				if (connString.Name == connName)
				{
					return connString.ConnectionString;
				}
			}
			return null;
		}

		/// <summary>
		/// 根据枚举值和数据库类型获取连接字符串
		/// </summary>
		/// <param name="enumValue">WMSConnString EnumValue.</param>
		/// <param name="databaseType">DbType. Excuted or Readonly.</param>
		/// <returns></returns>
		public static string GetConnectionString(string enumValue, DatabaseType databaseType)
		{
			AssertUtils.ArgumentNotNull(databaseType, "databaseType");
			AssertUtils.StringNotNullOrEmpty(enumValue, "enumValue");

			CheckedInited();
			return GetConnectionString(enumValue + databaseType);
		}

		/// <summary>
		/// 根据仓库ID获取WMS连接字符串
		/// </summary>
		/// <param name="warehouseId"></param>
		/// <param name="databaseType"></param>
		/// <returns></returns>
		public static string GetConnectionStringByWarehouse(string warehouseId, DatabaseType databaseType)
		{
			AssertUtils.ArgumentNotNull(databaseType, "databaseType");
			AssertUtils.StringNotNullOrEmpty(warehouseId, "warehouseId");

			CheckedInited();
            if(databaseType == DatabaseType.Execute)
            {
                if (warehouseIdCache.ContainsKey(warehouseId))
                {
                    return GetConnectionString(warehouseIdCache[warehouseId]);
                }
            }
            if (databaseType == DatabaseType.Readonly)
            {
                if (warehouseIdCacheReadonly.ContainsKey(warehouseId))
                {
                    return GetConnectionString(warehouseIdCacheReadonly[warehouseId]);
                }
            }
			return null;
		}

		#endregion
	}
}