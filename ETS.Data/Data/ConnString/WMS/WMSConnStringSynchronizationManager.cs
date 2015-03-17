using Common.Logging;
using ETS.Threading;
using ETS.Util;


namespace ETS.Data.ConnString.WMS
{
	/// <summary>
	/// 每个线程中的字符串管理器
	/// </summary>
	/// <remarks>
	/// 每个线程中都会保存一个WarehouseId，此类主要用来获取或设置每个线程的WarehouseId
	/// </remarks>
	public sealed class WMSConnStringSynchronizationManager
	{
		#region Logging

		private static readonly ILog log = DbLogManager.GetConnectionStringLog();

		#endregion

		#region Fields

		private const string CURRENT_WAREHOUSE_ID = "WMS.CurrentThread.CurrentWarehouseId";

		#endregion

		/// <summary>
		/// 获取线程的WarehouseId
		/// </summary>
		/// <returns>get the warehouseId bounded to current thread.</returns>
		public static string GetCurrentThreadWarehouseId()
		{
			object warehouseId = LogicalThreadContext.GetData(CURRENT_WAREHOUSE_ID);
			if (warehouseId == null)
			{
				return string.Empty;
			}
			return warehouseId.ToString();
		}

		/// <summary>
		/// 设置当前线程运行环境的WarehouseId
		/// </summary>
		/// <param name="warehouseId">a WarehouseId</param>
		public static void SetCurrentThreadWarehouseId(string warehouseId)
		{
			AssertUtils.StringNotNullOrEmpty(warehouseId, "warehouseId");
			LogicalThreadContext.SetData(CURRENT_WAREHOUSE_ID, warehouseId);
		}

		/// <summary>
		/// 从当前线程中移除WarehouseId设置.
		/// </summary>
		public static void ClearCurrentThreadWarehouseId()
		{
			LogicalThreadContext.FreeNamedDataSlot(CURRENT_WAREHOUSE_ID);
		}
	}
}