using System;
using System.Transactions;

namespace ETS.Transaction.Common
{
	public interface ITransactionScopeFactory
	{
		TransactionScope GetInstance();
	}

	public class TransactionScopeFactoryOfDefault : ITransactionScopeFactory
	{
		#region ITransactionScopeFactory Members

		public TransactionScope GetInstance()
		{
			IUnitOfWorkDefinition definition = UnitOfWorkDefinition.DefaultDefinition;

			return new TransactionScopeFactory(definition).GetInstance();
		}

		#endregion
	}

	public class TransactionScopeFactory : ITransactionScopeFactory
	{
		private IUnitOfWorkDefinition unitOfWorkDefinition;

		public TransactionScopeFactory(IUnitOfWorkDefinition unitOfWorkDefinition)
		{
			this.unitOfWorkDefinition = unitOfWorkDefinition;
		}

		#region ITransactionScopeFactory Members

		public TransactionScope GetInstance()
		{
			if (unitOfWorkDefinition == null)
			{
				unitOfWorkDefinition = UnitOfWorkDefinition.DefaultDefinition;
			}

			var transactionToUse = new TransactionOptions
			                       	{
			                       		IsolationLevel = IsolationConvert.Convert(unitOfWorkDefinition.IsolationLevel),
			                       		Timeout = new TimeSpan(0, 0, unitOfWorkDefinition.Timeout)
			                       	};

			return new TransactionScope(TransactionScopeOption.Required, transactionToUse);
		}

		#endregion

		#region Nested type: IsolationConvert

		private static class IsolationConvert
		{
			public static IsolationLevel Convert(System.Data.IsolationLevel isolationLevel)
			{
				switch (isolationLevel)
				{
					case System.Data.IsolationLevel.Serializable:
						return IsolationLevel.Serializable;
					case System.Data.IsolationLevel.RepeatableRead:
						return IsolationLevel.RepeatableRead;
					case System.Data.IsolationLevel.ReadCommitted:
						return IsolationLevel.ReadCommitted;
					case System.Data.IsolationLevel.ReadUncommitted:
						return IsolationLevel.ReadUncommitted;
					case System.Data.IsolationLevel.Snapshot:
						return IsolationLevel.Snapshot;
					case System.Data.IsolationLevel.Chaos:
						return IsolationLevel.Chaos;
					case System.Data.IsolationLevel.Unspecified:
						return IsolationLevel.Unspecified;
					default:
						return IsolationLevel.ReadCommitted;
				}
			}
		}

		#endregion
	}
}