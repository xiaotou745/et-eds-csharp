using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using TaskPlatform.TaskInterface;

namespace TaskPlatform.Forms
{
    public partial class TaskParametersEditor : DevExpress.XtraEditors.XtraForm
    {
        public TaskParametersEditor()
        {
            InitializeComponent();
        }

        private List<TaskParameter> _parameters = null;

        public List<TaskParameter> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        private void TaskParametersEditor_Load(object sender, EventArgs e)
        {

        }
    }
}