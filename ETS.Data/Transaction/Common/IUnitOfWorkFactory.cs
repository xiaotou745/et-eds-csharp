namespace ETS.Transaction.Common
{
	/// <summary>
	/// Create a unit of work based on a transaction definition.
	/// </summary>
	public interface IUnitOfWorkFactory
	{
		/// <summary>
		/// 根据<see cref="IUnitOfWorkDefinition"/>获取<see cref="IUnitOfWork"/>实例.
		/// </summary>
		/// <param name="definition">定义事务环境的<see cref="IUnitOfWorkDefinition"/>实例</param>
		/// <returns>A UnitOfWork.</returns>
		IUnitOfWork GetUnitOfWork(IUnitOfWorkDefinition definition);
	}
}