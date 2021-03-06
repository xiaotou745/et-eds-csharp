using System;
using System.Collections;
using System.Threading;
using Common.Logging;
using ETS.Threading;

namespace ETS.Data.Core
{
	/// <summary>
	/// Internal class that manages resources and transaction synchronizations[ˌsiŋkrənaiˈzeiʃən]同步 per thread.
	/// </summary>
	/// <remarks>
	/// Supports one resource per key without overwriting, i.e. a resource needs to
	/// be removed before a new one can be set for the same key.
	/// Supports a list of transaction synchronizations if synchronization is active.
	/// <p>
	/// Resource management code should check for thread-bound resources via GetResource().
	/// It is normally not supposed
	/// to bind resources to threads, as this is the responsiblity of transaction managers.
	/// A further option is to lazily bind on first use if transaction synchronization
	/// is active, for performing transactions that span an arbitrary number of resources.
	/// </p>
	/// <p>
	/// Transaction synchronization must be activated and deactivated by a transaction
	/// manager via
	/// </p>
	/// <p>
	/// Resource management code should only register synchronizations when this
	/// manager is active, and perform resource cleanup immediately else.
	/// If transaction synchronization isn't active, there is either no current
	/// transaction, or the transaction manager doesn't support synchronizations.
	/// </p>
	/// Note that this class uses following naming convention for the 
	/// named 'data slots' for storage of thread local data, 'Spring.Transaction:Name'
	/// where Name is either 
	/// </remarks>
	public sealed class TransactionSynchronizationManager
	{
		#region Logging

		private static readonly ILog log = DbLogManager.GetConnectionStringLog();

		#endregion

		#region Fields 

		private const string RESOURCES_DATA_SLOT_NAME = "System.Data.IDbConnection:resources";

		#endregion

		#region Management of transaction-associated resource handles

		/// <summary>
		/// Return all resources that are bound to the current thread.
		/// </summary>
		/// <remarks>Main for debugging purposes.  Resource manager should always
		/// invoke HasResource for a specific resource key that they are interested in.
		/// </remarks>
		/// <returns>IDictionary with resource keys and resource objects or empty 
		/// dictionary if none is bound.</returns>
		public static IDictionary ResourceDictionary
		{
			get
			{
				var resources = LogicalThreadContext.GetData(RESOURCES_DATA_SLOT_NAME) as IDictionary;
				if (resources != null)
				{
					return resources;
				}
				else
				{
					return new Hashtable();
				}
			}
		}

		/// <summary>
		/// Check if there is a resource for the given key bound to the current thread.
		/// </summary>
		/// <param name="key">key to check</param>
		/// <returns>if there is a value bound to the current thread</returns>
		public static bool HasResource(Object key)
		{
			return ResourceDictionary.Contains(key);
		}

		/// <summary>
		/// Retrieve a resource for the given key that is bound to the current thread.
		/// </summary>
		/// <param name="key">key to check</param>
		/// <returns>a value bound to the current thread, or null if none.</returns>
		public static object GetResource(Object key)
		{
			var resources = LogicalThreadContext.GetData(RESOURCES_DATA_SLOT_NAME) as IDictionary;
			if (resources == null)
			{
				return null;
			}
			//Check for contains since indexer returning null behavior changes in 2.0
			if (!resources.Contains(key))
			{
				return null;
			}
			object val = resources[key];

			if (val != null && log.IsDebugEnabled)
			{
				log.Debug("get value [" + Describe(val) + "] for key [" + Describe(key) + "] bound to thread [" +
				          Thread.CurrentThread.ManagedThreadId + "]");
			}
			return val;
		}

		/// <summary>
		/// Bind the given resource for teh given key to the current thread
		/// </summary>
		/// <param name="key">key to bind the value to</param>
		/// <param name="value">value to bind</param>
		public static void BindResource(Object key, Object value)
		{
			var resources = LogicalThreadContext.GetData(RESOURCES_DATA_SLOT_NAME) as IDictionary;
			//Set thread local resource storage if not found
			if (resources == null)
			{
				resources = new Hashtable();
				LogicalThreadContext.SetData(RESOURCES_DATA_SLOT_NAME, resources);
			}
			if (resources.Contains(key))
			{
				throw new InvalidOperationException("Already value [" + resources[key] + "] for key [" + key +
				                                    "] bound to thread [" + Thread.CurrentThread.ManagedThreadId + "]");
			}
			resources.Add(key, value);
			if (log.IsDebugEnabled)
			{
				log.Debug("Bound value [" + Describe(value) + "] for key [" + Describe(key) + "] to thread [" +
				          Thread.CurrentThread.ManagedThreadId + "]");
			}
		}


		/// <summary>
		/// Unbind a resource for the given key from the current thread
		/// </summary>
		/// <param name="key">key to check</param>
		/// <returns>the previously bound value</returns>
		/// <exception cref="InvalidOperationException">if there is no value bound to the thread</exception>
		public static object UnbindResource(Object key)
		{
			var resources = LogicalThreadContext.GetData(RESOURCES_DATA_SLOT_NAME) as IDictionary;
			if (resources == null || !resources.Contains(key))
			{
				throw new InvalidOperationException("No value for key [" + key + "] bound to thread [" +
				                                    Thread.CurrentThread.ManagedThreadId + "]");
			}
			Object val = resources[key];
			resources.Remove(key);
			if (resources.Count == 0)
			{
				LogicalThreadContext.FreeNamedDataSlot(RESOURCES_DATA_SLOT_NAME);
			}
			if (log.IsDebugEnabled)
			{
				log.Debug("Removed value [" + Describe(val) + "] for key [" + Describe(key) + "] from thread [" +
				          Thread.CurrentThread.ManagedThreadId + "]");
			}
			return val;
		}

		#endregion

		private static string Describe(object obj)
		{
			return obj == null ? "" : obj + "@" + obj.GetHashCode().ToString("X");
		}
	}
}