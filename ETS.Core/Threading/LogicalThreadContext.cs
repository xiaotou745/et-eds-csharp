using System;
using System.Runtime.Remoting.Messaging;

namespace ETS.Threading
{
	/// <summary>
	/// An abstraction to safely store "ThreadStatic" data.
	/// </summary>
	/// <remarks>
	/// By default, <see cref="CallContext"/> is used to store thread-specific data. 
	/// You may switch the storage strategy by calling <see cref="SetStorage(IThreadStorage)"/>.<p/>
	/// <b>NOTE:</b> Access to the underlying storage is not synchronized for performance reasons. 
	/// You should call <see cref="SetStorage(IThreadStorage)"/> only once at application startup!
	/// </remarks>
	/// <author>Erich Eichinger</author>
	public sealed class LogicalThreadContext
	{
		/// <summary>
		/// Holds the current <see cref="IThreadStorage"/> strategy.
		/// </summary>
		/// <remarks>
		/// Access to this variable is not synchronized on purpose for performance reasons. 
		/// Setting a different <see cref="IThreadStorage"/> strategy should happen only once
		/// at application startup.
		/// </remarks>
		private static IThreadStorage threadStorage = new CallContextStorage();

		private LogicalThreadContext()
		{
			throw new NotSupportedException("must not be instantiated");
		}

		/// <summary>
		/// Set the new <see cref="IThreadStorage"/> strategy.
		/// </summary>
		public static void SetStorage(IThreadStorage storage)
		{
			threadStorage = storage;
		}

		/// <summary>
		/// Retrieves an object with the specified name.
		/// </summary>
		/// <param name="name">The name of the item.</param>
		/// <returns>The object in the context associated with the specified name or null if no object has been stored previously</returns>
		public static object GetData(string name)
		{
			return threadStorage.GetData(name);
		}

		/// <summary>
		/// Stores a given object and associates it with the specified name.
		/// </summary>
		/// <param name="name">The name with which to associate the new item.</param>
		/// <param name="value">The object to store in the current thread's context.</param>
		public static void SetData(string name, object value)
		{
			threadStorage.SetData(name, value);
		}

		/// <summary>
		/// Empties a data slot with the specified name.
		/// </summary>
		/// <param name="name">The name of the data slot to empty.</param>
		public static void FreeNamedDataSlot(string name)
		{
			threadStorage.FreeNamedDataSlot(name);
		}
	}
}