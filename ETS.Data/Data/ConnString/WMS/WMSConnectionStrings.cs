using System.Timers;
using System.Web;
using Common.Logging;
using ETS.Util;

namespace ETS.Data.ConnString.WMS
{
	/// <summary>
	/// �ַ���������
	/// </summary>
	public class WMSConnectionStrings
	{
		#region Properties and Fields

		/// <summary>
		/// �ַ�����־����
		/// </summary>
		private static readonly ILog Logger = LogManager.GetLogger("ConnectionStringLog");

		/// <summary>
		/// �ж�����վ���Ǻ�̨ true:��վ;false:��̨
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
			//��ʼ�����ӹ���
			InitConnList(null, null);

			//�����Զ�ˢ������
			if (ConfigUtils.GetConfigValue("IsRefreshConn", "") == "true")
			{
				//���嶨ʱ���ļ��
				int refrashInterval = int.Parse(ConfigUtils.GetConfigValue("RefrashInterval", "1"));
				if (refrashInterval < 7200000) refrashInterval = 7200000;

				//������ʱ�����������Զ�����
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
		/// ���ݲֿ�Id��ȡ�ֿ����ݿ������ַ���
		/// </summary>
		/// <param name="warehouseId">ָ���Ĳֿ�Id</param>
		/// <param name="databaseType">���ݿ�����</param>
		/// <returns>����ָ���ֿ�Id��WMS���ݿ������ַ���</returns>
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
		///SCM ���������ַ���
		///</summary>
		public static string ConnStringOfSCM
		{
			get { return WMSConnStringUtil.GetConnectionString(DATABASE_SOURCE_VANCL, DatabaseType.Execute); }
		}

		/// <summary>
		/// WMS ���������ַ���
		/// </summary>
		public static string ConnStringOfWMS
		{
			get
			{
				//������ֳֻ���վ
				if (IsWmsWeb)
				{
					HttpCookie httpCookie = HttpContext.Current.Request.Cookies["WarehouseId"];
					if (httpCookie == null)
						return null;
					return WMSConnStringUtil.GetConnectionStringByWarehouse(httpCookie.Value, DatabaseType.Execute);
				}
				//������õ��ǵ�ǰ��½�ֿ�Id������ID��ȡ�����ַ���
				string currentWarehouseId = WMSConnStringSynchronizationManager.GetCurrentThreadWarehouseId();
				return WMSConnStringUtil.GetConnectionStringByWarehouse(currentWarehouseId, DatabaseType.Execute);
			}
		}

		/// <summary>
		/// FanKu(����)���ݿ������ַ���
		/// </summary>
		public static string ConnStringOfFanKu
		{
			get { return GetConnectionString(DATABASE_SOURCE_FAN_KU, DatabaseType.Execute); }
		}

		/// <summary>
		/// �ͻ����ݿ������ַ���
		/// </summary>
		public static string ConnStringOfCustomer
		{
			get { return GetConnectionString(DATABASE_SOURCE_CUSTOMER, DatabaseType.Execute); }
		}

		/// <summary>
		/// WMS�������ݿ������ַ���
		/// </summary>
		public static string ConnStringOfWMSMaster
		{
			get { return GetConnectionString(DATABASE_SOURCE_WMS_MASTER, DatabaseType.Execute); }
		}

		/// <summary>
		/// �������ݿ������ַ���
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
		///�������ݿ������ַ���
		///</summary>
		public static string ConnStringOfOrder
		{
			get { return GetConnectionString(DATABASE_SOURCE_ORDER, DatabaseType.Execute); }
		}

		/// <summary>
		/// EDI���ݿ������ַ���
		/// </summary>
		public static string ConnStringOfEDI
		{
			get { return GetConnectionString(DATABASE_SOURCE_EDI, DatabaseType.Execute); }
		}

		/// <summary>
		/// �������ݿ����Ӵ�
		/// </summary>
		public static string ConnStringOfAborad
		{
			get { return GetConnectionString(DATABASE_SOURCE_ABORAD, DatabaseType.Execute); }
		}

		#endregion

		#region ReadonlyConnectionStrings

		/// <summary>
		/// ��ǰ��½SCM ֻ���������ַ���
		/// </summary>
		public static string ReadOnlyConnStringOfSCM
		{
			get { return WMSConnStringUtil.GetConnectionString(DATABASE_SOURCE_VANCL, DatabaseType.Readonly); }
		}

		/// <summary>
		/// �������ݿ�ֻ�������ַ���
		/// </summary>
		public static string ReadOnlyConnStringOfReport
		{
			get { return GetConnectionString(DATABASE_SOURCE_WMS_REPORT, DatabaseType.Readonly); }
		}

		/// <summary>
		/// ��ǰ��½WMS ֻ���������ַ���
		/// </summary>
		public static string ReadOnlyConnStringOfWMS
		{
			get
			{
				//������ֳֻ���վ
				if (IsWmsWeb)
				{
					HttpCookie httpCookie = HttpContext.Current.Request.Cookies["WarehouseId"];
					if (httpCookie == null)
						return null;
					return WMSConnStringUtil.GetConnectionStringByWarehouse(httpCookie.Value, DatabaseType.Execute);
				}
				//������õ��ǵ�ǰ��½�ֿ�Id������ID��ȡ�����ַ���
				string currentWarehouseId = WMSConnStringSynchronizationManager.GetCurrentThreadWarehouseId();
				return WMSConnStringUtil.GetConnectionStringByWarehouse(currentWarehouseId, DatabaseType.Execute);
			}
		}

		/// <summary>
		/// Customer���ݿ�ֻ�������ַ���
		/// </summary>
		public static string ReadOnlyConnStringOfCustomer
		{
			get { return GetConnectionString(DATABASE_SOURCE_CUSTOMER, DatabaseType.Readonly); }
		}

		/// <summary>
		/// Order ���ݿ�ֻ�������ַ���
		/// </summary>
		public static string ReadOnlyConnStringOfOrder
		{
			get { return GetConnectionString(DATABASE_SOURCE_ORDER, DatabaseType.Readonly); }
		}

		/// <summary>
		/// FanKu ���ݿ�ֻ�������ַ���
		/// </summary>
		public static string ReadOnlyConnStringOfFanKu
		{
			get { return GetConnectionString(DATABASE_SOURCE_FAN_KU, DatabaseType.Readonly); }
		}

		/// <summary>
		/// EDI ���ݿ�ֻ�������ַ���
		/// </summary>
		public static string ReadOnlyConnStringOfEDI
		{
			get { return GetConnectionString(DATABASE_SOURCE_EDI, DatabaseType.Readonly); }
		}

		/// <summary>
		/// LMS_VANCL ���ݿ�ֻ�������ַ���
		/// </summary>
		public static string ReadOnlyConnStringOfLMS_VANCL
		{
			get { return GetConnectionString(DATABASE_SOURCE_LMS_VANCL, DatabaseType.Readonly); }
		}

		/// <summary>
		/// LMS_RFD ���ݿ�ֻ�������ַ���
		/// </summary>
		public static string ReadOnlyConnStringOfLMS_RFD
		{
			get { return GetConnectionString(DATABASE_SOURCE_LMS_RFD, DatabaseType.Readonly); }
		}

		#endregion
	}
}