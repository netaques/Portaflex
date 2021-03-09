using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Portaflex
{
    public partial class BudgetList : ListView
    {
        public BudgetList()
        {
            InitializeComponent();
            View = System.Windows.Forms.View.Details;
            Dock = DockStyle.Fill;
            GridLines = true;
            FullRowSelect = true;
            
            Columns.Add("id","Číslo účtu",100);
            Columns.Add("name","Název účtu",100);
            Columns.Add("total","Celkem",100);

            Items.Add(new ListViewItem(new string[] {"06854","elektricity","2548"}));
        }
    }
}
