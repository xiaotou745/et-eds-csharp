using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using TaskPlatform.TaskInterface;
using System.Linq;

namespace TaskPlatform.Controls
{
    public partial class TaskParameterEditor : DevExpress.XtraEditors.XtraUserControl
    {
        public TaskParameterEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 计划任务参数
        /// </summary>
        public TaskParameter Parameter { get; set; }

        private void TaskParameterEditor_Load(object sender, EventArgs e)
        {
            List<ParameterType> parameterTypes = EnumDescription.GetEnumValues<ParameterType>();
            DataTable table = new DataTable();
            table.Columns.Add("ValueType");
            table.Columns.Add("ValueTypeKey");
            parameterTypes.ForEach(type =>
            {
                table.LoadDataRow(new object[] { type.GetEnumDescription(), type.ToString() }, true);
            });
            lueValueType.Properties.DataSource = table;
            lueValueType.EditValue = ParameterType.String.ToString();

            if (Parameter == null)
            {
                Parameter = new TaskParameter();
                Parameter.CanNotInOptions = true;
            }

            txtKey.Text = Parameter.Key;
            lueValueType.EditValue = Parameter.ValueType.ToString();
            ckbCanChangeValueType.Checked = Parameter.PlatformCanChangeValueType;
            txtValue.Text = Parameter.Value;
            txtOptions.Lines = Parameter.Options.ToArray();

            SetAutoCompleteDataSource();
        }

        private void txtOptions_EditValueChanged(object sender, EventArgs e)
        {
            SetAutoCompleteDataSource();
        }

        private void SetAutoCompleteDataSource()
        {
            if (txtOptions.Lines.Length > 0)
            {
                string[] options = (from f in txtOptions.Lines
                                    where !string.IsNullOrWhiteSpace(f)
                                    select f.Trim()).ToArray();
                if (options.Length > 0)
                {
                    txtValue.AutoCompleteMode = AutoCompleteMode.Suggest;
                    txtValue.AutoCompleteCustomSource.AddRange(options);
                }
                else
                {
                    txtValue.AutoCompleteMode = AutoCompleteMode.None;
                }
            }
            else
            {
                txtValue.AutoCompleteMode = AutoCompleteMode.None;
            }
        }

        /// <summary>
        /// 检查是否合法
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
            return false;
        }
    }
}
