using ETS.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{
    public class SimpleResultModel
    {
        public SimpleResultModel(int status)
            : this(status, string.Empty)
        {

        }

        public SimpleResultModel(int status, string message)
        {
            this.Status = status;
            this.Message = message;
        }

        public static SimpleResultModel Conclude(Enum status)
        {
            var enumItem = EnumExtenstion.GetEnumItem(status.GetType(), status);
            return new SimpleResultModel(enumItem.Value, enumItem.Text);
        }

        public int Status { get; set; }
        public string Message { get; set; }
    }
    public class ResultModel
    {
        public ResultModel(bool isSuccess, string message)
            : this(isSuccess, message, null)
        {
        }

        public ResultModel(bool isSuccess, string message, object data)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
            this.Data = data;
        }

        public bool IsSuccess { get;  set; }
        public string Message { get;  set; }
        public object Data { get; set; }
    }

    public class ResultModel<TResult>
    {
        protected ResultModel(int status, string message, TResult result)
        {
            this.Status = status;
            this.Message = message;
            this.Result = result;
        }

        public static ResultModel<TResult> Conclude(Enum status)
        {   
            return Conclude(status, default(TResult));
        }

        public static ResultModel<TResult> Conclude(Enum status, TResult result)
        {
            var enumItem = EnumExtenstion.GetEnumItem(status.GetType(), status);
            return new ResultModel<TResult>(enumItem.Value, enumItem.Text, result);
        }

        public int Status { get; protected set; }
        public string Message { get; protected set; }
        public TResult Result { get; protected set; }
    }
    /// <summary>
    ///  美团结果自定义类
    /// </summary>
    public class ResultModelToString
    {
        public ResultModelToString(string data)
        {
            this.data = data;
        }

        public string data { get; protected set; }
    }
}
