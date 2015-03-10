using System;
using Common.Logging;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using ETS.Util;

namespace ETS.Transaction.Common
{
	/// <summary>
	/// UnitOfWork 基类
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
		/// 是否最外层事务，默认为true
		/// </summary>
		protected bool IsOuterMostUnitOfWork = true;

		#endregion

		#region IUnitOfWork Members

		/// <summary>
		/// Dispose
		/// </summary>
		public virtual void Dispose()
		{
			//验证栈的最顶部的IUnitOfWork是否是当前IUnitOfWork.如果不是，抛出异常。
			AssertUtils.State(this == UnitOfWorkStackManager.Pop(), "[AbstractUnitOfWork.Dispose]:Disposing UnitOfWork must on top of the ThreadBoundedStack.");
		}

		public virtual void Complete()
		{
			Logger.Debug("[AbstractUnitOfWork.Complete]:Unit Of Work Complete here.");
		}

		#endregion

		#region 构造函数

		protected AbstractUnitOfWork()
		{
			Init();
		}

		protected void Init()
		{
			//线程中是否存在栈,如果存在栈，则当前事务为嵌套事务，否则为最外层事务；
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
			//把当前UnitOfWork压入栈中
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
		/// 验证DbProvider是否符合要求
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
		/// 验证UnitOfWorkDefinition是否有异常.
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
		/// 释放当前线程中的ConnectionHolder，并从当前线程中清空此资源。
		/// </summary>
		/// <param name="dbProvider">the DbProvider.</param>
		protected void DisposeConnHolderBoundThread(IDbProvider dbProvider)
		{
			//从当前线程中获取ConnectionHolder
			ConnectionHolder connHolder = GetConnectionHolder(dbProvider);
			if (connHolder == null)
			{
				Logger.Debug("[AbstractUnitOfWork.DisposeConnHolderBoundThread]:Dispose UnitOfWork failed, because the connHolder bound current thread is null");
				TransactionSynchronizationManager.UnbindResource(dbProvider);
				return;
			}
			//如果是最外层事务，则进行Dispose
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
		/// 根据当前线程中的ConnHolder来验证是否是最外层事务标识
		/// </summary>
		protected void CheckUnitOfWorkOutMostFlag(IDbProvider dbProvider)
		{
			ConnectionHolder connectionHolder = GetConnectionHolder(dbProvider);
			//当前线程中存在ConnHolder,理论上此事务应为嵌套事务，但是标识为最外层事务，冲突！
			if (connectionHolder != null && IsOuterMostUnitOfWork)
			{
				throw new InValidUnitOfWorkException("当前线程中存在ConnHolder,理论上此事务应为嵌套事务，但是IsOuterMostUnitOfWork标识为最外层事务，冲突！");
			}
			//当前线程中不存在ConnHolder,理论上此事务应为最外层事务，但是IsOuterMostUnitOfWork标识为非外层事务，冲突！
			if (connectionHolder == null && !IsOuterMostUnitOfWork)
			{
				throw new InValidUnitOfWorkException("当前线程中不存在ConnHolder,理论上此事务应为最外层事务，但是IsOuterMostUnitOfWork标识为非外层事务，冲突！");
			}
		}

		/// <summary>
		/// 事务开始
		/// </summary>
		protected void BeginTransaction(IDbProvider dbProvider, IUnitOfWorkDefinition definition)
		{
			CheckUnitOfWorkOutMostFlag(dbProvider);

			//从当前线程中获取
			ConnectionHolder conHolder = GetConnectionHolder(dbProvider);

			//如果当前线程中存在ConnectionHolder，则此事务为嵌套事务，外层事务已存在多年；
			if (conHolder != null)
			{
				Logger.Debug(m => m("[AbstractUnitOfWork.BeginTransaction]:Inner(deepth:{0}) unit of work initiates with definition({1})!",
				                    UnitOfWorkStackManager.Count, definition));
			}
			else //外层事务
			{
				Logger.Debug(m => m("[AbstractUnitOfWork.BeginTransaction]:Outer most unit of work initiates with definition({0})!", definition));
				ConnectionHolder connectionHolder = CreateConnectionHolder();
				TransactionSynchronizationManager.BindResource(dbProvider, connectionHolder);
				Logger.Debug(m => m("[AbstractUnitOfWork.BeginTransaction]:UnitOfWork created with(connstring:{0})", connectionHolder.Connection.ConnectionString));
			}
		}

		/// <summary>
		/// 创建ConnectionHolder
		/// </summary>
		/// <returns>returns a ConnectionHolder.</returns>
		protected virtual ConnectionHolder CreateConnectionHolder()
		{
			throw new NotImplementedException("请在子类中实现此方法.");
		}

		/// <summary>
		/// 决定TimeOut事务的Timeout值
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