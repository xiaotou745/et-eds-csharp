using System;
using System.Collections;
using System.Data;
using Common.Logging;

namespace ETS.Data.Core
{
	/// <summary>
	/// Miscellaneous utility methods for manipulating parameter objects. 
	/// </summary>
	public class ParameterUtils
	{
		#region Fields

		#endregion

		#region Logger Definition

		private static readonly ILog logger = LogManager.GetCurrentClassLogger();
		#endregion

		#region Constants

		/// <summary>
		/// The shared log instance for this class (and derived classes). 
		/// </summary>
		protected static readonly ILog log =
			LogManager.GetLogger(typeof (ParameterUtils));

		#endregion

		#region Constructor (s)

		#endregion

		#region Properties

		#endregion

		#region Methods

		/// <summary>
		/// Copies the parameters from IDbParameters to the parameter collection in IDbCommand
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="parameterCollection">The param collection.</param>
		public static void CopyParameters(IDbCommand command, IDbParameters parameterCollection)
		{
			if (parameterCollection != null)
			{
				IDataParameterCollection collection = parameterCollection.DataParameterCollection;

				foreach (IDbDataParameter parameter in collection)
				{
					var pClone = (IDbDataParameter) ((ICloneable) parameter).Clone();
					command.Parameters.Add(pClone);
				}
			}
		}

		public static IDataParameter[] CloneParameters(IDbCommand command)
		{
			var returnParameters = new IDataParameter[command.Parameters.Count];
			for (int i = 0; i < command.Parameters.Count; i++)
			{
				var pClone = (IDbDataParameter) ((ICloneable) command.Parameters[i]).Clone();
				returnParameters[i] = pClone;
			}
			return returnParameters;
		}

		public static IDataParameter CloneParameter(IDataParameter parameter)
		{
			return (IDbDataParameter)((ICloneable)parameter).Clone();
		}
		/// <summary>
		/// Copies the parameters in IDbCommand to IDbParameters
		/// </summary>
		/// <param name="paramCollection">The spring param collection.</param>
		/// <param name="command">The command.</param>
		public static void CopyParameters(IDbParameters paramCollection,IDbCommand command)
		{
			if (paramCollection != null)
			{
				IDataParameterCollection cmdParameterCollection = command.Parameters;
				//TODO investigate copying only the values over, not a full clone.
				int count = 0;
				foreach (IDbDataParameter dbDataParameter in cmdParameterCollection)
				{
					paramCollection[count] = (IDbDataParameter) ((ICloneable) dbDataParameter).Clone();
					count++;
				}
			}
		}

		#endregion

		public static void ExtractOutputParameters(IDictionary returnedParameters, IDbCommand command)
		{
			if (returnedParameters == null)
			{
				return;
			}
			IDataParameterCollection paramCollection = command.Parameters;
			int count = 0;
			foreach (IDbDataParameter dbDataParameter in paramCollection)
			{
				if (dbDataParameter.Direction == ParameterDirection.Output
				    || dbDataParameter.Direction == ParameterDirection.InputOutput)
				{
					if (dbDataParameter.ParameterName == null)
					{
						returnedParameters.Add("P" + count, dbDataParameter.Value);
					}
					else
					{
						returnedParameters.Add(dbDataParameter.ParameterName, dbDataParameter.Value);
					}
				}
				if (dbDataParameter.Direction == ParameterDirection.ReturnValue)
				{
					returnedParameters.Add("RETURN_VALUE", dbDataParameter.Value);
				}
				count++;
			}
			/*
	        for (int i = 0; i < declaredParameters.Count; i++)
	        {
	            IDataParameter declaredParameter = declaredParameters[i];
	            if (declaredParameter.Direction == ParameterDirection.Output
	                || declaredParameter.Direction == ParameterDirection.InputOutput)
	            {
	                IDataParameter outParameter = (IDataParameter)command.Parameters[i];
	                if (declaredParameter.ParameterName == null)
	                {
	                    returnedParameters.Add("P" + i, outParameter.Value);
	                }
	                else
	                {
	                    returnedParameters.Add(outParameter.ParameterName, outParameter.Value);
	                }
	            }
	        }*/
		}
	}
}