namespace Portaflex
{
    partial class DirLock
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.OKbutton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.pwdLabel = new System.Windows.Forms.Label();
            this.pwd2Label = new System.Windows.Forms.Label();
            this.pwdTextBox = new System.Windows.Forms.TextBox();
            this.pwd2TextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(283, 102);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 3;
            // 
            // OKbutton
            // 
            this.OKbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKbutton.Location = new System.Drawing.Point(43, 74);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(75, 23);
            this.OKbutton.TabIndex = 3;
            this.OKbutton.Text = "OK";
            this.OKbutton.UseVisualStyleBackColor = true;
            this.OKbutton.Click += new System.EventHandler(this.OKbutton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(135, 74);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Zrušit";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // pwdLabel
            // 
            this.pwdLabel.AutoSize = true;
            this.pwdLabel.Location = new System.Drawing.Point(13, 13);
            this.pwdLabel.Name = "pwdLabel";
            this.pwdLabel.Size = new System.Drawing.Size(37, 13);
            this.pwdLabel.TabIndex = 9;
            this.pwdLabel.Text = "Heslo:";
            // 
            // pwd2Label
            // 
            this.pwd2Label.AutoSize = true;
            this.pwd2Label.Location = new System.Drawing.Point(13, 43);
            this.pwd2Label.Name = "pwd2Label";
            this.pwd2Label.Size = new System.Drawing.Size(79, 13);
            this.pwd2Label.TabIndex = 10;
            this.pwd2Label.Text = "Heslo podruhé:";
            // 
            // pwdTextBox
            // 
            this.pwdTextBox.Location = new System.Drawing.Point(120, 10);
            this.pwdTextBox.Name = "pwdTextBox";
            this.pwdTextBox.PasswordChar = '*';
            this.pwdTextBox.Size = new System.Drawing.Size(120, 20);
            this.pwdTextBox.TabIndex = 1;
            this.pwdTextBox.EnabledChanged += new System.EventHandler(this.pwdTextBox_EnabledChanged);
            this.pwdTextBox.TextChanged += new System.EventHandler(this.pwdTextBox_TextChanged);
            // 
            // pwd2TextBox
            // 
            this.pwd2TextBox.Location = new System.Drawing.Point(120, 40);
            this.pwd2TextBox.Name = "pwd2TextBox";
            this.pwd2TextBox.PasswordChar = '*';
            this.pwd2TextBox.Size = new System.Drawing.Size(120, 20);
            this.pwd2TextBox.TabIndex = 2;
            this.pwd2TextBox.EnabledChanged += new System.EventHandler(this.pwd2TextBox_EnabledChanged);
            this.pwd2TextBox.TextChanged += new System.EventHandler(this.pwd2TextBox_TextChanged);
            // 
            // DirLock
            // 
            this.AcceptButton = this.OKbutton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(252, 112);
            this.ControlBox = false;
            this.Controls.Add(this.pwd2TextBox);
            this.Controls.Add(this.pwdTextBox);
            this.Controls.Add(this.pwd2Label);
            this.Controls.Add(this.pwdLabel);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.OKbutton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DirLock";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Zamknout";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button OKbutton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label pwdLabel;
        private System.Windows.Forms.Label pwd2Label;
        private System.Windows.Forms.TextBox pwdTextBox;
        private System.Windows.Forms.TextBox pwd2TextBox;
    }
}