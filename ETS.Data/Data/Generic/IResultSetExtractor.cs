using System.Data;

namespace ETS.Data.Generic
{
	/// <summary>
	/// Callback interface to process all results sets and rows in
	/// an AdoTemplate query method.
	/// </summary>
	/// <remarks>Implementations of this interface perform the work
	/// of extracting results but don't need worry about managing
	/// ADO.NET resources, such as closing the reader. 
	/// <p>
	/// This interface is mainly used within the ADO.NET
	/// framework.  An IResultSetExtractor is usually a simpler choice
	/// for result set (DataReader) processing, in particular a 
	/// RowMapperResultSetExtractor in combination with a IRowMapper.
	/// </p>
	/// <para>Note: in contracts to a IRowCallbackHandler, a ResultSetExtractor
	/// is usually stateless and thus reusable, as long as it doesn't access
	/// stateful resources or keep result state within the object.
	/// </para>
	/// </remarks>
	public interface IResultSetExtractor<T>
	{
		/// <summary>
		/// Implementations must implement this method to process all
		/// result set and rows in the IDataReader.
		/// </summary>
		/// <param name="dataSet">The DataTable to extract data from.</param>
		/// <param name="tableIndex">the table index of the dataset. </param>
		/// <returns>An arbitrary result object or null if none.  The
		/// extractor will typically be stateful in the latter case.</returns>
		T ExtractData(DataSet dataSet, int tableIndex);
	}
}