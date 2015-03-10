using System;
using System.Runtime.Serialization;

namespace ETS.Dao
{
	/// <summary>
	/// dataaccess exception thrown when the current dbprovider was null
	/// or the connectionString of the current dbprovider is null or empty.
	/// </summary>
	public class InValidUnitOfWorkException : DataAccessException
	{
		/// <summary>
		/// Creates a new instance of the
		/// <see cref="InValidUnitOfWorkException"/> class.
		/// </summary>
		public InValidUnitOfWorkException()
		{
		}

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="InValidUnitOfWorkException"/> class.
		/// </summary>
		/// <param name="message">
		/// A message about the exception.
		/// </param>
		public InValidUnitOfWorkException(string message) : base(message)
		{
		}

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="InValidUnitOfWorkException"/> class.
		/// </summary>
		/// <param name="message">
		/// A message about the exception.
		/// </param>
		/// <param name="rootCause">
		/// The root exception (from the underlying data access API, such as ADO.NET).
		/// </param>
		public InValidUnitOfWorkException(string message, Exception rootCause)
			: base(message, rootCause)
		{
		}

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="InValidUnitOfWorkException"/> class.
		/// </summary>
		/// <param name="info">
		/// The <see cref="System.Runtime.Serialization.SerializationInfo"/>
		/// that holds the serialized object data about the exception being thrown.
		/// </param>
		/// <param name="context">
		/// The <see cref="System.Runtime.Serialization.StreamingContext"/>
		/// that contains contextual information about the source or destination.
		/// </param>
		protected InValidUnitOfWorkException(
			SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}