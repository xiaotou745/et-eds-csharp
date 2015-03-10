namespace ETS.Transaction.Common
{
	/// <summary>
	/// Create a unit of work based on a transaction definition.
	/// </summary>
	public interface IUnitOfWorkFactory
	{
		/// <summary>
		/// ����<see cref="IUnitOfWorkDefinition"/>��ȡ<see cref="IUnitOfWork"/>ʵ��.
		/// </summary>
		/// <param name="definition">�������񻷾���<see cref="IUnitOfWorkDefinition"/>ʵ��</param>
		/// <returns>A UnitOfWork.</returns>
		IUnitOfWork GetUnitOfWork(IUnitOfWorkDefinition definition);
	}
}