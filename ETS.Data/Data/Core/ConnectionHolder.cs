using System;
using System.Data;
using ETS.Dao;

namespace ETS.Data.Core
{
	/// <summary>
	/// Connection holder, wrapping a ADO.NET connection and transaction.
	/// </summary>
	/// <remarks>AdoTransactionManager binds instances of this class to the
	/// thread for a given DbProvider.</remarks>
	public class ConnectionHolder
	{
		/// <summary>
		/// The <see cref="IDbConnection"/>
		/// </summary>
		private IDbConnection currentConnection;

		/// <summary>
		/// 获取次数
		/// </summary>
		private int referenceCount;

		/// <summary>
		/// Create a new ConnectionHolder
		/// </summary>
		/// <param name="conn">The connection to hold</param>
		/// <param name="transaction">The transaction to hold</param>
		public ConnectionHolder(IDbConnection conn, IDbTransaction transaction)
		{
			//TODO assert conn is not null.
			currentConnection = conn;
			Transaction = transaction;
		}

		/// <summary>
		/// 
		/// </summary>
		public int RefrerenceCount
		{
			get { return referenceCount; }
		}

		/// <summary>
		/// get or set the <see cref="IDbConnection"/> of the holder
		/// </summary>
		public IDbConnection Connection
		{
			get { return currentConnection; }
			set { currentConnection = value; }
		}

		/// <summary>
		/// get or set the <see cref="IDbTransaction"/> of the holder
		/// </summary>
		public IDbTransaction Transaction { get; set; }

		/// <summary>
		/// 获取当前<see cref="ConnectionHolder"/>是否拥有<see cref="IDbConnection"/>
		/// </summary>
		/// <remarks>
		/// return true if the Connection is not null, else false
		/// </remarks>
		public bool HasConnection
		{
			get { return (currentConnection != null); }
		}

		/// <summary>
		/// Increase the reference count by one because the holder has been requested.
		/// </summary>
		public void Requested()
		{
			referenceCount++;
		}

		/// <summary>
		/// Decrease the reference count by one because the holder has been released.
		/// </summary>
		public void Released()
		{
			referenceCount--;
		}

		#region 超时时间实现 
		private DateTime deadline;

		/// <summary>
		/// Return the expiration deadline of this object.
		/// </summary>
		public DateTime Deadline
		{
			get { return deadline; }
		}

		/// <summary>
		/// Return whether this object has an associated timeout.
		/// </summary>
		public bool HasTimeout
		{
			get { return (deadline != DateTime.MinValue); }
		}

		/// <summary>
		/// Return the time to live for this object in seconds.
		/// </summary>
		/// <remarks>
		/// <p>
		/// Rounds up eagerly, e.g. '9.00001' to '10'.
		/// </p>
		/// </remarks>
		/// <exception cref="System.ArgumentException">
		/// If no deadline has been set.
		/// </exception>
		public int TimeToLiveInSeconds
		{
			get
			{
				int secs = (int)Math.Ceiling(TimeToLiveInMilliseconds / 1000);
				checkTransactionTimeout(secs <= 0);
				return secs;
			}
		}

		/// <summary>
		/// Return the time to live for this object in milliseconds.
		/// </summary>
		/// <exception cref="System.ArgumentException">
		/// If no deadline has been set.
		/// </exception>
		public double TimeToLiveInMilliseconds
		{
			get
			{
				if (deadline == DateTime.MinValue)
				{
					throw new ArgumentException("No deadline specified for this resource holder.");
				}
				TimeSpan duration = deadline - DateTime.Now;
				checkTransactionTimeout(duration.TotalMilliseconds <= 0);
				if (duration.TotalMilliseconds > 0)
				{
					return duration.TotalMilliseconds;
				}
				else
				{
					return 0;
				}
			}
		}

		/// <summary>
		/// Sets the timeout for this object in milliseconds.
		/// </summary>
		/// <value>Number of milliseconds until expiration.</value>
		public long TimeoutInMillis
		{
			set
			{
				deadline = DateTime.Now.AddMilliseconds(value);
			}
		}

		/// <summary>
		/// Sets the timeout for this object in seconds.
		/// </summary>
		/// <value>Number of seconds until expiration.</value>
		public int TimeoutInSeconds
		{
			set
			{
				TimeoutInMillis = value * 1000;
			}
		}

		private void checkTransactionTimeout(bool deadlineReached)
		{
			if (deadlineReached)
			{
				throw new TransactionTimedOutException("Transaction timed out: deadline was " + Deadline);
			}
		}
		#endregion
	}
}