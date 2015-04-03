using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.Commom
{
    public class SystemTask
    {
        private bool _useForItem = false;
        public SystemTask()
        { }
        public SystemTask(bool useForItem)
        {
            _useForItem = useForItem;
        }
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string DBHelperName { get; set; }
        public string ConnectionString { get; set; }
        public string SQL { get; set; }
        public string LuaScript { get; set; }
        public string CreateTaskTemplateName { get; set; }
        public string FileName { get; set; }
        public override string ToString()
        {
            if (_useForItem)
            {
                return DisplayName;
            }
            else
            {
                return string.Format("{0}({1})", SystemName, DisplayName);
            }
        }

        public string OpenFileVerb { get; set; }

        public string OpenFileArgs { get; set; }
    }
}
