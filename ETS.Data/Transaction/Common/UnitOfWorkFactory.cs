using System;
using ETS.Data.Core;

namespace ETS.Transaction.Common
{
	/// <summary>
	/// �ṩ��ȡ<see cref="IUnitOfWork"/>�ĸ��ַ���
	/// </summary>
	public class UnitOfWorkFactory
	{
		/// <summary>
		/// returns a <see cref="IUnitOfWork"/> instance based on the Ado.NET.
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <returns>��������Ԫ������<see cref="IUnitOfWork"/>ʵ������ʵ������Ado.Netʵ��<see cref="IUnitOfWork"/>�ӿ�.</returns>
		public static IUnitOfWork GetAdoNetUnitOfWork(string connString)
		{
			return GetAdoNetUnitOfWork(connString, null);
		}

		/// <summary>
		/// returns a <see cref="IUnitOfWork"/> instance based on the Ado.NET.
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="unitOfWorkDefinition">A UnitOfWorkDefinition.</param>
		/// <returns>��������Ԫ������<see cref="IUnitOfWork"/>ʵ������ʵ������Ado.Netʵ��<see cref="IUnitOfWork"/>�ӿ�.</returns>
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
		/// <returns>��������Ԫ������<see cref="IUnitOfWork"/>ʵ������ʵ������TransactionScopeʵ��<see cref="IUnitOfWork"/>�ӿ�.</returns>
		public static IUnitOfWork GetTransactionScopeUnitOfWorkWithDTCUsed(IUnitOfWorkDefinition unitOfWorkDefinition)
		{
			return TransactionScopeUnitOfWorkFactory.Create().SetIsDtcAllowed(true).GetUnitOfWork(unitOfWorkDefinition);
		}

		/// <summary>
		/// return a <see cref="IUnitOfWork"/> based on the <see cref="System.Transactions.TransactionScope"/>.
		/// ��ǰUnit Work�����У����ݿ��DTC���ã�����ȫ��װTransactionScope��������Conenction��ѹ���̣߳�
		/// </summary>
		/// <returns>��������Ԫ������<see cref="IUnitOfWork"/>ʵ������ʵ������TransactionScopeʵ��<see cref="IUnitOfWork"/>�ӿ�.</returns>
		public static IUnitOfWork GetTransactionScopeUnitOfWorkWithDTCUsed()
		{
			return TransactionScopeUnitOfWorkFactory.Create().SetIsDtcAllowed(true).GetUnitOfWork(null);
		}

		/// <summary>
		/// return a <see cref="IUnitOfWork"/> based on the <see cref="System.Transactions.TransactionScope"/>.
		/// ��ǰUnit Work�����У����ݿ��DTC�����ã�UnitWork�е�����DAO��������ͬһ��Connection.
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <param name="unitOfWorkDefinition">A UnitOfWorkDefinition.</param>
		/// <returns>��������Ԫ������<see cref="IUnitOfWork"/>ʵ������ʵ������TransactionScopeʵ��<see cref="IUnitOfWork"/>�ӿ�.</returns>
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
		/// ��ǰUnit Work�����У����ݿ��DTC�����ã�UnitWork�е�����DAO��������ͬһ��Connection.
		/// </summary>
		/// <param name="connString">A ConnectionString.</param>
		/// <returns>��������Ԫ������<see cref="IUnitOfWork"/>ʵ������ʵ������TransactionScopeʵ��<see cref="IUnitOfWork"/>�ӿ�.</returns>
		public static IUnitOfWork GetTransactionScopeUnitOfWorkWithoutDTC(string connString)
		{
			IDbProvider dbProvider = CreateDbProvider(connString);
			return TransactionScopeUnitOfWorkFactory.Create().SetIsDtcAllowed(false).SetDbProvider(dbProvider).GetUnitOfWork(null);
		}

		/// <summary>
		/// ʹ��ָ���������ַ�������<see cref="IDbProvider"/>����
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