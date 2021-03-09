using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Portaflex
{
    public partial class BackgroundWorkDialog : Form
    {
        private BackgroundWorker worker;
        private Dictionary<string, object> args;
        private RunWorkerCompletedEventHandler completed;

        public BackgroundWorkDialog(DoWorkEventHandler work, RunWorkerCompletedEventHandler completed, Dictionary<string,object> args,String title = "",String label = "")
        {
            InitializeComponent();
            if (label.Length != 0)
                this.label.Text = label;
            if (title.Length != 0)
                this.Text = title;
            this.StartPosition = FormStartPosition.CenterParent;
            this.args = args;
            this.completed = completed;
            this.worker = new BackgroundWorker();
            this.worker.WorkerSupportsCancellation = true;
            this.worker.DoWork += work;
            this.worker.RunWorkerCompleted += completed;
            this.worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BackgroundWorkDialog_Load(object sender, EventArgs e)
        {
            worker.RunWorkerAsync(args);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            worker.RunWorkerCompleted -= completed;
            worker.RunWorkerCompleted -= worker_RunWorkerCompleted;
            worker.CancelAsync();
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
