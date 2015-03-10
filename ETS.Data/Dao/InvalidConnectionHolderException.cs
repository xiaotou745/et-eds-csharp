using System;
using System.Runtime.Serialization;

namespace ETS.Dao
{
	/// <summary> 
	/// Exception thrown on the connectionHolder in the current thread is <see langword="null"/>
	/// Or the connection of the connHodler in the current thread is null
	/// Or the transaction of the connHolder in the current thread is null
	/// </summary>
	/// <remarks>
	/// <p>
	/// This exception will be thrown either by O/R mapping tools or by custom DAO
	/// implementations.
	/// </p>
	/// </remarks>
	public class InvalidConnectionHolderException : DataAccessException
	{
		/// <summary>
		/// Creates a new instance of the
		/// <see cref="InvalidConnectionHolderException"/> class.
		/// </summary>
		public InvalidConnectionHolderException()
		{
		}

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="InvalidConnectionHolderException"/> class.
		/// </summary>
		/// <param name="message">
		/// A message about the exception.
		/// </param>
		public InvalidConnectionHolderException(string message) : base(message)
		{
		}

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="InvalidConnectionHolderException"/> class.
		/// </summary>
		/// <param name="message">
		/// A message about the exception.
		/// </param>
		/// <param name="rootCause">
		/// The root exception (from the underlying data access API, such as ADO.NET).
		/// </param>
		public InvalidConnectionHolderException(string message, Exception rootCause)
			: base(message, rootCause)
		{
		}

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="InvalidConnectionHolderException"/> class.
		/// </summary>
		/// <param name="info">
		/// The <see cref="System.Runtime.Serialization.SerializationInfo"/>
		/// that holds the serialized object data about the exception being thrown.
		/// </param>
		/// <param name="context">
		/// The <see cref="System.Runtime.Serialization.StreamingContext"/>
		/// that contains contextual information about the source or destination.
		/// </param>
		protected InvalidConnectionHolderException(
			SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}