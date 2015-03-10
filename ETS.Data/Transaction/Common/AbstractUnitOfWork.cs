using System;
using Common.Logging;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using ETS.Util;

namespace ETS.Transaction.Common
{
	/// <summary>
	/// UnitOfWork ����
	/// </summary>
	public abstract class AbstractUnitOfWork : IUnitOfWork
	{
		#region Logging Definition

		/// <summary>
		/// get the <see cref="ILog"/> with a WYC.Data.txt file appender
		/// </summary>
		protected static readonly ILog Logger = DbLogManager.GetConnectionStringLog();

		#endregion

		#region Propeties

		/// <summary>
		/// �Ƿ����������Ĭ��Ϊtrue
		/// </summary>
		protected bool IsOuterMostUnitOfWork = true;

		#endregion

		#region IUnitOfWork Members

		/// <summary>
		/// Dispose
		/// </summary>
		public virtual void Dispose()
		{
			//��֤ջ�������IUnitOfWork�Ƿ��ǵ�ǰIUnitOfWork.������ǣ��׳��쳣��
			AssertUtils.State(this == UnitOfWorkStackManager.Pop(), "[AbstractUnitOfWork.Dispose]:Disposing UnitOfWork must on top of the ThreadBoundedStack.");
		}

		public virtual void Complete()
		{
			Logger.Debug("[AbstractUnitOfWork.Complete]:Unit Of Work Complete here.");
		}

		#endregion

		#region ���캯��

		protected AbstractUnitOfWork()
		{
			Init();
		}

		protected void Init()
		{
			//�߳����Ƿ����ջ,�������ջ����ǰ����ΪǶ�����񣬷���Ϊ���������
			if (UnitOfWorkStackManager.ThreadBoundUnitOfWorkStackExists)
			{
				IsOuterMostUnitOfWork = false;
				Logger.Debug(m => m("[AbstractUnitOfWork.Init]:the initiates UnitOfWork is Nested UnitOfWork,Inner(deepth:{0})", UnitOfWorkStackManager.Count+1));
			}
			else
			{
				IsOuterMostUnitOfWork = true;
				Logger.Debug(m => m("[AbstractUnitOfWork.Init]:the initiates UnitOfWork is OutMost UnitOfWork."));
			}
			//�ѵ�ǰUnitOfWorkѹ��ջ��
			UnitOfWorkStackManager.Push(this);
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// get the <see cref="ConnectionHolder"/> from the current thread
		/// </summary>
		/// <returns>The<see cref="ConnectionHolder"/> of the current thread.</returns>
		protected ConnectionHolder GetConnectionHolder(IDbProvider dbProvider)
		{
			CheckDbProvider(dbProvider);
			var conHolder = (ConnectionHolder) TransactionSynchronizationManager.GetResource(dbProvider);
			return conHolder;
		}

		/// <summary>
		/// ��֤DbProvider�Ƿ����Ҫ��
		/// </summary>
		protected void CheckDbProvider(IDbProvider dbProvider)
		{
			if (dbProvider == null)
			{
				throw new DbProviderException("DbProvider is required to be set on the UnitOfWork");
			}
			if (string.IsNullOrEmpty(dbProvider.ConnectionString))
			{
				throw new DbProviderException("DbProvider's ConnectionString can not be null and Empty");
			}
		}

		/// <summary>
		/// ��֤UnitOfWorkDefinition�Ƿ����쳣.
		/// </summary>
		/// <param name="unitOfWorkDefinition">The UnitOfWorkDefinition.</param>
		protected void CheckUnitOfWorkDefinition(IUnitOfWorkDefinition unitOfWorkDefinition)
		{
			if (unitOfWorkDefinition == null)
			{
				return;
			}
			// Check definition settings for new transaction.
			if (unitOfWorkDefinition.Timeout < DefaultUnitOfWorkDefinition.TIMEOUT_DEFAULT)
			{
				throw new InvalidUnitOfWorkDefinitionException("Invalid transaction timeout");
			}
		}

		/// <summary>
		/// �ͷŵ�ǰ�߳��е�ConnectionHolder�����ӵ�ǰ�߳�����մ���Դ��
		/// </summary>
		/// <param name="dbProvider">the DbProvider.</param>
		protected void DisposeConnHolderBoundThread(IDbProvider dbProvider)
		{
			//�ӵ�ǰ�߳��л�ȡConnectionHolder
			ConnectionHolder connHolder = GetConnectionHolder(dbProvider);
			if (connHolder == null)
			{
				Logger.Debug("[AbstractUnitOfWork.DisposeConnHolderBoundThread]:Dispose UnitOfWork failed, because the connHolder bound current thread is null");
				TransactionSynchronizationManager.UnbindResource(dbProvider);
				return;
			}
			//�������������������Dispose
			if (IsOuterMostUnitOfWork)
			{
				if (connHolder.Connection != null)
				{
					connHolder.Connection.Dispose();
				}
				if (connHolder.Transaction != null)
				{
					connHolder.Transaction.Dispose();
				}
				TransactionSynchronizationManager.UnbindResource(dbProvider);

				Logger.Debug("[AbstractUnitOfWork.DisposeConnHolderBoundThread]:Outer most unit of work disposing, disposing connHolder, unbindResource connHolder in thread.");
			}
			else
			{
				//It's the transactional connection bound to the thread so don't close it.
				Logger.Debug(
					m =>
					m(
						"[AbstractUnitOfWork.DisposeConnHolderBoundThread]:It's the transactional connHolder bound to the thread so don't close it. Inner UnitOfWork(deepth:{0}) disposing, keep connHolder in thread.",
						UnitOfWorkStackManager.Count + 1));
			}
		}

		/// <summary>
		/// ���ݵ�ǰ�߳��е�ConnHolder����֤�Ƿ�������������ʶ
		/// </summary>
		protected void CheckUnitOfWorkOutMostFlag(IDbProvider dbProvider)
		{
			ConnectionHolder connectionHolder = GetConnectionHolder(dbProvider);
			//��ǰ�߳��д���ConnHolder,�����ϴ�����ӦΪǶ�����񣬵��Ǳ�ʶΪ��������񣬳�ͻ��
			if (connectionHolder != null && IsOuterMostUnitOfWork)
			{
				throw new InValidUnitOfWorkException("��ǰ�߳��д���ConnHolder,�����ϴ�����ӦΪǶ�����񣬵���IsOuterMostUnitOfWork��ʶΪ��������񣬳�ͻ��");
			}
			//��ǰ�߳��в�����ConnHolder,�����ϴ�����ӦΪ��������񣬵���IsOuterMostUnitOfWork��ʶΪ��������񣬳�ͻ��
			if (connectionHolder == null && !IsOuterMostUnitOfWork)
			{
				throw new InValidUnitOfWorkException("��ǰ�߳��в�����ConnHolder,�����ϴ�����ӦΪ��������񣬵���IsOuterMostUnitOfWork��ʶΪ��������񣬳�ͻ��");
			}
		}

		/// <summary>
		/// ����ʼ
		/// </summary>
		protected void BeginTransaction(IDbProvider dbProvider, IUnitOfWorkDefinition definition)
		{
			CheckUnitOfWorkOutMostFlag(dbProvider);

			//�ӵ�ǰ�߳��л�ȡ
			ConnectionHolder conHolder = GetConnectionHolder(dbProvider);

			//�����ǰ�߳��д���ConnectionHolder���������ΪǶ��������������Ѵ��ڶ��ꣻ
			if (conHolder != null)
			{
				Logger.Debug(m => m("[AbstractUnitOfWork.BeginTransaction]:Inner(deepth:{0}) unit of work initiates with definition({1})!",
				                    UnitOfWorkStackManager.Count, definition));
			}
			else //�������
			{
				Logger.Debug(m => m("[AbstractUnitOfWork.BeginTransaction]:Outer most unit of work initiates with definition({0})!", definition));
				ConnectionHolder connectionHolder = CreateConnectionHolder();
				TransactionSynchronizationManager.BindResource(dbProvider, connectionHolder);
				Logger.Debug(m => m("[AbstractUnitOfWork.BeginTransaction]:UnitOfWork created with(connstring:{0})", connectionHolder.Connection.ConnectionString));
			}
		}

		/// <summary>
		/// ����ConnectionHolder
		/// </summary>
		/// <returns>returns a ConnectionHolder.</returns>
		protected virtual ConnectionHolder CreateConnectionHolder()
		{
			throw new NotImplementedException("����������ʵ�ִ˷���.");
		}

		/// <summary>
		/// ����TimeOut�����Timeoutֵ
		/// </summary>
		/// <param name="definition">the <see cref="IUnitOfWorkDefinition"/></param>
		/// <returns>returns the timeout of the current unitofwork.</returns>
		protected int DetermineTimeout(IUnitOfWorkDefinition definition)
		{
			if (definition.Timeout != DefaultUnitOfWorkDefinition.TIMEOUT_DEFAULT)
			{
				return definition.Timeout;
			}
			return DefaultUnitOfWorkDefinition.TIMEOUT_DEFAULT;
		}

		#endregion
	}
}