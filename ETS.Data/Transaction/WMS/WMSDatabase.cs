namespace ETS.Transaction.WMS
{
	/// <summary>
	/// WMS 数据库类型
	/// </summary>
	public enum WMSDatabase
	{
		/// <summary>
		/// 使用Vancl的数据库
		/// </summary>
		Vancl,
		/// <summary>
		/// 凡库数据库
		/// </summary>
		FanKu,
		/// <summary>
		/// 客户数据库
		/// </summary>
		Customer,
		/// <summary>
		/// WMS管理数据库
		/// </summary>
		WMSMaster,
		/// <summary>
		/// 报表数据库
		/// </summary>
		WMSReport,
		/// <summary>
		/// 质检数据库
		/// </summary>
		QMS,
		/// <summary>
		/// 使用当前登陆仓库的数据库
		/// </summary>
		WMS,
		/// <summary>
		/// 指定仓库Id
		/// </summary>
		WarehouseIdSpecified,
	}
}