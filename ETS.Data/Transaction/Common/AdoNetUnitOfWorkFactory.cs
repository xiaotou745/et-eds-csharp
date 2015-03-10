using ETS.Data.Core;

namespace ETS.Transaction.Common
{
	/// <summary>
	/// AdoNet based implementation of the <see cref="IUnitOfWorkFactory"/> interface.
	/// </summary>
	public class AdoNetUnitOfWorkFactory : IUnitOfWorkFactory
	{
		private IDbProvider dbProvider;

		private AdoNetUnitOfWorkFactory()
		{
		}

		#region IUnitOfWorkFactory Members

		public IUnitOfWork GetUnitOfWork(IUnitOfWorkDefinition definition)
		{
			return new AdoNetUnitOfWork(dbProvider, definition);
		}

		#endregion

		/// <summary>
		/// Create the <see cref="TransactionScopeUnitOfWorkFactory"/> by the private constructor.
		/// </summary>
		/// <returns></returns>
		public static AdoNetUnitOfWorkFactory Create()
		{
			return new AdoNetUnitOfWorkFactory();
		}

		/// <summary>
		/// …Ë÷√DbProvider
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public AdoNetUnitOfWorkFactory SetDbProvider(IDbProvider provider)
		{
			dbProvider = provider;
			return this;
		}
	}
}