using System;
using System.Transactions;

namespace ETS.Transaction.Common
{
	/// <summary>
	/// IUnitOfWork内部可以使用SqlTransaction，但是如果Rollback或者没有Commit会造成
	/// IUnitOfWork的<see cref="Complete"/>和<see cref="IDisposable.Dispose"/>抛出
	/// <see cref="TransactionAbortedException"/>。
	/// </summary>
	public interface IUnitOfWork : IDisposable
	{
		void Complete();
	}
}