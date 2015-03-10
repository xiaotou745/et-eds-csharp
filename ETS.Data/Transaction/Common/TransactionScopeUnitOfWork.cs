using System;
using System.Data;
using System.Transactions;
using ETS.Dao;
using ETS.Data.Core;

namespace ETS.Transaction.Common
{
	/// <summary>
	/// ʹ��<see cref="TransactionScope"/>ʵ���������
	/// ���ӱ��ӳٵ���һ�η���CurrentConnectionʱ��򿪣����Ǳ�������Ƕ��
	/// <see cref="TransactionScopeUnitOfWork"/>�е�<see cref="TransactionScope"/>������
	/// ��ǰ�򿪣���Ȼ�ͻ��������״̬��Ч��
	/// </summary>
	/// <remarks>
	/// <p>
	/// if the DTC was allowed, then the connHolder in thread was not needed.
	/// else if the DTC was now allowed, then the connHolder needed to hold in the current thread.
	/// </p>
	/// <p>
	/// ���DTC����ʹ�ã�����Ҫ��Connectionѹ���̵߳��У������е�DAO����ȫ��ʹ�ò�ͬ��Connection����.
	/// ���DTC������ʹ�ã�����ʼʱ���봴��һ��Connection����ѹ���߳��У������е�DAO����ȫ��ʹ���߳��е�Connection.
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
		/// �Ƿ�����ʹ�÷ֲ�ʽ�������,Ĭ��Ϊtrue.
		/// </summary>
		private bool isDtcAllowed = true;

		/// <summary>
		/// get or set whether the DTC is allowed, return true if allowed, else false.
		/// the default value is true.
		/// �Ƿ�����ʹ�÷ֲ�ʽ������ơ�
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

		#region ���캯��

		/// <summary>
		/// ʹ��<see cref="TransactionScope"/>ʵ��<see cref="IUnitOfWork"/>�ӿ�
		/// if the parameter <paramref name="isDtcAllowed"/> is true, the connection of UnitOfWork not needed to create.
		/// else the connection need to create and bound to the thread.
		/// </summary>
		/// <param name="isDtcAllowed">�Ƿ�����ʹ��DTC</param>
		/// <param name="transactionScopeFactory">ʵ��<see cref="ITransactionScopeFactory"/>�ӿڵ�ʵ������������<see cref="TransactionScope"/></param>
		/// <param name="unitOfWorkDefinition">ʵ��<see cref="IUnitOfWorkDefinition"/>�ӿڵ�ʵ���������������񻷾�</param>
		/// <param name="dbProvider">ʵ��<see cref="IDbProvider"/>��ʵ��.</param>
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

			//������ʹ�÷ֲ�ʽ������DAO��ʽ����ʹ��ͬһ��Connection����Ҫ������ʼǰ����Connection�������뵱ǰ�߳��С�
			if (!IsDTCAllowed)
			{
				CheckDbProvider(dbProvider);
				DbProvider = dbProvider;

				//��ʼ���񣬴������ӣ���ѹ�뵱ǰ�߳�
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
			//���������ʹ��DTC,����Ҫ�ͷ��߳��е�Connection
			if (!isDtcAllowed)
			{
				//�ӵ�ǰ�߳��л�ȡConnectionHolder
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