using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.WxPay
{
    [Serializable]
    public class ReturnValue
    {
        private bool hasError;
        private string errorCode;
        private string message;
        private object returnObject;
        private Hashtable mapData = null;

        #region ReturnValue
        public ReturnValue()
        {
            hasError = false;
            errorCode = "";
            message = "";
            returnObject = null;
        }

        public ReturnValue(bool hasError, string errCode, string message, object retObj)
        {
            this.hasError = hasError;
            this.errorCode = errCode;
            this.message = message;
            this.returnObject = retObj;
        }

        public ReturnValue(bool hasError, string errCode, string message)
        {
            this.hasError = hasError;
            this.errorCode = errCode;
            this.message = message;
            this.returnObject = null;
        }
        public ReturnValue(bool hasError, string message)
        {
            this.hasError = hasError;
            this.errorCode = null;
            this.message = message;
            this.returnObject = null;
        }
        public ReturnValue(string errCode, string errMessage)
        {
            this.hasError = true;
            this.errorCode = errCode;
            this.message = errMessage;
            this.returnObject = null;
        }

        public ReturnValue(string errMessage)
        {
            this.hasError = false;
            this.errorCode = null;
            this.message = errMessage;
            this.returnObject = null;
        }
        #endregion ReturnValue

        #region HasError
        public bool HasError
        {
            get
            {
                return hasError;
            }
            set
            {
                hasError = value;
            }
        }
        #endregion HasError

        #region ErrorCode
        public string ErrorCode
        {
            get
            {
                return errorCode;
            }
            set
            {
                errorCode = value;
            }
        }
        #endregion ErrorCode

        #region Message
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }
        #endregion Message

        #region ReturnObject
        public object ReturnObject
        {
            get
            {
                return returnObject;
            }
            set
            {
                returnObject = value;
            }
        }
        #endregion ReturnObject

        #region GetValue
        public object GetValue(string key)
        {
            if (mapData == null)
            {
                return null;
            }
            if (!mapData.ContainsKey(key))
            {
                return null;
            }
            return mapData[key];
        }
        #endregion GetValue

        #region GetStringValue
        public string GetStringValue(string key, string defaultValue)
        {
            object objValue = GetValue(key);
            if (objValue == null)
            {
                return defaultValue;
            }
            else
            {
                return objValue.ToString();
            }
        }
        public string GetStringValue(string key)
        {
            return GetStringValue(key, "");
        }
        #endregion GetStringValue

        #region GetIntValue
        public int GetIntValue(string key, int defaultValue)
        {
            object objValue = GetValue(key);
            try
            {
                if (objValue == null)
                {
                    return defaultValue;
                }
                return Int32.Parse(objValue.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }
        public int GetIntValue(string key)
        {
            return GetIntValue(key, 0);
        }
        #endregion GetIntValue

        #region GetDecimalValue
        public decimal GetDecimalValue(string key, decimal defaultValue)
        {
            object objValue = GetValue(key);
            try
            {
                if (objValue == null)
                {
                    return defaultValue;
                }
                return decimal.Parse(objValue.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }
        public decimal GetDecimalValue(string key)
        {
            return GetDecimalValue(key, 0);
        }
        #endregion GetDecimalValue

        #region GetBooleanValue
        public bool GetBooleanValue(string key, bool defaultValue)
        {
            object objValue = GetValue(key);
            try
            {
                if (objValue == null)
                {
                    return defaultValue;
                }
                return Boolean.Parse(objValue.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }
        public bool GetBooleanValue(string key)
        {
            return GetBooleanValue(key, false);
        }
        #endregion GetBooleanValue

        #region GetFloatValue
        public float GetFloatValue(string key, float defaultValue)
        {
            object objValue = GetValue(key);
            try
            {
                if (objValue == null)
                {
                    return defaultValue;
                }
                return float.Parse(objValue.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }
        public float GetFloatValue(string key)
        {
            return GetFloatValue(key, 0);
        }
        #endregion GetFloatValue

        #region GetDateTimeValue
        public DateTime GetDateTimeValue(string key, DateTime defaultValue)
        {
            object objValue = GetValue(key);
            try
            {
                if (objValue == null)
                {
                    return defaultValue;
                }
                return DateTime.Parse(objValue.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }
        public DateTime GetDateTimeValue(string key)
        {
            return GetDateTimeValue(key, DateTime.Now);
        }
        #endregion GetDateTimeValue

        #region PutValue
        public void PutValue(string key, object valueObj)
        {
            if (mapData == null)
            {
                mapData = new Hashtable(5);
            }
            mapData[key] = valueObj;
        }
        public void PutValue(ReturnValue retValue)
        {
            if (mapData == null)
            {
                mapData = new Hashtable(5);
            }
            try
            {
                IDictionaryEnumerator dict = retValue.mapData.GetEnumerator();
                while (dict.MoveNext())
                {
                    mapData[dict.Key] = dict.Value;
                }
            }
            catch
            {
            }
        }
        public void PutValue(DataRow drData)
        {
            if (mapData == null)
            {
                mapData = new Hashtable(5);
            }
            try
            {
                foreach (DataColumn dc in drData.Table.Columns)
                {
                    mapData[dc.ColumnName] = drData[dc.ColumnName];
                }
            }
            catch
            {
            }
        }
        #endregion PutValue
    }
}
