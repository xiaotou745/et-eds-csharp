using System;
using System.Data;
using System.Threading;
using Common.Logging;
using ETS.Dao;

namespace ETS.Data.Core
{
	/// <summary>
	/// Summary description for DbProviderUtils.
	/// </summary>
	public abstract class ConnectionUtils
	{
		#region Logging

		private static readonly ILog logger = DbLogManager.GetConnectionStringLog();

		#endregion

		#region DisposeConnection

		/// <summary>
		/// Dispose of the given Connection, created via the given IDbProvider,
		/// if it is not managed externally (that is, not bound to the thread).
		/// </summary>
		/// <param name="conn">The connection to close if necessary.  If
		/// this is null the call will be ignored. </param>
		/// <param name="dbProvider">The IDbProvider the connection came from</param>
		public static void DisposeConnection(IDbConnection conn, IDbProvider dbProvider)
		{
			try
			{
				if (conn == null)
				{
					return;
				}
				if (dbProvider != null)
				{
					var conHolder = (ConnectionHolder) TransactionSynchronizationManager.GetResource(dbProvider);
					if (conHolder != null && ConnectionEquals(conHolder.Connection, conn))
					{
						conHolder.Released();
						logger.Debug(
							"[ConnectionUtils.DisposeConnection]:It's the transactional connection bound to the thread so don't close it.");
						// It's the transactional connection bound to the thread so don't close it.
						return;
					}
				}
				logger.Debug("[ConnectionUtils.DisposeConnection]:Disposing of IDbConnection with connection string = [" +
				             conn.ConnectionString + "]");
				conn.Dispose();
			}
			catch (Exception e)
			{
				logger.Warn("[ConnectionUtils.DisposeConnection]:Could not close connection", e);
			}
		}

		private static bool ConnectionEquals(IDbConnection heldCon, IDbConnection passedInCon)
		{
			return (heldCon == passedInCon || heldCon.Equals(passedInCon));
		}

		#endregion


		//private static bool ConnectionStringInvalid(IDbProvider dbProvider,ConnectionHolder connHolder)
		//{
		//    using(IDbConnection dbConnection = dbProvider.CreateConnection(dbProvider.ConnectionString))
		//    {
		//        dbConnection.Open();
		//        logger.Info(dbConnection.ConnectionString);
		//        logger.Info(connHolder.Connection.ConnectionString);
		//        if (dbConnection.ConnectionString != connHolder.Connection.ConnectionString)
		//        {
		//            return true;
		//        }
		//    }
		//    return false;
		//}
		public static string RemoveConnStringPassword(string connString)
		{
			string result = connString;
			int pwdIndexStart = result.ToLower().IndexOf("pwd=", StringComparison.Ordinal);
			if (pwdIndexStart >= 0)
			{
				int pwdIndexEnd = result.ToLower().IndexOf(";", pwdIndexStart, StringComparison.Ordinal);
				if (pwdIndexEnd >= 0)
				{
					result = result.Remove(pwdIndexStart, pwdIndexEnd - pwdIndexStart + 1);
				}
				else
				{
					result = result.Remove(pwdIndexStart, result.Length - pwdIndexStart);
				}
			}
			return result;
		}
		#region GetConnectionTxPair

		/// <summary>
		/// Get a ADO.NET Connection/Transaction Pair for the given IDbProvider.
		/// </summary>
		/// <remarks>
		/// Is aware of a corresponding Connection/Transaction bound to the current thread, for example
		/// when using AdoPlatformTransactionManager. Will bind a IDbConnection to the thread
		/// if transaction synchronization is active
		/// </remarks>
		/// <param name="provider">The provider.</param>
		/// <returns>A Connection/Transaction pair.</returns>
		public static ConnectionTxPair GetConnectionTxPair(IDbProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (string.IsNullOrEmpty(provider.ConnectionString))
			{
				throw new ArgumentNullException("provider", "provider.ConenctionString can not be null or empty.");
			}
			try
			{
				var conHolder = (ConnectionHolder) TransactionSynchronizationManager.GetResource(provider);
				if (conHolder != null)
				{
					conHolder.Requested();
					logger.Debug(
						"[ConnectionUtils.GetConnectionTxPair]:get connectionHolder  for key [IDbProvider] bound to thread ["
						+ Thread.CurrentThread.ManagedThreadId + "]");
					if (!conHolder.HasConnection || conHolder.Transaction == null)
					{
						throw new InvalidConnectionHolderException(
							"[ConnectionUtils.GetConnectionTxPair]:从当前线程中获取的ConnectionHolder的Connection|Transaction为Null");
					}
					//ConnectionStringInvalid(provider, conHolder);
					//if (conHolder.Connection.ConnectionString != provider.ConnectionString)
					//{
					//    throw new InvalidConnectionHolderException("[ConnectionUtils.GetConnectionTxPair]:该方法连接字符串与从当前线程中获取的ConnectionHolder的连接字符串不一致.事务异常终止");
					//}
					return new ConnectionTxPair(conHolder.Connection, conHolder.Transaction);
				}

				// Else we either got no holder or an empty thread-bound holder here.
				logger.Debug("[ConnectionUtils.GetConnectionTxPair]:Create Connection use DbProvider");
				IDbConnection conn = provider.CreateConnection();
				conn.ConnectionString = provider.ConnectionString;
				conn.Open();
				return new ConnectionTxPair(conn, null);
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		#endregion

		#region ApplyTransactionTimeout

		/// <summary>
		/// Applies the specified timeout - overridden by the current transaction timeout, if any, to to the
		/// given ADO.NET IDb command object.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="dbProvider">The db provider the command was obtained from.</param>
		/// <param name="timeout">The timeout to apply (or 0 for no timeout outside of a transaction.</param>
		public static void ApplyTransactionTimeout(IDbCommand command, IDbProvider dbProvider, int timeout)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command", "No DbCommand specified.");
			}
			if (dbProvider == null)
			{
				throw new ArgumentNullException("dbProvider", "No IDbProvider specified.");
			}

			var conHolder = (ConnectionHolder) TransactionSynchronizationManager.GetResource(dbProvider);
			//事务内超时时间控制
			if (conHolder != null && conHolder.HasTimeout)
			{
				logger.DebugFormat("[ConnectionUtils.ApplyTransactionTimeout]:使用当前Transaction的timeout值:{0}覆盖指定timeout:{1}",
				                   conHolder.TimeToLiveInSeconds, timeout);
				// Remaining transaction timeout overrides specified value.
				command.CommandTimeout = conHolder.TimeToLiveInSeconds;
			}
			else if (timeout != -1) //如果不是在事务中，除非指定了超时时间，否则不进行设置
			{
				logger.DebugFormat(
					"[ConnectionUtils.ApplyTransactionTimeout]:No current transaction timeout -> apply specified value:{0}.", timeout);
				// No current transaction timeout -> apply specified value.  0 = infinite timeout in some drivers.
				command.CommandTimeout = timeout;
			}
		}

		#endregion
	}
}