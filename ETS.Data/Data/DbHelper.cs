using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Common.Logging;
using ETS.Data.Core;
using ETS.Data.Generic;
using ETS.Util;
using ETS.Extension;

namespace ETS.Data
{
	/// <summary>
	/// the <see cref="DbHelper"/> Class
	/// </summary>
	public class DbHelper
	{
	    public static string SqlServerDbProviderName = DbProviderFactoryObj.SQL_CLIENT_PROVIDERINVARIANT_NAME;
	    public static string MySqlDbProviderName = DbProviderFactoryObj.MYSQL_CLIENT_PROVIDERINVARIANT_NAME;
		#region Construtor

		public DbHelper()
		{
			dbProvider = DbProviderFactoryObj.GetProvider();
		}

		public DbHelper(string providerInvariantName)
		{
			dbProvider = DbProviderFactoryObj.GetProvider(providerInvariantName);
		}

		#endregion

		#region Logger

		/// <summary>
		/// ConnectionStringLog.txt Logger
		/// </summary>
		protected readonly ILog logger = DbLogManager.GetConnectionStringLog();

		#endregion

		#region 属性

		/// <summary>
		/// The default transaction timeout.
		/// </summary>
		public const int TIMEOUT_DEFAULT = -1;

		private IDbProvider dbProvider;

		private IDbProvider DbProvider
		{
			get { return dbProvider ?? (dbProvider = DbProviderFactoryObj.GetProvider()); }
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// 获取Ado.Net Connection/Transaction Pair from the given connectionString
		/// </summary>
		/// <param name="connString">连接字符串</param>
		/// <returns>A Connection/Transaction Pair</returns>
		public ConnectionTxPair GetConnectionTxPair(string connString)
		{
			DbProvider.ConnectionString = connString;
			ConnectionTxPair connectionTxPair = ConnectionUtils.GetConnectionTxPair(DbProvider);
			return connectionTxPair;
		}

		/// <summary>
		/// 设置指定的超时时间 - 如果在事务内，则使用事务超时时间 否则使用指定超时时间
		/// </summary>
		/// <param name="dbCommand">The command.</param>
		/// <param name="timeout">The timeout to apply (or 0 for no timeout outside of a transaction)</param>
		private void ApplyTransactionTimeout(IDbCommand dbCommand, int timeout)
		{
			ConnectionUtils.ApplyTransactionTimeout(dbCommand, DbProvider, timeout);
		}

		/// <summary>
		/// 为<see cref="DbCommand"/>附加参数
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="commandParameters">The sql parameters.</param>
		private void AttachParameters(IDbCommand command, ICollection<IDataParameter> commandParameters)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command", "command must not be Null.");
			}
			if (commandParameters == null || commandParameters.Count == 0)
			{
				return;
			}
			foreach (IDataParameter parameter in commandParameters)
			{
				if (parameter != null)
				{
					if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) &&
					    (parameter.Value == null))
					{
						parameter.Value = DBNull.Value;
					}
					command.Parameters.Add(parameter);
				}
			}
		}

		/// <summary>
		/// 设置<see cref="DbCommand"/>.
		/// </summary>
		/// <param name="dbCommand">The command.</param>
		/// <param name="dbConnection">The connection.</param>
		/// <param name="dbTransaction">The transaction</param>
		/// <param name="commandType">The<see cref="CommandType"/>.</param>
		/// <param name="commandText">The commandText</param>
		/// <param name="commandParameters">The SQL Parameters</param>
		private void PrepareCommand(IDbCommand dbCommand, IDbConnection dbConnection, IDbTransaction dbTransaction,
		                            CommandType commandType, string commandText,
		                            IDbParameters commandParameters)
		{
			AssertUtils.StringNotNullOrEmpty(commandText, "commandText", "commandText must not be Null or Empty.");
			AssertUtils.ArgumentNotNull(dbCommand, "dbCommand");
			AssertUtils.ArgumentNotNull(dbConnection, "dbConnection");

			if (dbConnection.State != ConnectionState.Open)
			{
				dbConnection.Open();
			}
			dbCommand.Connection = dbConnection;
			if (dbTransaction != null)
			{
				AssertUtils.ArgumentNotNull(dbTransaction.Connection, "dbTransaction.Connection",
				                           "The transaction was rollbacked or commited, please provide an open transaction.");
				dbCommand.Transaction = dbTransaction;
			}
			dbCommand.CommandType = commandType;
			dbCommand.CommandText = commandText;

			if (commandParameters != null)
			{
				ParameterUtils.CopyParameters(dbCommand, commandParameters);
			}
		}

		#endregion

		#region DbCommand Methods

		/// <summary>
		/// Creates a new instance of <see cref="DbParameters"/>
		/// </summary>
		/// <returns>a new instance of <see cref="DbParameters"/></returns>
		public virtual IDbParameters CreateDbParameters()
		{
			return new DbParameters(DbProvider);
		}

		/// <summary>
		/// Creates the a db parameters collection, adding to the collection a parameter created from
		/// the method parameters.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="dbType">The type of the parameter.</param>
		/// <param name="size">The size of the parameter, for use in defining lengths of string values.  Use
		/// 0 if not applicable.</param>
		/// <param name="parameterValue">The parameter value.</param>
		/// <returns>A collection of db parameters with a single parameter in the collection based
		/// on the method parameters</returns>
		public IDbParameters CreateDbParameters(string name, Enum dbType, int size, object parameterValue)
		{
			IDbParameters parameters = new DbParameters(DbProvider);
			parameters.Add(name, dbType, size).Value = parameterValue;
			return parameters;
		}

		/// <summary>
		/// Creates a new instance of <see cref="IDataParameter"/>
		/// </summary>
		/// <returns> a new instance of <see cref="IDataParameter"/></returns>
		public virtual IDbDataParameter CreateDbParameter()
		{
			var dbParameters = new DbParameters(DbProvider);
			return dbParameters.CreateParameter();
		}

		public virtual IDataParameter CreateDbParameter(string name, Enum dbType, int size, object value)
		{
			var dbParameters = new DbParameters(DbProvider);
			IDbDataParameter dbDataParameter = dbParameters.Add(name, dbType, size);
			dbDataParameter.Value = value;
			return ParameterUtils.CloneParameter(dbDataParameter);
		}

		#endregion

		#region ExecuteDataset

		/// <summary>
		/// 执行SQL, 返回Dataset
		/// </summary>
		/// <param name="connString">the ConnectionString</param>
		/// <param name="commandText">the execute sql</param>
		/// <returns>return a Dataset</returns>
		public DataSet ExecuteDataset(string connString, string commandText)
		{
			return ExecuteDataset(connString, CommandType.Text, commandText, TIMEOUT_DEFAULT, null);
		}

        /// <summary>
        /// 执行SQL, 返回DataTable
        /// </summary>
        /// <param name="connString">the ConnectionString</param>
        /// <param name="commandText">the execute sql</param>
        /// <returns>return a Dataset</returns>
        public DataTable ExecuteDataTable(string connString, string commandText)
        {
            return DataTableHelper.GetTable(ExecuteDataset(connString, CommandType.Text, commandText, TIMEOUT_DEFAULT, null));
        }

		/// <summary>
		/// 执行SQL, 返回Dataset
		/// </summary>
		/// <param name="connString">the ConnectionString</param>
		/// <param name="commandText">the execute sql</param>
		/// <param name="parameterValues">the sql parameters</param>
		/// <returns>return a Dataset</returns>
		public DataSet ExecuteDataset(string connString, string commandText, IDbParameters parameterValues)
		{
			return ExecuteDataset(connString, CommandType.Text, commandText, TIMEOUT_DEFAULT, parameterValues);
		}

		/// <summary>
		/// 执行SQL, 返回Dataset
		/// </summary>
		/// <param name="connString">the ConnectionString</param>
		/// <param name="commandType">the sql CommandType.</param>
		/// <param name="commandText">the execute sql</param>
		/// <returns>return a Dataset</returns>
		public DataSet ExecuteDataset(string connString, CommandType commandType, string commandText)
		{
			return ExecuteDataset(connString, commandType, commandText, TIMEOUT_DEFAULT, null);
		}

		/// <summary>
		/// 执行SQL, 返回Dataset
		/// </summary>
		/// <param name="connString">the ConnectionString</param>
		/// <param name="commandType">the sql CommandType.</param>
		/// <param name="commandText">the execute sql</param>
		/// <param name="parameterValues">the sql parameters</param>
		/// <returns>return a Dataset</returns>
		public DataSet ExecuteDataset(string connString, CommandType commandType, string commandText,
		                              IDbParameters parameterValues)
		{
			return ExecuteDataset(connString, commandType, commandText, TIMEOUT_DEFAULT, parameterValues);
		}

		/// <summary>
		/// 执行SQL, 返回Dataset
		/// </summary>
		/// <param name="connString">the ConnectionString</param>
		/// <param name="commandType">the sql CommandType.</param>
		/// <param name="commandText">the execute sql</param>
		/// <param name="timeoutInSeconds">the dbcommand timeout.</param>
		/// <param name="parameterValues">the sql parameters</param>
		/// <returns>return a Dataset</returns>
		/// <exception cref="ArgumentNullException">如果参数connString为null或空，则抛出此异常</exception>
		public DataSet ExecuteDataset(string connString, CommandType commandType, string commandText,
		                              int timeoutInSeconds, IDbParameters parameterValues)
		{
			AssertUtils.StringNotNullOrEmpty(connString, "connString");

			ConnectionTxPair connectionTxPair = GetConnectionTxPair(connString);
			try
			{
				using (IDbCommand dbCommand = DbProvider.CreateCommand())
				{
					PrepareCommand(dbCommand, connectionTxPair.Connection, connectionTxPair.Transaction, commandType, commandText,
					               parameterValues);

					ApplyTransactionTimeout(dbCommand, timeoutInSeconds);

					IDbDataAdapter dbDataAdapter = DbProvider.CreateDataAdapter();

					dbDataAdapter.SelectCommand = dbCommand;
					var ds = new DataSet();
					dbDataAdapter.Fill(ds);

					ParameterUtils.CopyParameters(parameterValues, dbCommand);

					return ds;
				}
			}
			catch (Exception)
			{
				ConnectionUtils.DisposeConnection(connectionTxPair.Connection, DbProvider);
				connectionTxPair.Connection = null;
				throw;
			}
			finally
			{
				ConnectionUtils.DisposeConnection(connectionTxPair.Connection, DbProvider);
			}
		}
     

		#endregion

		#region ExecuteNoQuery

		/// <summary>
		/// 执行SQL，并返回影响的行数
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="commandText">the execute Sql.</param>
		/// <returns>返回受影响的行数.</returns>
		public int ExecuteNonQuery(string connString, string commandText)
		{
			return ExecuteNonQuery(connString, CommandType.Text, commandText, TIMEOUT_DEFAULT, null);
		}

		/// <summary>
		/// 执行SQL，并返回影响的行数
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="commandText">the execute Sql.</param>
		/// <param name="parameters">the sql parameters.</param>
		/// <returns>返回受影响的行数.</returns>
		public int ExecuteNonQuery(string connString, string commandText, IDbParameters parameters)
		{
			return ExecuteNonQuery(connString, CommandType.Text, commandText, TIMEOUT_DEFAULT, parameters);
		}

		/// <summary>
		/// 执行SQL，并返回影响的行数
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="commandType">the Sql CommandType.</param>
		/// <param name="commandText">the execute Sql.</param>
		/// <returns>返回受影响的行数.</returns>
		public int ExecuteNonQuery(string connString, CommandType commandType, string commandText)
		{
			return ExecuteNonQuery(connString, commandType, commandText, TIMEOUT_DEFAULT, null);
		}

		/// <summary>
		/// 执行SQL，并返回影响的行数
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="commandType">the Sql CommandType.</param>
		/// <param name="commandText">the execute Sql.</param>
		/// <param name="parameters">the sql parameters.</param>
		/// <returns>返回受影响的行数.</returns>
		public int ExecuteNonQuery(string connString, CommandType commandType, string commandText,
		                           IDbParameters parameters)
		{
			return ExecuteNonQuery(connString, commandType, commandText, TIMEOUT_DEFAULT, parameters);
		}

		/// <summary>
		/// 执行SQL，并返回影响的行数
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="commandType">the Sql CommandType.</param>
		/// <param name="commandText">the execute Sql.</param>
		/// <param name="timeOutInSeconds">the dbcommand timeout.</param>
		/// <param name="parameters">the sql parameters.</param>
		/// <returns>返回受影响的行数.</returns>
		public int ExecuteNonQuery(string connString, CommandType commandType, string commandText, int timeOutInSeconds,
		                           IDbParameters parameters)
		{
			AssertUtils.StringNotNullOrEmpty(connString, "connString");

			ConnectionTxPair connectionTxPair = GetConnectionTxPair(connString);
			try
			{
				using (IDbCommand dbCommand = DbProvider.CreateCommand())
				{
					PrepareCommand(dbCommand, connectionTxPair.Connection, connectionTxPair.Transaction, CommandType.Text, commandText,
					               parameters);

					ApplyTransactionTimeout(dbCommand, timeOutInSeconds);

					int rowCount = dbCommand.ExecuteNonQuery();

					ParameterUtils.CopyParameters(parameters, dbCommand);

					return rowCount;
				}
			}
			catch (Exception ex)
			{
				ConnectionUtils.DisposeConnection(connectionTxPair.Connection, DbProvider);
				connectionTxPair.Connection = null;
				throw ex;
			}
			finally
			{
				ConnectionUtils.DisposeConnection(connectionTxPair.Connection, DbProvider);
			}
		}

		#endregion

		#region ExecuteScalar

		/// <summary>
		/// 执行查询，并返回查询所返回的结果集中第一行的第一列，忽略额外的列或行
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="commandText">A execute Sql.</param>
		/// <returns>返回查询所返回的结果集中的第一行的第一列。</returns>
		public object ExecuteScalar(string connString, string commandText)
		{
			return ExecuteScalar(connString, CommandType.Text, commandText, TIMEOUT_DEFAULT, null);
		}

		/// <summary>
		/// 执行查询，并返回查询所返回的结果集中第一行的第一列，忽略额外的列或行
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="commandText">A execute Sql.</param>
		/// <param name="parameterValues">the sql parameters.</param>
		/// <returns>返回查询所返回的结果集中的第一行的第一列。</returns>
		public object ExecuteScalar(string connString, string commandText, IDbParameters parameterValues)
		{
			return ExecuteScalar(connString, CommandType.Text, commandText, TIMEOUT_DEFAULT, parameterValues);
		}

		/// <summary>
		/// 执行查询，并返回查询所返回的结果集中第一行的第一列，忽略额外的列或行
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="commandType">the CommandType.</param>
		/// <param name="commandText">A execute Sql.</param>
		/// <returns>返回查询所返回的结果集中的第一行的第一列。</returns>
		public object ExecuteScalar(string connString, CommandType commandType, string commandText)
		{
			return ExecuteScalar(connString, commandType, commandText, TIMEOUT_DEFAULT, null);
		}

		/// <summary>
		/// 执行查询，并返回查询所返回的结果集中第一行的第一列，忽略额外的列或行
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="commandType">the CommandType.</param>
		/// <param name="commandText">A execute Sql.</param>
		/// <param name="parameterValues">the sql parameters.</param>
		/// <returns>返回查询所返回的结果集中的第一行的第一列。</returns>
		public object ExecuteScalar(string connString, CommandType commandType, string commandText,
		                            IDbParameters parameterValues)
		{
			return ExecuteScalar(connString, commandType, commandText, TIMEOUT_DEFAULT, parameterValues);
		}

		/// <summary>
		/// 执行查询，并返回查询所返回的结果集中第一行的第一列，忽略额外的列或行
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="commandType">the CommandType.</param>
		/// <param name="commandText">A execute Sql.</param>
		/// <param name="timeoutInSeconds">the dbcommand timeout.</param>
		/// <param name="parameterValues">the sql parameters.</param>
		/// <returns>返回查询所返回的结果集中的第一行的第一列。</returns>
		public object ExecuteScalar(string connString, CommandType commandType, string commandText,
		                            int timeoutInSeconds, IDbParameters parameterValues)
		{
			AssertUtils.StringNotNullOrEmpty(connString, "connString");

			ConnectionTxPair connectionTxPair = GetConnectionTxPair(connString);
			try
			{
				using (IDbCommand dbCommand = DbProvider.CreateCommand())
				{
					PrepareCommand(dbCommand, connectionTxPair.Connection, connectionTxPair.Transaction, commandType, commandText,
					               parameterValues);

					ApplyTransactionTimeout(dbCommand, timeoutInSeconds);

					object executeScalar = dbCommand.ExecuteScalar();
					ParameterUtils.CopyParameters(parameterValues, dbCommand);
					return executeScalar;
				}
			}
			catch (Exception ex)
			{
				ConnectionUtils.DisposeConnection(connectionTxPair.Connection, DbProvider);
				connectionTxPair.Connection = null;
				throw ex;
			}
			finally
			{
				ConnectionUtils.DisposeConnection(connectionTxPair.Connection, DbProvider);
			}
		}

		#endregion

		#region ExecuteReader

		/// <summary>
		/// 针对<see cref="System.Data.IDbConnection"/>执行<see cref="System.Data.IDbCommand.CommandText"/>,并生成
		/// <see cref="System.Data.IDataReader"/>
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="commandText">A execute Sql.</param>
		/// <returns>a datareader.</returns>
		public IDataReader ExecuteReader(string connString, string commandText)
		{
			return ExecuteReader(connString, CommandType.Text, commandText, TIMEOUT_DEFAULT, null);
		}

		/// <summary>
		/// 针对<see cref="System.Data.IDbConnection"/>执行<see cref="System.Data.IDbCommand.CommandText"/>,并生成
		/// <see cref="System.Data.IDataReader"/>
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="commandText">A execute Sql.</param>
		/// <param name="parameterValues">the sql parameters.</param>
		/// <returns>a datareader.</returns>
		public IDataReader ExecuteReader(string connString, string commandText, IDbParameters parameterValues)
		{
			return ExecuteReader(connString, CommandType.Text, commandText, TIMEOUT_DEFAULT, parameterValues);
		}

		/// <summary>
		/// 针对<see cref="System.Data.IDbConnection"/>执行<see cref="System.Data.IDbCommand.CommandText"/>,并生成
		/// <see cref="System.Data.IDataReader"/>
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="commandType">the CommandType. </param>
		/// <param name="commandText">A execute Sql.</param>
		/// <returns>a datareader.</returns>
		public IDataReader ExecuteReader(string connString, CommandType commandType, string commandText)
		{
			return ExecuteReader(connString, commandType, commandText, TIMEOUT_DEFAULT, null);
		}

		/// <summary>
		/// 针对<see cref="System.Data.IDbConnection"/>执行<see cref="System.Data.IDbCommand.CommandText"/>,并生成
		/// <see cref="System.Data.IDataReader"/>
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="commandType">the CommandType. </param>
		/// <param name="commandText">A execute Sql.</param>
		/// <param name="parameterValues">the sql parameters.</param>
		/// <returns>a datareader.</returns>
		public IDataReader ExecuteReader(string connString, CommandType commandType, string commandText,
		                                 IDbParameters parameterValues)
		{
			return ExecuteReader(connString, commandType, commandText, TIMEOUT_DEFAULT, parameterValues);
		}

		/// <summary>
		/// 针对<see cref="System.Data.IDbConnection"/>执行<see cref="System.Data.IDbCommand.CommandText"/>,并生成
		/// <see cref="System.Data.IDataReader"/>
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="commandType">the CommandType.</param>
		/// <param name="commandText">A execute Sql.</param>
		/// <param name="timeoutInSeconds">the dbcommand timeout.</param>
		/// <param name="parameterValues">the sql parameters.</param>
		/// <returns>a datareader.</returns>
		public IDataReader ExecuteReader(string connString, CommandType commandType, string commandText,
		                                 int timeoutInSeconds, IDbParameters parameterValues)
		{
			AssertUtils.StringNotNullOrEmpty(connString, "connString");

			ConnectionTxPair connectionTxPair = GetConnectionTxPair(connString);
			try
			{
                IDbCommand dbCommand = DbProvider.CreateCommand();
				
					PrepareCommand(dbCommand, connectionTxPair.Connection, connectionTxPair.Transaction, commandType, commandText,
					               parameterValues);

					ApplyTransactionTimeout(dbCommand, timeoutInSeconds);

					IDataReader executeReader = dbCommand.ExecuteReader();

					ParameterUtils.CopyParameters(parameterValues, dbCommand);

					return executeReader;
				
			}
			catch (Exception)
			{
				ConnectionUtils.DisposeConnection(connectionTxPair.Connection, DbProvider);
				connectionTxPair.Connection = null;
				throw;
			}
			finally
			{
				//ConnectionUtils.DisposeConnection(connectionTxPair.Connection, DbProvider);
			}
		}

		#endregion

		#region Queries with RowMapper<T>

		public IList<T> QueryWithRowMapper<T>(string connString,
		                                      string commandText,
		                                      IDataTableRowMapper<T> rowMapper)
		{
			return QueryWithRowMapper(connString, CommandType.Text, commandText, TIMEOUT_DEFAULT, null, 0, rowMapper);
		}

		public IList<T> QueryWithRowMapper<T>(string connString,
		                                      string commandText,
		                                      IDbParameters parameterValues,
		                                      IDataTableRowMapper<T> rowMapper)
		{
			return QueryWithRowMapper(connString, CommandType.Text, commandText, TIMEOUT_DEFAULT, parameterValues, 0, rowMapper);
		}

		public IList<T> QueryWithRowMapper<T>(string connString,
		                                      CommandType commandType,
		                                      string commandText,
		                                      int timeoutInSeconds,
		                                      IDbParameters parameterValues,
		                                      int tableIndex,
		                                      IDataTableRowMapper<T> rowMapper)
		{
			return QueryWithResultSetExtractor(connString,
			                                   commandType,
			                                   commandText,
			                                   timeoutInSeconds,
			                                   parameterValues,
			                                   tableIndex,
			                                   new RowMapperResultSetExtractor<T>(rowMapper));
		}

		#endregion

		#region Queries with RowMapperDelegate<T>

		public IList<T> QueryWithRowMapperDelegate<T>(string connString,
		                                              string commandText,
		                                              DataTableRowMapperDelegate<T> rowMapper)
		{
			return QueryWithRowMapperDelegate(connString, CommandType.Text, commandText, TIMEOUT_DEFAULT, null, 0, rowMapper);
		}

		public IList<T> QueryWithRowMapperDelegate<T>(string connString,
		                                              string commandText,
		                                              IDbParameters parameterValues,
		                                              DataTableRowMapperDelegate<T> rowMapper)
		{
			return QueryWithRowMapperDelegate(connString, CommandType.Text, commandText, TIMEOUT_DEFAULT, parameterValues, 0,
			                                  rowMapper);
		}

		public IList<T> QueryWithRowMapperDelegate<T>(string connString,
		                                              CommandType commandType,
		                                              string commandText,
		                                              int timeoutInSeconds,
		                                              IDbParameters parameterValues,
		                                              int tableIndex,
		                                              DataTableRowMapperDelegate<T> rowMapperDelegate)
		{
			return QueryWithResultSetExtractor(connString,
			                                   commandType,
			                                   commandText,
			                                   timeoutInSeconds,
			                                   parameterValues,
			                                   tableIndex,
			                                   new RowMapperResultSetExtractor<T>(rowMapperDelegate));
		}

		#endregion

		#region Queries with ResultSetExtractor<T>

		private T QueryWithResultSetExtractor<T>(string connString,
		                                         CommandType commandType,
		                                         string commandText,
		                                         int timeoutInSeconds,
		                                         IDbParameters parameterValues,
		                                         int tableIndex,
		                                         IResultSetExtractor<T> resultSetExtractor)
		{
			AssertUtils.ArgumentNotNull(resultSetExtractor, "resultSetExtractor", "Result Set Extractor must not be null");

			DataSet dataset = ExecuteDataset(connString,
			                                 commandType,
			                                 commandText,
			                                 timeoutInSeconds,
			                                 parameterValues);
			return resultSetExtractor.ExtractData(dataset, tableIndex);
		}

		#endregion

		#region Query for Object<T>

		public T QueryForObject<T>(string connString,
		                           string commandText,
		                           IDataTableRowMapper<T> rowMapper)
		{
			return QueryForObject(connString, CommandType.Text, commandText, TIMEOUT_DEFAULT, null, 0, rowMapper);
		}

		public T QueryForObject<T>(string connString,
		                           string commandText,
		                           IDbParameters parameterValues,
		                           IDataTableRowMapper<T> rowMapper)
		{
			return QueryForObject(connString, CommandType.Text, commandText, TIMEOUT_DEFAULT, parameterValues, 0, rowMapper);
		}

		public T QueryForObject<T>(string connString,
		                           CommandType commandType,
		                           string commandText,
		                           int timeoutInSeconds,
		                           IDbParameters parameterValues,
		                           int tableIndex,
		                           IDataTableRowMapper<T> rowMapper)
		{
			IList<T> results = QueryWithRowMapper(connString, commandType, commandText, timeoutInSeconds, parameterValues,
			                                      tableIndex, rowMapper);
			if (results == null || results.Count == 0)
			{
				return default(T);
			}
			return results[0];
		}

		#endregion

		#region Query for ObjectDelegate<T>

		public T QueryForObjectDelegate<T>(string connString,
		                                   string commandText,
		                                   DataTableRowMapperDelegate<T> rowMapperDelegate)
		{
			return QueryForObjectDelegate(connString, CommandType.Text, commandText, TIMEOUT_DEFAULT, null, 0, rowMapperDelegate);
		}

		public T QueryForObjectDelegate<T>(string connString,
		                                   string commandText,
		                                   IDbParameters parameterValues,
		                                   DataTableRowMapperDelegate<T> rowMapperDelegate)
		{
			return QueryForObjectDelegate(connString, CommandType.Text, commandText, TIMEOUT_DEFAULT, parameterValues, 0,
			                              rowMapperDelegate);
		}

		public T QueryForObjectDelegate<T>(string connString,
		                                   CommandType commandType,
		                                   string commandText,
		                                   int timeoutInSeconds,
		                                   IDbParameters parameterValues,
		                                   int tableIndex,
		                                   DataTableRowMapperDelegate<T> rowMapperDelegate)
		{
			IList<T> results = QueryWithRowMapperDelegate(connString, commandType, commandText, timeoutInSeconds, parameterValues,
			                                              tableIndex, rowMapperDelegate);
			if (results == null || results.Count == 0)
			{
				return default(T);
			}
			return results[0];
		}

		#endregion
	}
}