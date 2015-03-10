using ETS.Data.Core;

namespace ETS.Transaction.Common
{
	/// <summary>
	/// TransactionScope based implementation of the <see cref="IUnitOfWorkFactory"/> interface.
	/// </summary>
	public class TransactionScopeUnitOfWorkFactory : IUnitOfWorkFactory
	{
		private IDbProvider dbProvider;
		private bool isDtcAllowed = true;

		private TransactionScopeUnitOfWorkFactory()
		{
		}

		#region IUnitOfWorkFactory Members

		public IUnitOfWork GetUnitOfWork(IUnitOfWorkDefinition definition)
		{
			ITransactionScopeFactory transactionScopeFactory = new TransactionScopeFactory(definition);

			return new TransactionScopeUnitOfWork(isDtcAllowed, transactionScopeFactory, definition, dbProvider);
		}

		#endregion

		/// <summary>
		/// Create the <see cref="TransactionScopeUnitOfWorkFactory"/> by the private constructor.
		/// </summary>
		/// <returns></returns>
		public static TransactionScopeUnitOfWorkFactory Create()
		{
			return new TransactionScopeUnitOfWorkFactory();
		}

		/// <summary>
		/// �����Ƿ�����ʹ��DTC
		/// </summary>
		/// <param name="dtcAllowed">�Ƿ�����ʹ��DTC</param>
		/// <returns></returns>
		public TransactionScopeUnitOfWorkFactory SetIsDtcAllowed(bool dtcAllowed)
		{
			isDtcAllowed = dtcAllowed;
			return this;
		}

		/// <summary>
		/// ����DbProvider
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public TransactionScopeUnitOfWorkFactory SetDbProvider(IDbProvider provider)
		{
			dbProvider = provider;
			return this;
		}
	}
}