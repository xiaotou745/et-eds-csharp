using ETS.Data.ConnString;
using ETS.Data.ConnString.WMS;

namespace ETS.Data
{
	///<summary>
	/// DAO基类
	///</summary>
	public abstract class DaoBaseOfWMS : AbstractDaoBase
	{
		#region ConnectionStrings

		///<summary>
		/// SCM 主库连接字符串
		///</summary>
		protected string ConnStringOfSCM
		{
			get { return WMSConnectionStrings.ConnStringOfSCM; }
		}

		/// <summary>
		/// WMS 主库连接字符串
		/// </summary>
		protected string ConnStringOfWMS
		{
			get { return WMSConnectionStrings.ConnStringOfWMS; }
		}

		///<summary>
		///FanKu(凡库)数据库连接字符串
		///</summary>
		protected string ConnStringOfFanKu
		{
			get { return WMSConnectionStrings.ConnStringOfFanKu; }
		}

		/// <summary>
		/// 客户数据库连接字符串
		/// </summary>
		protected string ConnStringOfCustomer
		{
			get { return WMSConnectionStrings.ConnStringOfCustomer; }
		}

		/// <summary>
		/// WMS管理数据库连接字符串
		/// </summary>
		protected string ConnStringOfWMSMaster
		{
			get { return WMSConnectionStrings.ConnStringOfWMSMaster; }
		}

		/// <summary>
		/// 报表数据库连接字符串
		/// </summary>
		protected string ConnStringOfWMSReport
		{
			get { return WMSConnectionStrings.ConnStringOfWMSReport; }
		}

		protected string ConnStringOfQMS
		{
			get { return WMSConnectionStrings.ConnStringOfQMS; }
		}

		/// <summary>
		/// 订单数据库连接字符串
		/// </summary>
		protected string ConnStringOfOrder
		{
			get { return WMSConnectionStrings.ConnStringOfOrder; }
		}

		#endregion

		#region ReadonlyConnectionStrings

		/// <summary>
		/// SCM 只读库连接字符串
		/// </summary>
		protected string ReadonlyConnStringOfSCM
		{
			get { return WMSConnectionStrings.ReadOnlyConnStringOfSCM; }
		}

		/// <summary>
		/// WMS 只读库连接字符串
		/// </summary>
		protected string ReadOnlyConnStringOfWMS
		{
			get { return WMSConnectionStrings.ReadOnlyConnStringOfWMS; }
		}

		/// <summary>
		/// WMS 只读库连接字符串
		/// </summary>
		protected string ReadOnlyConnStringOfWMSReport
		{
			get { return WMSConnectionStrings.ReadOnlyConnStringOfReport; }
		}

		/// <summary>
		/// 凡库 只读库连接字符串
		/// </summary>
		protected string ReadOnlyConnStringOfFanKu
		{
			get { return WMSConnectionStrings.ReadOnlyConnStringOfFanKu; }
		}

		/// <summary>
		/// 客户 只读数据库连接字符串
		/// </summary>
		protected string ReadOnlyConnStringOfCustomer
		{
			get { return WMSConnectionStrings.ReadOnlyConnStringOfCustomer; }
		}

		/// <summary>
		/// 订单 只读数据库连接字符串
		/// </summary>
		protected string ReadOnlyConnStringOfOrder
		{
			get { return WMSConnectionStrings.ReadOnlyConnStringOfOrder; }
		}

		#endregion

        protected string GetConnStringByWarehouseId(string warehouseId, DatabaseType databaseType)
        {
            return WMSConnectionStrings.GetConnectionStringByWarehouseId(warehouseId, databaseType);
        }

        protected string GetReadonlyConnStringByWarehouseId(string warehouseId)
        {
            return GetConnStringByWarehouseId(warehouseId, DatabaseType.Readonly);
        }
	}
}