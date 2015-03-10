using System.Data;

namespace ETS.Data.Generic
{
	/// <summary>
	/// Generic callback to process a row of data in a result set to an object.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IDataTableRowMapper<T>
	{
		/// <summary>
		/// Implementations must implement this method to map a row of data in the
		/// result set (DataRow). 
		/// </summary>
		/// <param name="dataRow">The DataRow to map </param>
		/// <returns>The specific typed result object for the current row.</returns>
		T MapRow(DataRow dataRow);
	}

	/// <summary>
	/// Callback delegate to process each row of data in a result set to an object.
	/// </summary>
	/// <typeparam name="T">The type of the object returned from the mapping operation.</typeparam>
	/// <param name="dataRow">The DataRow to map</param>
	/// <returns>An abrirary object, typically derived from data
	/// in the result set.</returns>
	public delegate T DataTableRowMapperDelegate<T>(DataRow dataRow);
}