using System.Data;

namespace ETS.Transaction.Common
{
	/// <summary>
	/// Much based on TransactionDefinition of Spring.NET. But add <see cref="Exclude"/> support.
	/// </summary>
	public interface IUnitOfWorkDefinition
	{
		/// <summary>
		/// 事务隔离级别
		/// </summary>
		IsolationLevel IsolationLevel { get; set; }

		/// <summary>
		/// 事务超时时间
		/// </summary>
		int Timeout { get; set; }

		/// <summary>
		/// 告知Nhibernate此session为ReadOnly
		/// </summary>
		bool ReadOnly { get; set; }

		/// <summary>
		/// 事务名称
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// 该事务及内嵌声明式事务是否被排除，此属性对于直接Using不起作用
		/// </summary>
		bool Exclude { get; set; }
	}
}