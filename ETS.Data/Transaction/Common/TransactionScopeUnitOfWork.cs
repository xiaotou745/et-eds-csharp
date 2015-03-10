using System;
using System.Data;
using System.Transactions;
using ETS.Dao;
using ETS.Data.Core;

namespace ETS.Transaction.Common
{
	/// <summary>
	/// 使用<see cref="TransactionScope"/>实事事务管理
	/// 连接被延迟到第一次访问CurrentConnection时候打开，但是必须在内嵌的
	/// <see cref="TransactionScopeUnitOfWork"/>中的<see cref="TransactionScope"/>被创建
	/// 以前打开，不然就会造成事务状态无效。
	/// </summary>
	/// <remarks>
	/// <p>
	/// if the DTC was allowed, then the connHolder in thread was not needed.
	/// else if the DTC was now allowed, then the connHolder needed to hold in the current thread.
	/// </p>
	/// <p>
	/// 如果DTC允许使用，则不需要把Connection压到线程当中，事务中的DAO方法全部使用不同的Connection即可.
	/// 如果DTC不允许使用，事务开始时必须创建一个Connection，并压入线程中，事务中的DAO方法全部使用线程中的Connection.
	/// </p>
	/// </remarks>
	public class TransactionScopeUnitOfWork : AbstractUnitOfWork
	{
		#region Feilds

		/// <summary>
		/// The TransactionScope
		/// </summary>
		private readonly TransactionScope scope;

		/// <summary>
		/// 是否允许使用分布式事务控制,默认为true.
		/// </summary>
		private bool isDtcAllowed = true;

		/// <summary>
		/// get or set whether the DTC is allowed, return true if allowed, else false.
		/// the default value is true.
		/// 是否允许使用分布式事务控制。
		/// </summary>
		public bool IsDTCAllowed
		{
			get { return isDtcAllowed; }
			set { isDtcAllowed = value; }
		}

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

		/// <summary>
		/// 使用<see cref="TransactionScope"/>实现<see cref="IUnitOfWork"/>接口
		/// if the parameter <paramref name="isDtcAllowed"/> is true, the connection of UnitOfWork not needed to create.
		/// else the connection need to create and bound to the thread.
		/// </summary>
		/// <param name="isDtcAllowed">是否允许使用DTC</param>
		/// <param name="transactionScopeFactory">实现<see cref="ITransactionScopeFactory"/>接口的实例，用来创建<see cref="TransactionScope"/></param>
		/// <param name="unitOfWorkDefinition">实现<see cref="IUnitOfWorkDefinition"/>接口的实例，用来定义事务环境</param>
		/// <param name="dbProvider">实现<see cref="IDbProvider"/>的实例.</param>
		public TransactionScopeUnitOfWork(bool isDtcAllowed, ITransactionScopeFactory transactionScopeFactory,
		                                  IUnitOfWorkDefinition unitOfWorkDefinition, IDbProvider dbProvider)
		{
			IsDTCAllowed = isDtcAllowed;

			if (unitOfWorkDefinition == null)
			{
				unitOfWorkDefinition = UnitOfWorkDefinition.DefaultDefinition;
			}
			CheckUnitOfWorkDefinition(unitOfWorkDefinition);
			Definition = unitOfWorkDefinition;

			if (transactionScopeFactory == null)
			{
				transactionScopeFactory = new TransactionScopeFactory(unitOfWorkDefinition);
			}

			scope = transactionScopeFactory.GetInstance();

			System.Transactions.Transaction.Current.TransactionCompleted +=
				(o, args) => Logger.Debug(m => m("Current transaction completed with status {0}.",
				                                 args.Transaction.TransactionInformation.Status));

			//不允许使用分布式事务，则DAO方式必须使用同一个Connection，需要在事务开始前创建Connection，并放入当前线程中。
			if (!IsDTCAllowed)
			{
				CheckDbProvider(dbProvider);
				DbProvider = dbProvider;

				//开始事务，创建连接，并压入当前线程
				BeginTransaction(DbProvider, Definition);
			}
		}

		#endregion

		public override void Complete()
		{
			base.Complete();
			scope.Complete();
		}

		public override void Dispose()
		{
			base.Dispose();
			//如果不允许使用DTC,则需要释放线程中的Connection
			if (!isDtcAllowed)
			{
				//从当前线程中获取ConnectionHolder
				DisposeConnHolderBoundThread(DbProvider);
			}
			scope.Dispose();
		}

		#region Override.

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

				var connectionHolder = new ConnectionHolder(dbConnection, null);

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