using System.Data;

namespace ETS.Transaction.Common
{
	/// <summary>
	/// UnitOfWork Definition
	/// </summary>
	public class UnitOfWorkDefinition
	{
		/// <summary>
		/// Default Transaction Definition
		/// <table>
		/// <th><td>Property</td><td>Value</td></th>
		/// <tr><td>IsolationLevel</td><see cref="System.Data.IsolationLevel.ReadCommitted"/><td></td></tr>
		/// <tr><td>Timeout</td>60<td></td></tr>
		/// <tr><td>ReadOnly</td><see langword="false"></see><td></td></tr>
		/// <tr><td>Name</td><see langword="null"></see><td></td></tr>
		/// <tr><td>Exclude</td><see langword="false"></see><td></td></tr>
		/// </table>
		/// </summary>
		public static IUnitOfWorkDefinition DefaultDefinition
		{
			get { return new DefaultUnitOfWorkDefinition(); }
		}

		/// <summary>
		/// Excluded Transaction Definition
		/// <table>
		/// <th><td>Property</td><td>Value</td></th>
		/// <tr><td>IsolationLevel</td><see cref="System.Data.IsolationLevel.ReadCommitted"/><td></td></tr>
		/// <tr><td>Timeout</td>60<td></td></tr>
		/// <tr><td>ReadOnly</td><see langword="false"></see><td></td></tr>
		/// <tr><td>Name</td><see langword="null"></see><td></td></tr>
		/// <tr><td>Exclude</td><see langword="true"></see><td></td></tr>
		/// </table>
		/// </summary>
		public static IUnitOfWorkDefinition ExcludedDefinition
		{
			get { return new ExcludedUnitOfWorkDefinition(); }
		}

		/// <summary>
		/// Excluded Transaction Definition
		/// <table>
		/// <th><td>Property</td><td>Value</td></th>
		/// <tr><td>IsolationLevel</td><see cref="System.Data.IsolationLevel.ReadUncommitted"/><td></td></tr>
		/// <tr><td>Timeout</td>60<td></td></tr>
		/// <tr><td>ReadOnly</td><see langword="true"></see><td></td></tr>
		/// <tr><td>Name</td><see langword="null"></see><td></td></tr>
		/// <tr><td>Exclude</td><see langword="false"></see><td></td></tr>
		/// </table>
		/// </summary>
		public static IUnitOfWorkDefinition ReadOnlyDefinition
		{
			get { return new ReadOnlyAttribute(); }
		}

		#region Nested type: ExcludedUnitOfWorkDefinition

		public class ExcludedUnitOfWorkDefinition : DefaultUnitOfWorkDefinition
		{
			public ExcludedUnitOfWorkDefinition()
			{
				Exclude = true;
			}
		}

		#endregion

		#region Nested type: ReadOnlyAttribute

		public class ReadOnlyAttribute : DefaultUnitOfWorkDefinition
		{
			public ReadOnlyAttribute()
			{
				ReadOnly = true;
				IsolationLevel = IsolationLevel.ReadUncommitted;
			}
		}

		#endregion

		
	}
}