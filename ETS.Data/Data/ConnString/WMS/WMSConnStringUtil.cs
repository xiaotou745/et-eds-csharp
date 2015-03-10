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
		/// ���Դ���
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
		/// ��ʼ��WMS�����ַ���
		/// </summary>
		public static void InitConnList()
		{
			//��ȡ�����ַ����Ƿ�ɹ�
			bool getConnectionSuccess = false;
			//��ȡ�����ַ�������
			int getConnectionCounter = 0;

			while (!getConnectionSuccess && getConnectionCounter <= RETRY_COUNT)
			{
				try
				{
					logger.InfoFormat("��{0}�λ�ȡWMS�����ַ�����ʼ...", getConnectionCounter + 1);
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
							throw new Exception("û��Ҫʹ�õ�WMS�ַ���.WMS���ݿ����ӱ��ѯ�쳣");
						}

						lstWMSConnStrings = lstWmsConnStrings;
						//��־
						if (logger.IsInfoEnabled)
						{
							var stringBuilder = new StringBuilder();
							stringBuilder.Append(string.Format("WMSConnectionString��ʼ����ɣ��������ݿ������ַ�����{0}��", lstWMSConnStrings.Count));
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
		/// ���û�г�ʼ�������ʼ��֮
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
		/// ��ȡ����WMS�����ַ���
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
		/// �����ַ������ƻ�ȡ�����ַ���
		/// </summary>
		/// <param name="connName">A ConnStringName.</param>
		/// <returns></returns>
		public static string GetConnectionString(string connName)
		{
			AssertUtils.StringNotNullOrEmpty(connName, "connName");

			CheckedInited();

			if (connStringCache.ContainsKey(connName))
			{
				logger.Debug(string.Format("��connStringCache�л�ȡ�ַ������ַ������ƣ�{0}", connName));
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
		/// ����ö��ֵ�����ݿ����ͻ�ȡ�����ַ���
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
		/// ���ݲֿ�ID��ȡWMS�����ַ���
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