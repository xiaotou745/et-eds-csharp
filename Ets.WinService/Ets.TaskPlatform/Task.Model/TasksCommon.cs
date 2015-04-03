using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task.Model
{
    public class TasksCommon
    {

        /// <summary>
        /// 服务名称
        /// </summary>
        private string _dealFullName;
        public string DealFullName
        {
            set { _dealFullName = value; }
            get { return _dealFullName; }
        }
        /// <summary>
        /// 补贴配置
        /// </summary>
        private string _commisionConfig;
        public string CommisionConfig
        {
            set { _commisionConfig = value; }
            get { return _commisionConfig; }
        }
        /// <summary>
        /// 数据库连接字符串(读)
        /// </summary>
        private string _connStrRead;
        public string ConnStrRead
        {
            set { _connStrRead = value; }
            get { return _connStrRead; }
        }
        /// <summary>
        /// 数据库连接字符串(写)
        /// </summary>
        private string _connStrWrite;
        public string ConnStrWrite
        {
            set { _connStrWrite = value; }
            get { return _connStrWrite; }
        }
        
        /// <summary>
        /// 线程序号
        /// </summary>
        private int _threadIndex;
        public int ThreadIndex
        {
            set { _threadIndex = value; }
            get { return _threadIndex; }
        }
        
        private int _maxQty;
        public int MaxQty
        {
            set { _maxQty = value; }
            get { return _maxQty; }
        }
        public TasksCommon(string connStrRead, string connStrWrite, string commisionConfig,int maxQty)
        {
            _connStrRead = connStrRead;
            _connStrWrite = connStrWrite;
            _commisionConfig = commisionConfig;
            _maxQty = maxQty;
        }
       
    }
}
