using System;
using ETS.Data.Core;

namespace ETS.Transaction.Common
{
	/// <summary>
	/// 提供获取<see cref="IUnitOfWork"/>的各种方法
	/// </summary>
	public class UnitOfWorkFactory
	{
		/// <summary>
		/// returns a <see cref="IUnitOfWork"/> instance based on the Ado.NET.
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <returns>返回事务单元管理者<see cref="IUnitOfWork"/>实例，该实例基于Ado.Net实现<see cref="IUnitOfWork"/>接口.</returns>
		public static IUnitOfWork GetAdoNetUnitOfWork(string connString)
		{
			return GetAdoNetUnitOfWork(connString, null);
		}

		/// <summary>
		/// returns a <see cref="IUnitOfWork"/> instance based on the Ado.NET.
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="unitOfWorkDefinition">A UnitOfWorkDefinition.</param>
		/// <returns>返回事务单元管理者<see cref="IUnitOfWork"/>实例，该实例基于Ado.Net实现<see cref="IUnitOfWork"/>接口.</returns>
		public static IUnitOfWork GetAdoNetUnitOfWork(string connString, IUnitOfWorkDefinition unitOfWorkDefinition)
		{
			IDbProvider dbProvider = CreateDbProvider(connString);
			return AdoNetUnitOfWorkFactory.Create()
				.SetDbProvider(dbProvider)
				.GetUnitOfWork(unitOfWorkDefinition);
		}

		/// <summary>
		/// return a <see cref="IUnitOfWork"/> based on the <see cref="System.Transactions.TransactionScope"/>.
		/// </summary>
		/// <param name="unitOfWorkDefinition">A UnitOfWorkDefinition.</param>
		/// <returns>返回事务单元管理者<see cref="IUnitOfWork"/>实例，该实例基于TransactionScope实现<see cref="IUnitOfWork"/>接口.</returns>
		public static IUnitOfWork GetTransactionScopeUnitOfWorkWithDTCUsed(IUnitOfWorkDefinition unitOfWorkDefinition)
		{
			return TransactionScopeUnitOfWorkFactory.Create().SetIsDtcAllowed(true).GetUnitOfWork(unitOfWorkDefinition);
		}

		/// <summary>
		/// return a <see cref="IUnitOfWork"/> based on the <see cref="System.Transactions.TransactionScope"/>.
		/// 当前Unit Work环境中，数据库的DTC可用，则完全封装TransactionScope，不进行Conenction的压入线程；
		/// </summary>
		/// <returns>返回事务单元管理者<see cref="IUnitOfWork"/>实例，该实例基于TransactionScope实现<see cref="IUnitOfWork"/>接口.</returns>
		public static IUnitOfWork GetTransactionScopeUnitOfWorkWithDTCUsed()
		{
			return TransactionScopeUnitOfWorkFactory.Create().SetIsDtcAllowed(true).GetUnitOfWork(null);
		}

		/// <summary>
		/// return a <see cref="IUnitOfWork"/> based on the <see cref="System.Transactions.TransactionScope"/>.
		/// 当前Unit Work环境中，数据库的DTC不可用，UnitWork中的所有DAO方法采用同一个Connection.
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="unitOfWorkDefinition">A UnitOfWorkDefinition.</param>
		/// <returns>返回事务单元管理者<see cref="IUnitOfWork"/>实例，该实例基于TransactionScope实现<see cref="IUnitOfWork"/>接口.</returns>
		public static IUnitOfWork GetTransactionScopeUnitOfWorkWithoutDTC(string connString,
		                                                                  IUnitOfWorkDefinition unitOfWorkDefinition)
		{
			IDbProvider dbProvider = CreateDbProvider(connString);
			return TransactionScopeUnitOfWorkFactory.Create()
				.SetIsDtcAllowed(false)
				.SetDbProvider(dbProvider)
				.GetUnitOfWork(unitOfWorkDefinition);
		}
		/// <summary>
		/// return a <see cref="IUnitOfWork"/> based on the <see cref="System.Transactions.TransactionScope"/>.
		/// 当前Unit Work环境中，数据库的DTC不可用，UnitWork中的所有DAO方法采用同一个Connection.
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <returns>返回事务单元管理者<see cref="IUnitOfWork"/>实例，该实例基于TransactionScope实现<see cref="IUnitOfWork"/>接口.</returns>
		public static IUnitOfWork GetTransactionScopeUnitOfWorkWithoutDTC(string connString)
		{
			IDbProvider dbProvider = CreateDbProvider(connString);
			return TransactionScopeUnitOfWorkFactory.Create().SetIsDtcAllowed(false).SetDbProvider(dbProvider).GetUnitOfWork(null);
		}

		/// <summary>
		/// 使用指定的连接字符串创建<see cref="IDbProvider"/>对象
		/// </summary>
		/// <param name="connString">A connectionString.</param>
		/// <returns>A DbProvider.</returns>
		private static IDbProvider CreateDbProvider(string connString)
		{
			if (string.IsNullOrEmpty(connString))
			{
				throw new ArgumentException("the connectionString provided to the provider can not be null.", "connString");
			}
			IDbProvider dbProvider = DbProviderFactoryObj.GetProvider();
			dbProvider.ConnectionString = connString;
			return dbProvider;
		}
	}
}