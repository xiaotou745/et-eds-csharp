using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace TaskPlatform.Forms
{
    public partial class NewCustomArgForm : DevExpress.XtraEditors.XtraForm
    {
        public NewCustomArgForm()
        {
            InitializeComponent();
        }
        public string Key { get; set; }
        public string Value { get; set; }

        public bool IsOK { get; private set; }

        private void NewCustomArgForm_Load(object sender, EventArgs e)
        {
            IsOK = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKey.Text))
            {
                XtraMessageBox.Show(this, "键名不允许为空。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Key = txtKey.Text;
                Value = txtValue.Text;
                IsOK = true;
                DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsOK = false;
            this.Close();
        }
    }
}