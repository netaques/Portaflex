using System;
using System.Windows.Forms;
using Portaflex.Data;

namespace Portaflex
{
    public partial class DirLock : Form
    {
        private Directing dir;

        public DirLock(ref Total t)
        {
            InitializeComponent();
            dir = t.Dir;
            this.StartPosition = FormStartPosition.CenterParent;

            OKbutton.Enabled = false;
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            dir.Password = String2MD5.calc(pwdTextBox.Text);                      
            this.DialogResult = DialogResult.OK;
            this.Close();
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
            if (pwd2TextBox.Text != pwdTextBox.Text || pwdTextBox.Text == "")
                OKbutton.Enabled = false;
            else
                OKbutton.Enabled = true;
        }

    }
}
