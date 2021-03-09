using System;
using System.Windows.Forms;
using Portaflex.Data;

namespace Portaflex
{
    public partial class DepartmentProperties : Form
    {
        private AbstractDepartment d;
        private bool created;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="d">department, ktery bude modifikovan</param>
        /// <param name="created">flag, zda se vytvari novy department nebo upravuje stavajici</param>
        /// <param name="header">text, ktery ma byt zobrazen jako nadpis formulare</param>
        /// <param name="max">maximalni hodnota procentualniho zastoupeni</param>
        public DepartmentProperties(ref Department d, bool created, string header = "Vlastnosti", int max = 100)
        {
            InitializeComponent();
            this.created = created;
            init((AbstractDepartment)d, header, max);
            if (!created && d.Password != "")
            {
                pwdCheckBox.Checked = true;
                hidePwd();
            }
        }

        public DepartmentProperties(ref SubDepartment d, string header = "Vlastnosti", int max = 100)
        {
            InitializeComponent();
            init((AbstractDepartment)d, header, max);
            label1.Text = "Jméno podstřediska:";
            hidePwd();           
        }

        private void init(AbstractDepartment d, string header, int max)
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.d = (AbstractDepartment)d;
            OKbutton.Enabled = false;
            procUpDown.Maximum = max;
            this.Text = header;
            if (d.Name != null)
                nameTextBox.Text = d.Name;
            procUpDown.Value = d.Proc;            
        }

        private void nameLabel_TextChanged(object sender, EventArgs e)
        {
            var ok = !(nameTextBox.Text == "");
            OKbutton.Enabled = ok;
            //nameTextBox.BackColor = ok ? Color.White : Color.Tomato;
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            d.Name = nameTextBox.Text;
            d.Proc = procUpDown.Value;
            if (d is Department)
            {
                if (pwdCheckBox.Checked)
                    ((Department)d).Password = String2MD5.calc(pwdTextBox.Text);
                else
                    ((Department)d).Password = "";
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void DepartmentProperties_Shown(object sender, EventArgs e)
        {
            nameTextBox.Focus();
        }

        private void pwdCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var check = pwdCheckBox.Checked;
            pwdLabel.Enabled = check;
            pwd2Label.Enabled = check;
            pwdTextBox.Enabled = check;
            pwd2TextBox.Enabled = check;

            if (!check && !created)
                showPwd();
            tryEnableOkButton();
        }

        private void pwdTextBox_EnabledChanged(object sender, EventArgs e)
        {
            if (!pwdTextBox.Enabled)
                pwdTextBox.Text = "";
        }

        private void pwd2TextBox_EnabledChanged(object sender, EventArgs e)
        {
            if (!pwd2TextBox.Enabled)
                pwd2TextBox.Text = "";
        }

        private void pwdTextBox_TextChanged(object sender, EventArgs e)
        {
            tryEnableOkButton();
        }

        private void pwd2TextBox_TextChanged(object sender, EventArgs e)
        {
            tryEnableOkButton();
        }

        private void tryEnableOkButton()
        {
            if (pwdCheckBox.Checked && (pwd2TextBox.Text != pwdTextBox.Text || pwdTextBox.Text == "") && pwdTextBox.Visible)
                OKbutton.Enabled = false;
            else
                OKbutton.Enabled = true;
        }

        private void hidePwd()
        {
            var offset = 20;
            if (!(d is Department))
            {
                offset = 0;
                pwdProtectLabel.Visible = false;
                pwdCheckBox.Visible = false;
            }
            pwdLabel.Visible = false;
            pwdTextBox.Visible = false;
            pwd2Label.Visible = false;
            pwd2TextBox.Visible = false;

            OKbutton.SetBounds(OKbutton.Location.X, 80 + offset, OKbutton.Width, OKbutton.Height);
            cancelButton.SetBounds(cancelButton.Location.X, 80 + offset, cancelButton.Width, cancelButton.Height);
            SetBounds(this.Location.X, this.Location.Y, this.Width, 160);
        }

        private void showPwd()
        {
            pwdProtectLabel.Visible = true;
            pwdCheckBox.Visible = true;
            pwdLabel.Visible = true;
            pwdTextBox.Visible = true;
            pwd2Label.Visible = true;
            pwd2TextBox.Visible = true;

            OKbutton.SetBounds(OKbutton.Location.X, 171, OKbutton.Width, OKbutton.Height);
            cancelButton.SetBounds(cancelButton.Location.X, 171, cancelButton.Width, cancelButton.Height);
            SetBounds(this.Location.X, this.Location.Y, this.Width, 238);
        }

    }
}
