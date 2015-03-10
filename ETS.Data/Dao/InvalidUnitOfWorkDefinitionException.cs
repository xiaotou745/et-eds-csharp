using System;
using System.Runtime.Serialization;

namespace ETS.Dao
{
	/// <summary>
	/// Exception thrown on incorrect definition of the unit of work, such as invalid timeout value
	/// </summary>
	public class InvalidUnitOfWorkDefinitionException : DataAccessException
	{
		 /// <summary>
		/// Creates a new instance of the
		/// <see cref="DbProviderException"/> class.
		/// </summary>
		public InvalidUnitOfWorkDefinitionException()
		{
		}

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="DbProviderException"/> class.
		/// </summary>
		/// <param name="message">
		/// A message about the exception.
		/// </param>
		public InvalidUnitOfWorkDefinitionException(string message) : base(message)
		{
		}

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="DbProviderException"/> class.
		/// </summary>
		/// <param name="message">
		/// A message about the exception.
		/// </param>
		/// <param name="rootCause">
		/// The root exception (from the underlying data access API, such as ADO.NET).
		/// </param>
		public InvalidUnitOfWorkDefinitionException(string message, Exception rootCause)
			: base(message, rootCause)
		{
		}

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="DbProviderException"/> class.
		/// </summary>
		/// <param name="info">
		/// The <see cref="System.Runtime.Serialization.SerializationInfo"/>
		/// that holds the serialized object data about the exception being thrown.
		/// </param>
		/// <param name="context">
		/// The <see cref="System.Runtime.Serialization.StreamingContext"/>
		/// that contains contextual information about the source or destination.
		/// </param>
		protected InvalidUnitOfWorkDefinitionException(
			SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}