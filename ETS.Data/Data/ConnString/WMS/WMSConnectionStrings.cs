using System.Timers;
using System.Web;
using Common.Logging;
using ETS.Util;

namespace ETS.Data.ConnString.WMS
{
	/// <summary>
	/// 字符串管理类
	/// </summary>
	public class WMSConnectionStrings
	{
		#region Properties and Fields

		/// <summary>
		/// 字符串日志对象
		/// </summary>
		private static readonly ILog Logger = LogManager.GetLogger("ConnectionStringLog");

		/// <summary>
		/// 判断是网站还是后台 true:网站;false:后台
		/// </summary>
		public static bool IsWmsWeb
		{
			get { return ConfigUtils.GetConfigValue("isWmsWeb", "") == "1"; }
		}

		#endregion

		#region Constants (s)

		private const string DATABASE_SOURCE_VANCL = "Vancl";
		private const string DATABASE_SOURCE_FAN_KU = "FanKu";
		private const string DATABASE_SOURCE_CUSTOMER = "Customer";
		private const string DATABASE_SOURCE_WMS_MASTER = "WMSMaster";
		private const string DATABASE_SOURCE_WMS_REPORT = "WMSReport";
		private const string DATABASE_SOURCE_QMS = "QMS";
		private const string DATABASE_SOURCE_ORDER = "Order";
		private const string DATABASE_SOURCE_EDI = "EDI";
		private const string DATABASE_SOURCE_ABORAD = "Aborad";
		private const string DATABASE_SOURCE_LMS_VANCL = "LMS_VANCL";
		private const string DATABASE_SOURCE_LMS_RFD = "LMS_RFD";

		#endregion

		#region Constructor.

		/// <summary>
		/// Constructor
		/// </summary>
		static WMSConnectionStrings()
		{
			//初始化连接管理
			InitConnList(null, null);

			//设置自动刷新属性
			if (ConfigUtils.GetConfigValue("IsRefreshConn", "") == "true")
			{
				//定义定时器的间隔
				int refrashInterval = int.Parse(ConfigUtils.GetConfigValue("RefrashInterval", "1"));
				if (refrashInterval < 7200000) refrashInterval = 7200000;

				//创建定时器触发连接自动更新
				var timer = new Timer {Interval = refrashInterval, Enabled = true};
				timer.Elapsed += InitConnList;
			}
		}

		private static void InitConnList(object sender, ElapsedEventArgs e)
		{
			WMSConnStringUtil.InitConnList();
		}

		#endregion

		#region Public Methods.

		public static string GetConnectionString(string databaseSource)
		{
			return GetConnectionString(databaseSource, DatabaseType.Execute);
		}

		public static string GetConnectionStringOfReadonly(string databaseSource)
		{
			return GetConnectionString(databaseSource, DatabaseType.Readonly);
		}

		public static string GetConnectionString(string databaseSource, DatabaseType databaseType)
		{
			return WMSConnStringUtil.GetConnectionString(databaseSource, databaseType);
		}

		/// <summary>
		/// 根据仓库Id获取仓库数据库连接字符串
		/// </summary>
		/// <param name="warehouseId">指定的仓库Id</param>
		/// <param name="databaseType">数据库类型</param>
		/// <returns>返回指定仓库Id的WMS数据库连接字符串</returns>
		public static string GetConnectionStringByWarehouseId(string warehouseId, DatabaseType databaseType)
		{
			return WMSConnStringUtil.GetConnectionStringByWarehouse(warehouseId, databaseType);
		}

		public static string GetConnectionStringByWarehouseId(string warehouseId)
		{
			return GetConnectionStringByWarehouseId(warehouseId, DatabaseType.Execute);
		}

		public static string GetConnectionStringOfReadonlyByWarehouseId(string warehouseId)
		{
			return GetConnectionStringByWarehouseId(warehouseId, DatabaseType.Readonly);
		}

		#endregion

		#region ConnectionStrings

		///<summary>
		///SCM 主库连接字符串
		///</summary>
		public static string ConnStringOfSCM
		{
			get { return WMSConnStringUtil.GetConnectionString(DATABASE_SOURCE_VANCL, DatabaseType.Execute); }
		}

		/// <summary>
		/// WMS 主库连接字符串
		/// </summary>
		public static string ConnStringOfWMS
		{
			get
			{
				//如果是手持或网站
				if (IsWmsWeb)
				{
					HttpCookie httpCookie = HttpContext.Current.Request.Cookies["WarehouseId"];
					if (httpCookie == null)
						return null;
					return WMSConnStringUtil.GetConnectionStringByWarehouse(httpCookie.Value, DatabaseType.Execute);
				}
				//如果设置的是当前登陆仓库Id，根据ID获取连接字符串
				string currentWarehouseId = WMSConnStringSynchronizationManager.GetCurrentThreadWarehouseId();
				return WMSConnStringUtil.GetConnectionStringByWarehouse(currentWarehouseId, DatabaseType.Execute);
			}
		}

		/// <summary>
		/// FanKu(凡库)数据库连接字符串
		/// </summary>
		public static string ConnStringOfFanKu
		{
			get { return GetConnectionString(DATABASE_SOURCE_FAN_KU, DatabaseType.Execute); }
		}

		/// <summary>
		/// 客户数据库连接字符串
		/// </summary>
		public static string ConnStringOfCustomer
		{
			get { return GetConnectionString(DATABASE_SOURCE_CUSTOMER, DatabaseType.Execute); }
		}

		/// <summary>
		/// WMS管理数据库连接字符串
		/// </summary>
		public static string ConnStringOfWMSMaster
		{
			get { return GetConnectionString(DATABASE_SOURCE_WMS_MASTER, DatabaseType.Execute); }
		}

		/// <summary>
		/// 报表数据库连接字符串
		/// </summary>
		public static string ConnStringOfWMSReport
		{
			get { return GetConnectionString(DATABASE_SOURCE_WMS_REPORT, DatabaseType.Execute); }
		}

		public static string ConnStringOfQMS
		{
			get { return GetConnectionString(DATABASE_SOURCE_QMS, DatabaseType.Execute); }
		}

		///<summary>
		///订单数据库连接字符串
		///</summary>
		public static string ConnStringOfOrder
		{
			get { return GetConnectionString(DATABASE_SOURCE_ORDER, DatabaseType.Execute); }
		}

		/// <summary>
		/// EDI数据库连接字符串
		/// </summary>
		public static string ConnStringOfEDI
		{
			get { return GetConnectionString(DATABASE_SOURCE_EDI, DatabaseType.Execute); }
		}

		/// <summary>
		/// 海外数据库连接串
		/// </summary>
		public static string ConnStringOfAborad
		{
			get { return GetConnectionString(DATABASE_SOURCE_ABORAD, DatabaseType.Execute); }
		}

		#endregion

		#region ReadonlyConnectionStrings

		/// <summary>
		/// 当前登陆SCM 只读库连接字符串
		/// </summary>
		public static string ReadOnlyConnStringOfSCM
		{
			get { return WMSConnStringUtil.GetConnectionString(DATABASE_SOURCE_VANCL, DatabaseType.Readonly); }
		}

		/// <summary>
		/// 报表数据库只读链接字符串
		/// </summary>
		public static string ReadOnlyConnStringOfReport
		{
			get { return GetConnectionString(DATABASE_SOURCE_WMS_REPORT, DatabaseType.Readonly); }
		}

		/// <summary>
		/// 当前登陆WMS 只读库连接字符串
		/// </summary>
		public static string ReadOnlyConnStringOfWMS
		{
			get
			{
				//如果是手持或网站
				if (IsWmsWeb)
				{
					HttpCookie httpCookie = HttpContext.Current.Request.Cookies["WarehouseId"];
					if (httpCookie == null)
						return null;
					return WMSConnStringUtil.GetConnectionStringByWarehouse(httpCookie.Value, DatabaseType.Execute);
				}
				//如果设置的是当前登陆仓库Id，根据ID获取连接字符串
				string currentWarehouseId = WMSConnStringSynchronizationManager.GetCurrentThreadWarehouseId();
				return WMSConnStringUtil.GetConnectionStringByWarehouse(currentWarehouseId, DatabaseType.Execute);
			}
		}

		/// <summary>
		/// Customer数据库只读链接字符串
		/// </summary>
		public static string ReadOnlyConnStringOfCustomer
		{
			get { return GetConnectionString(DATABASE_SOURCE_CUSTOMER, DatabaseType.Readonly); }
		}

		/// <summary>
		/// Order 数据库只读链接字符串
		/// </summary>
		public static string ReadOnlyConnStringOfOrder
		{
			get { return GetConnectionString(DATABASE_SOURCE_ORDER, DatabaseType.Readonly); }
		}

		/// <summary>
		/// FanKu 数据库只读链接字符串
		/// </summary>
		public static string ReadOnlyConnStringOfFanKu
		{
			get { return GetConnectionString(DATABASE_SOURCE_FAN_KU, DatabaseType.Readonly); }
		}

		/// <summary>
		/// EDI 数据库只读链接字符串
		/// </summary>
		public static string ReadOnlyConnStringOfEDI
		{
			get { return GetConnectionString(DATABASE_SOURCE_EDI, DatabaseType.Readonly); }
		}

		/// <summary>
		/// LMS_VANCL 数据库只读链接字符串
		/// </summary>
		public static string ReadOnlyConnStringOfLMS_VANCL
		{
			get { return GetConnectionString(DATABASE_SOURCE_LMS_VANCL, DatabaseType.Readonly); }
		}

		/// <summary>
		/// LMS_RFD 数据库只读链接字符串
		/// </summary>
		public static string ReadOnlyConnStringOfLMS_RFD
		{
			get { return GetConnectionString(DATABASE_SOURCE_LMS_RFD, DatabaseType.Readonly); }
		}

		#endregion
	}
}