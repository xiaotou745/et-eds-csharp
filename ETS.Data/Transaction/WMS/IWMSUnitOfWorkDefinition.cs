using ETS.Transaction.Common;

namespace ETS.Transaction.WMS
{
	/// <summary>
	/// WMS 事务定义接口
	/// 在通用接口上加上了数据库的指定
	/// </summary>
	public interface IWMSUnitOfWorkDefinition : IUnitOfWorkDefinition
	{
		/// <summary>
		/// 在哪个数据库执行事务
		/// </summary>
		WMSDatabase? Database { get; set; }

		/// <summary>
		/// 仓库Id
		/// </summary>
		string WarehouseId { get; set; }
	}
}