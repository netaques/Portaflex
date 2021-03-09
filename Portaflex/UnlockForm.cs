using System;
using System.Windows.Forms;

namespace Portaflex
{
    public partial class UnlockForm : Form
    {
        public string Password { get; set; }

        public UnlockForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            OKButton.Enabled = false;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Password = String2MD5.calc(pwdTextBox.Text);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void pwdTextBox_TextChanged(object sender, EventArgs e)
        {
            OKButton.Enabled = pwdTextBox.Text != "";
        }
    }
}
