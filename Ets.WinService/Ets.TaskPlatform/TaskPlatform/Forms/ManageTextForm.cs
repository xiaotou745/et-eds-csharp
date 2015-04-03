using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Utils;

namespace TaskPlatform.Forms
{
    public partial class ManageTextForm : DevExpress.XtraEditors.XtraForm
    {
        public ManageTextForm()
        {
            InitializeComponent();
        }

        private void txtGroupName_EditValueChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = txtText.Text.Trim().Length > 0;
        }

        private bool _isOK = false;

        public bool IsOK
        {
            get { return _isOK; }
            set { _isOK = value; }
        }

        private string _labelText = "分组名称";

        public string LabelText
        {
            get { return _labelText; }
            set { _labelText = value; }
        }

        public string StringText { get; set; }

        public SuperToolTip SuperTip { get; set; }

        private void NewTaskGroupForm_Load(object sender, EventArgs e)
        {
            txtText.Text = (StringText ?? "").Trim();
            lbLabel.Text = LabelText + "：";
            if (SuperTip != null)
            {
                txtText.SuperTip = SuperTip;
                btnOK.SuperTip = SuperTip;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            StringText = txtText.Text.Trim();
            IsOK = true;
            Close();
        }
    }
}