using System;
using ETS.Data.ConnString.WMS;

namespace ETS.Transaction.WMS
{
	/// <summary>
	/// the <see cref="ConnectionStringsUtil"/>'s Utils
	/// </summary>
	public class ConnectionStringsUtil
	{
		public static string GetConnStringByDefinition(IWMSUnitOfWorkDefinition definition)
		{
			string connectionString;
			if(definition.Database == null)
			{
				throw new Exception("没有指定当前事务运行环境.UnitOfWorkDefinition.Database is null");
			}
			switch (definition.Database.Value)
			{
				case WMSDatabase.Vancl:
					connectionString = WMSConnectionStrings.ConnStringOfSCM;
					break;
				case WMSDatabase.FanKu:
					connectionString = WMSConnectionStrings.ConnStringOfFanKu;
					break;
				case WMSDatabase.Customer:
					connectionString = WMSConnectionStrings.ConnStringOfCustomer;
					break;
				case WMSDatabase.WMSMaster:
					connectionString = WMSConnectionStrings.ConnStringOfWMSMaster;
					break;
				case WMSDatabase.WMSReport:
					connectionString = WMSConnectionStrings.ConnStringOfWMSReport;
					break;
				case WMSDatabase.QMS:
					connectionString = WMSConnectionStrings.ConnStringOfQMS;
					break;
				case WMSDatabase.WMS:
					connectionString = WMSConnectionStrings.ConnStringOfWMS;
					break;
				case WMSDatabase.WarehouseIdSpecified:
					if (string.IsNullOrEmpty(definition.WarehouseId))
					{
						throw new Exception("请指定事务的仓库Id");
					}
					connectionString = WMSConnectionStrings.GetConnectionStringByWarehouseId(definition.WarehouseId);
					break;
				default:
					connectionString = WMSConnectionStrings.GetConnectionString(definition.Database.Value.ToString());
					break;
			}
			return connectionString;
		}
	}
}