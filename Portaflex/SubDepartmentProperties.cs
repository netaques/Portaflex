using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Portaflex.Data;

namespace Portaflex
{
    public partial class SubDepartmentProperties : Form
    {
        private SubDepartment d;

        public SubDepartmentProperties(ref SubDepartment d, string header = "Vlastnosti", int max = 100)
        {
            InitializeComponent();
            init(d, header, max);
            label1.Text = "Jméno podstřediska:";        
        }

        private void init(SubDepartment d, string header, int max)
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.d = d;
            OKbutton.Enabled = false;
            procUpDown.Maximum = max;
            this.Text = header;
            if (d.Name != null)
                nameTextBox.Text = d.Name;
            procUpDown.Value = d.Proc;
            intProcUpDown.Value = d.IntProc;
        }

        private void nameLabel_TextChanged(object sender, EventArgs e)
        {
            bool ok = !(nameTextBox.Text == "");
            OKbutton.Enabled = ok;
            //nameTextBox.BackColor = ok ? Color.White : Color.Tomato;
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            d.Name = nameTextBox.Text;
            d.Proc = procUpDown.Value;
            d.IntProc = intProcUpDown.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void DepartmentProperties_Shown(object sender, EventArgs e)
        {
            nameTextBox.Focus();
        }
    }
}
