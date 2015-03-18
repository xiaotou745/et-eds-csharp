using System;
using ETS.Transaction.Common;
using ETS.Util;


namespace ETS.Transaction.WMS
{
	/// <summary>
	/// WMS 事务获取工具
	/// </summary>
	public class UnitOfWorkUtil
	{
		/// <summary>
		/// 获取SCM库的事务管理对象
		/// </summary>
		/// <returns></returns>
		public static IUnitOfWork GetUnitOfWorkOfEDS()
		{
			return GetUnitOfWork(WMSDatabase.Vancl, string.Empty);
		}

		/// <summary>
		/// 获取当前登陆仓库的事务管理对象
		/// </summary>
		/// <returns>返回当前登陆仓库的事务管理对象</returns>
		public static IUnitOfWork GetUnitOfWorkOfWMS()
		{
			return GetUnitOfWork(WMSDatabase.WMS, string.Empty);
		}

		/// <summary>
		/// 获取Master库的事务管理对象
		/// </summary>
		/// <returns></returns>
		public static IUnitOfWork GetUnitOfWorkOfWMSMaster()
		{
			return GetUnitOfWork(WMSDatabase.WMSMaster, string.Empty);
		}

		/// <summary>
		/// 获取报表库的事务管理对象
		/// </summary>
		/// <returns></returns>
		public static IUnitOfWork GetUnitOfWorkOfWMSReport()
		{
			return GetUnitOfWork(WMSDatabase.WMSReport, string.Empty);
		}

		/// <summary>
		/// 获取报表库的事务管理对象
		/// </summary>
		/// <returns></returns>
		public static IUnitOfWork GetUnitOfWorkOfQMS()
		{
			return GetUnitOfWork(WMSDatabase.QMS, string.Empty);
		}

		/// <summary>
		/// 获取指定仓库的事务管理对象
		/// </summary>
		/// <param name="warehouseId">指定仓库的Id</param>
		/// <returns>返回指定的仓库Id的事务管理对象</returns>
		public static IUnitOfWork GetUnitOfWorkOfWMSWithFixWarehouse(string warehouseId)
		{
			return GetUnitOfWork(WMSDatabase.WarehouseIdSpecified, warehouseId);
		}

		public static IUnitOfWork GetUnitOfWorkOfCustomer()
		{
			return GetUnitOfWork(WMSDatabase.Customer, string.Empty);
		}

		public static IUnitOfWork GetUnitOfWorkOfFanKu()
		{
			return GetUnitOfWork(WMSDatabase.FanKu, string.Empty);
		}

		/// <summary>
		/// 获取事务管理对象
		/// </summary>
		/// <param name="system">事务运行系统</param>
		/// <param name="warehouseId">事务运行仓库Id</param>
		/// <returns></returns>
		private static IUnitOfWork GetUnitOfWork(WMSDatabase system, string warehouseId)
		{
			AssertUtils.ArgumentNotNull(system, "system");
			if (system == WMSDatabase.WarehouseIdSpecified && string.IsNullOrEmpty(warehouseId))
			{
				throw new Exception("请指定运行事务的仓库Id");
			}
			IWMSUnitOfWorkDefinition unitOfWorkDefinition = WMSUnitOfWorkDefinition.DefaultDefintion;
			unitOfWorkDefinition.Database = system;
			unitOfWorkDefinition.WarehouseId = warehouseId;

			string connString = ConnectionStringsUtil.GetConnStringByDefinition(unitOfWorkDefinition);
			if (string.IsNullOrEmpty(connString))
			{
				throw new Exception("根据事务定义配置没有找到相应的连接字符串。");
			}

			return UnitOfWorkFactory.GetAdoNetUnitOfWork(connString, unitOfWorkDefinition);
		}
	}
}