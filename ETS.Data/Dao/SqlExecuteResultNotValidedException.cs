#region Imports

using System;
using System.Runtime.Serialization;

#endregion

namespace ETS.Dao
{
	/// <summary>
	/// SQLִ�н������ȷ
	/// </summary>
	[Serializable]
	public class SqlExecuteResultNotValidedException : DataAccessException
	{
		/// <summary>
		/// Creates a new instance of the
		/// <see cref="SqlExecuteResultNotValidedException"/> class.
		/// </summary>
		public SqlExecuteResultNotValidedException()
		{
		}

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="SqlExecuteResultNotValidedException"/> class.
		/// </summary>
		/// <param name="message">
		/// A message about the exception.
		/// </param>
		public SqlExecuteResultNotValidedException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="SqlExecuteResultNotValidedException"/> class.
		/// </summary>
		/// <param name="message">
		/// A message about the exception.
		/// </param>
		/// <param name="rootCause">
		/// The root exception (from the underlying data access API, such as ADO.NET).
		/// </param>
		public SqlExecuteResultNotValidedException(string message, Exception rootCause)
			: base(message, rootCause)
		{
		}

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="SqlExecuteResultNotValidedException"/> class.
		/// </summary>
		/// <param name="info">
		/// The <see cref="System.Runtime.Serialization.SerializationInfo"/>
		/// that holds the serialized object data about the exception being thrown.
		/// </param>
		/// <param name="context">
		/// The <see cref="System.Runtime.Serialization.StreamingContext"/>
		/// that contains contextual information about the source or destination.
		/// </param>
		protected SqlExecuteResultNotValidedException(
			SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}