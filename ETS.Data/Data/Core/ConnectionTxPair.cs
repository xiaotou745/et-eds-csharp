using System.Data;

namespace ETS.Data.Core
{
	/// <summary>
	/// A simple holder for the current Connection/Transaction objects
	/// to use within a given AdoTemplate Execute operation.  Used internally.
	/// </summary>
	public class ConnectionTxPair
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ConnectionTxPair"/> class.
		/// </summary>
		/// <param name="connection">The connection.</param>
		/// <param name="transaction">The transaction.</param>
		public ConnectionTxPair(IDbConnection connection, IDbTransaction transaction)
		{
			Connection = connection;
			Transaction = transaction;
		}

		/// <summary>
		/// Gets the connection.
		/// </summary>
		/// <value>The connection.</value>
		public IDbConnection Connection { get; set; }

		/// <summary>
		/// Gets the transaction.
		/// </summary>
		/// <value>The transaction.</value>
		public IDbTransaction Transaction { get; private set; }

		/// <summary>
		/// 获取当前<see cref="ConnectionTxPair"/>是否拥有<see cref="IDbConnection"/>
		/// </summary>
		/// <remarks>
		/// return true if the Connection is not null, else false
		/// </remarks>
		public bool HasConnection
		{
			get { return Connection != null; }
		}
	}
}