using System.Data;
using ETS.Transaction.Common;

namespace ETS.Transaction.WMS
{
	/// <summary>
	/// WMS 事务定义实现
	/// </summary>
	public class WMSUnitOfWorkDefinition
	{
		public static IWMSUnitOfWorkDefinition DefaultDefintion
		{
			get { return new WMSDefaultUnitOfWorkDefinition(); }
		}

		#region Nested type: WMSDefaultUnitOfWorkDefinition

		private class WMSDefaultUnitOfWorkDefinition : IWMSUnitOfWorkDefinition
		{
			private readonly IUnitOfWorkDefinition unitOfWorkDefinition;

			public WMSDefaultUnitOfWorkDefinition()
			{
				unitOfWorkDefinition = UnitOfWorkDefinition.DefaultDefinition;
				unitOfWorkDefinition.Timeout = 180;
				WarehouseId = string.Empty;
				Database = null;
				unitOfWorkDefinition.Name = "WMS_UnitOfWork";
			}

			#region IWMSUnitOfWorkDefinition Members

			/// <summary>
			/// 事务数据库
			/// </summary>
			public WMSDatabase? Database { get; set; }

			/// <summary>
			/// 仓库Id
			/// </summary>
			public string WarehouseId { get; set; }

			public IsolationLevel IsolationLevel
			{
				get { return unitOfWorkDefinition.IsolationLevel; }
				set { unitOfWorkDefinition.IsolationLevel = value; }
			}

			public int Timeout
			{
				get { return unitOfWorkDefinition.Timeout; }
				set { unitOfWorkDefinition.Timeout = value; }
			}

			public bool ReadOnly
			{
				get { return unitOfWorkDefinition.ReadOnly; }
				set { unitOfWorkDefinition.ReadOnly = value; }
			}

			public string Name
			{
				get { return unitOfWorkDefinition.Name; }
				set { unitOfWorkDefinition.Name = value; }
			}

			public bool Exclude
			{
				get { return unitOfWorkDefinition.Exclude; }
				set { unitOfWorkDefinition.Exclude = value; }
			}

			#endregion
		}

		#endregion
	}
}