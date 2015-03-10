using System;
using System.Collections.Generic;
using System.Data;
using ETS.Util;

namespace ETS.Data.Generic
{
	/// <summary>
	/// Adapter implementation of the ResultSetExtractor interface that delegates 
	/// to a RowMapper which is supposed to create an object for each row.
	/// Each object is added to the results List of this ResultSetExtractor.
	/// </summary>
	/// <remarks>
	/// Useful for the typical case of one object per row in the database table.
	/// The number of entries in the results list will match the number of rows.
	/// <p>
	/// Note that a RowMapper object is typically stateless and thus reusable;
	/// just the RowMapperResultSetExtractor adapter is stateful.
	/// </p>
	/// <p>
	/// As an alternative consider subclassing MappingAdoQuery from the 
	/// Spring.Data.Objects namespace:  Instead of working with separate 
	/// AdoTemplate and IRowMapper objects you can have executable
	/// query objects (containing row-mapping logic) there.
	/// </p>
	/// </remarks>
	/// <author>Mark Pollack (.NET)</author>
	public class RowMapperResultSetExtractor<T> : IResultSetExtractor<IList<T>>
	{
		#region Fields

		private readonly IDataTableRowMapper<T> rowMapper;

		private readonly DataTableRowMapperDelegate<T> rowMapperDelegate;

		#endregion

		#region Constructor (s)

		public RowMapperResultSetExtractor(IDataTableRowMapper<T> rowMapper)
		{
			AssertUtils.ArgumentNotNull(rowMapper, "rowMapper");

			this.rowMapper = rowMapper;
		}


		public RowMapperResultSetExtractor(DataTableRowMapperDelegate<T> rowMapperDelegate)
		{
			AssertUtils.ArgumentNotNull(rowMapperDelegate, "rowMapperDelegate");

			this.rowMapperDelegate = rowMapperDelegate;
		}

		#endregion

		#region IResultSetExtractor<IList<T>> Members

		public IList<T> ExtractData(DataSet dataSet, int tableIndex)
		{
			IList<T> results = new List<T>();

			if (dataSet == null || dataSet.Tables.Count == 0)
			{
				return results;
			}

			if (tableIndex + 1 > dataSet.Tables.Count)
			{
				throw new ArgumentException(
					string.Format("datatable index ³¬³öË÷Òý·¶Î§, [tableIndex:{0}] [dataset'table count:{1}]"
					              , tableIndex,
					              dataSet.Tables.Count), "tableIndex");
			}

			if (rowMapper != null)
			{
				foreach (DataRow dataRow in dataSet.Tables[tableIndex].Rows)
				{
					results.Add(rowMapper.MapRow(dataRow));
				}
			}
			else
			{
				foreach (DataRow dataRow in dataSet.Tables[tableIndex].Rows)
				{
					results.Add(rowMapperDelegate(dataRow));
				}
			}

			return results;
		}

		#endregion
	}
}