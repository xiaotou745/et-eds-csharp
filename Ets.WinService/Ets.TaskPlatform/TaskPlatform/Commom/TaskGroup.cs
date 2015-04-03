using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraNavBar;

namespace TaskPlatform.Commom
{
    /// <summary>
    /// 计划任务分组
    /// </summary>
    [Serializable]
    public class TaskGroup
    {
        private string _groupName = "默认分组";
        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }
        [NonSerialized]
        private NavBarGroup _groupControl = null;
        /// <summary>
        /// 分组控件
        /// </summary>
        public NavBarGroup GroupControl
        {
            get { return _groupControl; }
            set { _groupControl = value; }
        }
        private bool _isDefault = true;
        /// <summary>
        /// 是否为默认分组
        /// </summary>
        public bool IsDefault
        {
            get { return _isDefault; }
            set { _isDefault = value; }
        }
        private List<string> _taskList = null;
        /// <summary>
        /// 包含的任务列表
        /// </summary>
        public List<string> TaskList
        {
            get
            {
                if (_taskList == null)
                {
                    _taskList = new List<string>();
                }
                return _taskList;
            }
            set { _taskList = value; }
        }
    }
}
