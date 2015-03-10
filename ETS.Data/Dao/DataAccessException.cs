#region Imports

using System;
using System.Runtime.Serialization;

#endregion

namespace ETS.Dao
{
	/// <summary>
	/// Root of the hierarchy of data access exceptions
	/// </summary>
	[Serializable]
	public abstract class DataAccessException : Exception
	{
		/// <summary>
		/// Creates a new instance of the
		/// <see cref="DataAccessException"/> class.
		/// </summary>
		protected DataAccessException()
		{
		}

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="DataAccessException"/> class.
		/// </summary>
		/// <param name="message">
		/// A message about the exception.
		/// </param>
		protected DataAccessException(string message) : base(message)
		{
		}

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="DataAccessException"/> class.
		/// </summary>
		/// <param name="message">
		/// A message about the exception.
		/// </param>
		/// <param name="rootCause">
		/// The root exception (from the underlying data access API, such as ADO.NET).
		/// </param>
		protected DataAccessException(string message, Exception rootCause)
			: base(message, rootCause)
		{
		}

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="DataAccessException"/> class.
		/// </summary>
		/// <param name="info">
		/// The <see cref="System.Runtime.Serialization.SerializationInfo"/>
		/// that holds the serialized object data about the exception being thrown.
		/// </param>
		/// <param name="context">
		/// The <see cref="System.Runtime.Serialization.StreamingContext"/>
		/// that contains contextual information about the source or destination.
		/// </param>
		protected DataAccessException(
			SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}