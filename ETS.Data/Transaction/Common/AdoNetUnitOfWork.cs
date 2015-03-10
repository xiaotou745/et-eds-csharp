using System;
using System.Data;
using ETS.Dao;
using ETS.Data.Core;

namespace ETS.Transaction.Common
{
	/// <summary>
	/// ADO.NET based implementation of the <see cref="IUnitOfWork"/> interface.
	/// </summary>
	/// <remarks>
	/// 1.the property of IDbProvider can not be null,because it used to create Connection and Transaction.
	/// 2.the property of IUnitOfWorkDefinition can be null, if it is null, then use the <see cref="DefaultUnitOfWorkDefinition"/>
	/// </remarks>
	public class AdoNetUnitOfWork : AbstractUnitOfWork
	{
		#region Properties

		/// <summary>
		/// get or set The <see cref="IDbProvider"/>
		/// </summary>
		protected IDbProvider DbProvider { get; set; }

		/// <summary>
		/// get the <see cref="IUnitOfWorkDefinition"/>
		/// </summary>
		protected IUnitOfWorkDefinition Definition { get; set; }

		#endregion

		#region 构造函数

		public AdoNetUnitOfWork(IDbProvider dbProvider) : this(dbProvider, null)
		{
		}

		public AdoNetUnitOfWork(IDbProvider dbProvider, IUnitOfWorkDefinition unitOfWorkDefinition)
		{
			//验证DbProvider
			CheckDbProvider(dbProvider);
			DbProvider = dbProvider;

			if (unitOfWorkDefinition == null)
			{
				unitOfWorkDefinition = UnitOfWorkDefinition.DefaultDefinition;
			}
			// Check definition settings for new transaction.
			CheckUnitOfWorkDefinition(unitOfWorkDefinition);
			Definition = unitOfWorkDefinition;

			BeginTransaction(DbProvider, Definition);
		}

		#endregion

		public override void Dispose()
		{
			base.Dispose();

			DisposeConnHolderBoundThread(DbProvider);
		}

		public override void Complete()
		{
			base.Complete();

			if(IsOuterMostUnitOfWork)
			{
				ConnectionHolder connectionHolder = GetConnectionHolder(DbProvider);
				if (connectionHolder == null)
				{
					throw new InvalidConnectionHolderException(
						"Can't complete the UnitOfWork, because the connHolder in current thread is null.");
				}
				if (connectionHolder.Transaction == null)
				{
					throw new InvalidConnectionHolderException(
						"Can't complete the UnitOfWork, because the Transaction of the connHolder in current thread is null");
				}
				connectionHolder.Transaction.Commit();
				Logger.Debug("[AdoNetUnitOfWork.Complete]:the transaction of the connectionHolder in current thread was commit over.");
			}
			else
			{
				Logger.DebugFormat(
					"It's the inner UnitOfWork bound to the thread so don't commit the connHolder's transaction.Inner(deepth:{0})",
					UnitOfWorkStackManager.Count);
			}
		}

		#region Overried

		/// <summary>
		/// Create the ConnHolder by DbProvider.
		/// </summary>
		/// <returns></returns>
		protected override ConnectionHolder CreateConnectionHolder()
		{
			IDbConnection dbConnection = null;

			try
			{
				dbConnection = DbProvider.CreateConnection();
				dbConnection.ConnectionString = DbProvider.ConnectionString;
				dbConnection.Open();

				IDbTransaction dbTransaction = dbConnection.BeginTransaction(Definition.IsolationLevel);

				var connectionHolder = new ConnectionHolder(dbConnection, dbTransaction);

				int timeout = DetermineTimeout(Definition);
				if (timeout != DefaultUnitOfWorkDefinition.TIMEOUT_DEFAULT)
				{
					connectionHolder.TimeoutInSeconds = timeout;
				}

				return connectionHolder;
			}
				//TODO catch specific exception
			catch (Exception e)
			{
				ConnectionUtils.DisposeConnection(dbConnection, DbProvider);
				throw new CannotCreateConnectionHolderException("Could not create ADO.NET ConnectionHolder for transaction", e);
			}
		}

		#endregion
	}
}