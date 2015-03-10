using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ETS.Dao;

namespace ETS.Data.Core
{
    /// <summary>
    /// A more portable means to create a collection of ADO.NET
    /// parameters.
    /// </summary>
    public class DbParameters : IDbParameters
    {

        #region Fields

        private readonly IDbProvider dbProvider;

        //Just used as a container for the underlying parameter collection.	    
        private readonly IDbCommand dbCommand;
        private readonly IDataParameterCollection dataParameterCollection;

        #endregion

        #region Constructor (s)

        /// <summary>
        /// Initializes a new instance of the <see cref="DbParameters"/> class.
        /// </summary>
        public DbParameters(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
            dbCommand = dbProvider.CreateCommand();
            dataParameterCollection = dbCommand.Parameters;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #endregion

        public IDataParameter this[string parameterName]
        {
            get { return (IDbDataParameter) dataParameterCollection[parameterName]; }
            set { dataParameterCollection[parameterName] = value; }
        }

        public IDataParameter this[int index]
        {
            get { return (IDbDataParameter) dataParameterCollection[index]; }
            set { dataParameterCollection[index] = value; }
        }

        public IDataParameterCollection DataParameterCollection
        {
            get { return dataParameterCollection; }
        }
        
        public int Count
        {
            get
            {
                return dataParameterCollection.Count;
            }
        }
        public bool Contains(string parameterName)
        {
            return dataParameterCollection.Contains(parameterName);
        }

        public void AddParameter(IDataParameter dbParameter)
        {
            dataParameterCollection.Add(dbParameter);
        }
        

        public IDbDataParameter AddParameter(string name, 
                                 Enum parameterType, 
                                 int size, 
                                 ParameterDirection direction,
                                 bool isNullable,
                                 byte precision, 
                                 byte scale, 
                                 string sourceColumn, 
                                 DataRowVersion sourceVersion,
                                 object parameterValue)
        {
			
            IDbDataParameter parameter = dbCommand.CreateParameter();

			parameter.ParameterName = dbProvider.CreateParameterNameForCollection(name);

            AssignParameterType(parameter, parameterType);

            if (size > 0)
            {
                parameter.Size = size;
            }
            parameter.Direction = direction;
			if (isNullable)
			{
				if (parameter.Value == null)
				{
					parameter.Value = DBNull.Value;
				}
			}
            parameter.Precision = precision;
			
            if (scale > 0)
            {
                parameter.Scale = scale;
            }

            if (!string.IsNullOrEmpty(sourceColumn))
            {
                parameter.SourceColumn = sourceColumn;
            }

            parameter.SourceVersion = sourceVersion;

            parameter.Value = parameterValue ?? DBNull.Value;

            dataParameterCollection.Add(parameter);
            
            return parameter;
        }

        public IDbDataParameter AddParameter(string name, 
                                             Enum parameterType, 
                                             ParameterDirection direction, 
                                             bool isNullable,
                                             byte precision,
                                             byte scale, 
                                             string sourceColumn, 
                                             DataRowVersion sourceVersion, 
                                             object parameterValue)
        {
            return AddParameter(name, parameterType, 0, direction, isNullable, precision, scale, sourceColumn, sourceVersion,
                         parameterValue);
        }

        public int Add(object parameterValue)
        {
            IDbDataParameter parameter = dbCommand.CreateParameter();
            parameter.Value = parameterValue ?? DBNull.Value;
            dataParameterCollection.Add(parameter);
            return dataParameterCollection.Count - 1;
        }

        public void AddRange(Array values)
        {
            foreach (Array value in values)
            {
                Add(value);
            }
        }

        public IDbDataParameter AddWithValue(string name, object parameterValue)
        {
            return AddParameter(name, null, -1, ParameterDirection.Input, false, 
                                0, 0, null, DataRowVersion.Default, parameterValue);
        }

        public IDbDataParameter Add(string name, Enum parameterType)
        {
            return AddParameter(name, parameterType, -1, ParameterDirection.Input, 
                false, 0, 0, null, DataRowVersion.Default, null);
        }
        
        public IDbDataParameter Add(string name, Enum parameterType, int size)
        {
            return AddParameter(name, parameterType, size, ParameterDirection.Input, 
                false, 0, 0, null, DataRowVersion.Default, null);
        }
        
        public IDbDataParameter Add(string name, Enum parameterType, int size, string sourceColumn)
        {
            return AddParameter(name, parameterType, size, ParameterDirection.Input, 
                false, 0, 0, sourceColumn, DataRowVersion.Default, null);
        }

        public IDbDataParameter AddOut(string name, Enum parameterType)
        {
            return AddParameter(name, parameterType, -1, ParameterDirection.Output, 
                false, 0, 0, null, DataRowVersion.Default, null);
        }

        public IDbDataParameter AddOut(string name, Enum parameterType, int size)
        {
            return AddParameter(name, parameterType, size, ParameterDirection.Output,
                false, 0, 0, null, DataRowVersion.Default, null);
        }
        
        public IDbDataParameter AddInOut(string name, Enum parameterType)
        {
            return AddParameter(name, parameterType, -1, ParameterDirection.InputOutput,
                false, 0, 0, null, DataRowVersion.Default, null);
        }
        
        public IDbDataParameter AddInOut(string name, Enum parameterType, int size)
        {
            return AddParameter(name, parameterType, size, ParameterDirection.InputOutput,
                false, 0, 0, null, DataRowVersion.Default, null);
        }
        
        public IDbDataParameter AddReturn(string name, Enum parameterType)
        {
            return AddParameter(name, parameterType, -1, ParameterDirection.ReturnValue,
                false, 0, 0, null, DataRowVersion.Default, null);
        }
        
        public IDbDataParameter AddReturn(string name, Enum parameterType, int size)
        {
            return AddParameter(name, parameterType, size, ParameterDirection.ReturnValue,
                false, 0, 0, null, DataRowVersion.Default, null);
        }


        public object GetValue(string name)
        {
            IDataParameter parameter = dataParameterCollection[dbProvider.CreateParameterNameForCollection(name)] as IDataParameter;
            if (parameter != null)
            {
                return parameter.Value;
            }
			throw new DbProviderException(
                "object in IDataParameterCollection is not of the type IDataParameter, it is type [" +
                dataParameterCollection[dbProvider.CreateParameterNameForCollection(name)].GetType() + "].");
        }

        public void SetValue(string name, object parameterValue)
        {
            IDbDataParameter parameter = dataParameterCollection[dbProvider.CreateParameterNameForCollection(name)] as IDbDataParameter;
            if (parameter != null)
            {
                parameter.Value = parameterValue ?? DBNull.Value;
            }
        }

        public void AddInParameters<T>(IEnumerable<T> source, Enum parameterType, int size, out string parameterNames)
        {
            var paramNames = new StringBuilder();
            if (source != null)
            {
                int index = 1;
                foreach (T item in source)
                {
                    string paramName = "@param_name_" + index;
                    Add(paramName, parameterType, size).Value = item;
                    index++;
                    paramNames.Append(paramName + ",");
                }
            }
            parameterNames = paramNames.ToString();
            if (!string.IsNullOrEmpty(parameterNames))
            {
                parameterNames = parameterNames.Substring(0, parameterNames.Length - 1);
            }
        }

        public IDbDataParameter CreateParameter()
		{
			return dbCommand.CreateParameter();
		}

    	protected void AssignParameterType(IDbDataParameter parameter, Enum parameterType)
        {
            if (parameterType != null)
            {
                if (parameterType is DbType)
                {
                    parameter.DbType = (DbType) parameterType;
                }
                else
                {
                	dbProvider.CheckedParameterType(parameter.ParameterName, parameterType);
                }
            }
        }
    }
}