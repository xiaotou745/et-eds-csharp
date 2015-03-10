using System.Data;
using ETS.Dao;

namespace ETS.Transaction.Common
{
	/// <summary>
	/// 默认定义
	/// </summary>
	public class DefaultUnitOfWorkDefinition : IUnitOfWorkDefinition
	{
		/// <summary>
		/// The default transaction timeout.
		/// </summary>
		public const int TIMEOUT_DEFAULT = -1;

		/// <summary>
		/// 默认事务超时时间为60秒
		/// </summary>
		protected int _timeout = 60;

		/// <summary>
		/// 默认事务配置
		/// <table>
		/// <th><td>Property</td><td>Value</td></th>
		/// <tr><td>IsolationLevel</td><see cref="System.Data.IsolationLevel.ReadCommitted"/><td></td></tr>
		/// <tr><td>Timeout</td>60<td></td></tr>
		/// <tr><td>ReadOnly</td><see langword="false"></see><td></td></tr>
		/// <tr><td>Name</td><see langword="null"></see><td></td></tr>
		/// <tr><td>Exclude</td><see langword="false"></see><td></td></tr>
		/// </table>
		/// </summary>
		public DefaultUnitOfWorkDefinition()
		{
			IsolationLevel = IsolationLevel.ReadCommitted;
			Timeout = 60;
			ReadOnly = false;
			Name = null;
			Exclude = false;
		}

		#region IUnitOfWorkDefinition Members

		/// <summary>
		/// Return the isolation level of type <see cref="System.Data.IsolationLevel"/>.
		/// </summary>
		/// <remarks>
		/// <p>
		/// Note that a transaction manager that does not support custom isolation levels
		/// will throw an exception when given any other level than
		/// <see cref="System.Data.IsolationLevel.Unspecified"/>.
		/// </p>
		/// </remarks>
		public IsolationLevel IsolationLevel { get; set; }

		/// <summary>
		/// Return the transaction timeout.
		/// </summary>
		/// <remarks>
		/// <p>
		/// Must return a number of seconds, or -1.
		/// Only makes sense in combination with
		/// Note that a transaction manager that does not support timeouts will
		/// throw an exception when given any other timeout than -1.
		/// </p>
		/// </remarks>
		public int Timeout
		{
			get { return _timeout; }
			set
			{
				if (value < TIMEOUT_DEFAULT)
				{
					throw new InvalidUnitOfWorkDefinitionException(
						"Timeout must be a positive integer or DefaultUnitOfWorkDefinition.TIMEOUT_DEFAULT");
				}
				_timeout = value;
			}
		}

		/// <summary>
		/// Get whether to optimize as read-only transaction.
		/// </summary>
		/// <remarks>
		/// <p>
		/// This just serves as hint for the actual transaction subsystem,
		/// it will <i>not necessarily</i> cause failure of write accesses.
		/// </p>
		/// <p>
		/// A transaction manager that cannot interpret the read-only hint
		/// will <i>not</i> throw an exception when given <c>ReadOnly=true</c>.
		/// </p>
		/// </remarks>
		public bool ReadOnly { get; set; }

		/// <summary>
		/// Return the name of this transaction.  Can be null.
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// This will be used as a transaction name to be shown in a
		/// transaction monitor, if applicable.  In the case of Spring
		/// declarative transactions, the exposed name will be the fully
		/// qualified type name + "." method name + assembly (by default).
		/// </remarks>
		public string Name { get; set; }

		/// <summary>
		/// 该事务及内嵌声明式事务是否被排除，此属性对于直接Using不起作用
		/// </summary>
		public bool Exclude { get; set; }

		#endregion
	}
}