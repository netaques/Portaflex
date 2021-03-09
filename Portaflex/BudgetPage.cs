using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Portaflex.Data;
using System.Collections;
using System.Reflection;

namespace Portaflex
{
    public partial class BudgetPage : UserControl
    {
        #region Soukrome promenne
        private Department department;
        private int selectedColumnIndex = -1;
        private int selectedRowIndex = -1;
        private TabPage tabpage;
        #endregion

        #region Konstanty
        /// <summary>
        /// Celkovy pocet permanentnich sloupcu (4).
        /// </summary>
        private const int PERM_COLS = 4;
        /// <summary>
        /// Pocet sloupcu na jedno pododdeleni (4).
        /// </summary>
        private const int COLS_PER_DEP = 4;
        /// <summary>
        /// Pozice sloupce pro id uctu (0).
        /// </summary>
        private const int ID_COL = 0;
        /// <summary>
        /// Pozice sloupce pro nazev uctu (1).
        /// </summary>
        private const int NAME_COL = 1;
        /// <summary>
        /// Pozice sloupce pro soucet (2).
        /// </summary>
        private const int TOTAL_COL = 2;
        /// <summary>
        /// Pozice sloupce pro vnitrni rezii (3).
        /// </summary>
        private const int DIR_COL = 3;
        #endregion

        #region Life
        public BudgetPage() : this(new Department(), null)
        {
        }

        public BudgetPage(Department d, TabPage t)
        {
            InitializeComponent();
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null,
            dataGridView1, new object[] { true });
            setTab(t);
            this.Name = Texts.BudgetPage;
            
            dataGridView1.DataError +=new DataGridViewDataErrorEventHandler(dataGridView1_DataError);
            InitializeDepartment(d);
            dataGridView1.Columns[TOTAL_COL].ReadOnly = true;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            formateNumberColumn(dataGridView1.Columns[TOTAL_COL]);
            formateNumberColumn(dataGridView1.Columns[DIR_COL]);
        }

        /// <summary>
        /// Nacte do DataGridView data z predaneho departmentu. 
        /// </summary>
        /// <param name="d">Department pro vlozeni do DataGridView</param>
        private void InitializeDepartment(Department d)
        {                                             
            this.department = d;
            // event handlery
            d.DepartmentChanged += new DepartmentChangedHandler(d_DepartmentChanged);
            d.Parent.BudgetChanged += new BudgetChangedHandler(budgetChanged);
            d.Parent.Budgets.ListChanged += new ListChangedEventHandler(Budgets_ListChanged);
            d.SubDepartments.ListChanged += new ListChangedEventHandler(Subs_ListChanged);
            d.Parent.DirChanged += new EventHandler(Parent_DirChanged);
            if(d.Parent.Dir != null)
                department.Parent.Dir.Values.ListChanged += new ListChangedEventHandler(ParentDirValues_ListChanged);
            d.InternalDirChanged += new EventHandler(d_InternalDirChanged);
            if (d.InternalDir != null)
            {
                newDirButton.Enabled = false;
                dataGridView1.Columns[DIR_COL].Visible = true;
                d.InternalDir.Values.ListChanged += new ListChangedEventHandler(InternalDirValues_ListChanged);
            }

            // vytvori potrebny pocet sloupcu
            foreach(SubDepartment sub in d.SubDepartments)
            {
                //sloupce podstrediska
                dataGridView1.Columns.Add(sub.Name, sub.Name);
                dataGridView1.Columns[dataGridView1.Columns.Count - 1].Visible = !sub.Hidden;
                //prislusna vnejsi rezie
                dataGridView1.Columns.Add(createColumn("FR " + sub.Name + " (" + sub.Proc + "%)", department.Parent.Dir != null && !sub.Hidden));
                //vnitrni rezie
                dataGridView1.Columns.Add(createColumn("SR " + sub.Name + " (" + sub.IntProc + "%)", department.InternalDir != null && !sub.Hidden));
                // celkem za pododdeleni
                dataGridView1.Columns.Add(createColumn(sub.Name + " celkem", true));
                // event handler pro poslouchani zmen v oddeleni
                sub.DepartmentChanged += new DepartmentChangedHandler(sub_DepartmentChanged);
            }

            // naplni tabulku daty
            foreach (Budget b in d.Parent.Budgets)
            {
                int i = d.Parent.Budgets.IndexOf(b);
                double sum = 0;
                DataGridViewRow row = fillRowWithData(b);                
                foreach(SubDepartment sub in d.SubDepartments)
                {
                    double inner = 0, outer = 0;
                    // vlastni hodnota ucti podstrediska
                    DataGridViewTextBoxCell newcell = new DataGridViewTextBoxCell();
                    newcell.Value = sub.Values[i];
                    row.Cells.Add(newcell);
                    // vnejsi rezie
                    DataGridViewTextBoxCell outer_dircell = new DataGridViewTextBoxCell();
                    if (department.Parent.Dir != null)
                        outer = department.Parent.Dir.Values[i] * ((double)department.Proc / 100) * ((double)sub.Proc / 100);
                    outer_dircell.Value = outer;
                    row.Cells.Add(outer_dircell);
                    // vnitrni rezie
                    DataGridViewTextBoxCell inner_dircell = new DataGridViewTextBoxCell();
                    if (department.InternalDir != null)
                        inner =  department.InternalDir.Values[i] * ((double)sub.IntProc / 100);
                    inner_dircell.Value = inner;
                    row.Cells.Add(inner_dircell);
                    // celkem za pododdeleni
                    DataGridViewTextBoxCell subtotal = new DataGridViewTextBoxCell();
                    subtotal.Value = sub.Values[i] + inner + outer;
                    row.Cells.Add(subtotal);
                    sum += sub.Values[i] + inner + outer; 
                }
                row.Cells[TOTAL_COL].Value = sumRow(i);
                dataGridView1.Rows.Add(row);

                if (b.Sum)
                {
                    DataGridViewCellStyle s = new DataGridViewCellStyle();
                    s.Font = new Font(DefaultFont, FontStyle.Bold);
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle = s;

                    for (int col = TOTAL_COL; col < dataGridView1.ColumnCount; col++)
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[col].ReadOnly = true;
                        calcSum(i, col);
                    }
                }

                if (b.Income)
                    setRowColorAsIncome(row, -20);
                else
                    setRowColorAsExpense(row);
            }
            createTotalRow();
            checkProcentual();
            updateStats();
        }
        #endregion

        #region GUI event handlery
        private void exportSubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog expDialog = new SaveFileDialog();
            SubDepartment sub = department.SubDepartments[(selectedColumnIndex - PERM_COLS) / COLS_PER_DEP];
            expDialog.Filter = "FLX soubor|*.flx";
            expDialog.Title = "Exportovat podstředisko";
            expDialog.AddExtension = true;
            if (expDialog.ShowDialog() == DialogResult.OK)
            {
                XMLSerializer ser = new XMLSerializer(expDialog.FileName);
                ser.SaveTotal(department.Parent);
                Total t = ser.LoadTotal();

                List<Department> depToRemove = new List<Department>();
                foreach (Department dep in t.Departments)
                    if (dep.Name != department.Name)
                        depToRemove.Add(dep);
                foreach (Department dep in depToRemove)
                    t.Departments.Remove(dep);
                List<SubDepartment> subToRemove = new List<SubDepartment>();
                foreach (Department dep in t.Departments)
                {
                    subToRemove.Clear();
                    foreach (SubDepartment s in dep.SubDepartments)
                        if (s.Name != sub.Name)
                            subToRemove.Add(s);
                    foreach (SubDepartment s in subToRemove)
                        dep.SubDepartments.Remove(s);
                }


                ser.SaveTotal(t);
            }
        }

        private void expandAllButton_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (SubDepartment sub in department.SubDepartments)
            {
                miniMaxiColumns(true, i++);
                minimaxiToolStripMenuItem.Text = Texts.Minimalize;
                sub.Hidden = false;
            }
        }

        private void collapseAllButton_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (SubDepartment sub in department.SubDepartments)
            {
                miniMaxiColumns(false, i++);
                minimaxiToolStripMenuItem.Text = Texts.Maximalize;
                sub.Hidden = true;
            }
        }

        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            if (dataGridView1.Columns.IndexOf(e.Column) >= PERM_COLS)
                formateNumberColumn(e.Column);
        }

        private void newSubButton_Click(object sender, EventArgs e)
        {
            SubDepartment sub = new SubDepartment();
            SubDepartmentProperties prop = new SubDepartmentProperties(ref sub, "Nové podstředisko");
            if (prop.ShowDialog() != DialogResult.OK)
                return;
            foreach (Budget b in department.Parent.Budgets)
            {
                sub.Values.Add(0);
            }
            department.SubDepartments.Add(sub);
            sub.DepartmentChanged += new DepartmentChangedHandler(sub_DepartmentChanged);
        }

        private void newSumButton_Click(object sender, EventArgs e)
        {
            department.Parent.Budgets.Add(new Budget(Texts.Sum, Texts.Id, true));
        }

        private void newAcount_Click(object sender, EventArgs e)
        {
            department.Parent.Budgets.Add(new Budget(Texts.Budget, Texts.Id));
        }

        private void newDirButton_Click(object sender, EventArgs e)
        {
            department.InternalDir = new Directing();
            newDirButton.Enabled = false;
        }

        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hinfo = dataGridView1.HitTest(e.X, e.Y);
                // nektery ze sloupcu pododdeleni
                if (hinfo.Type == DataGridViewHitTestType.ColumnHeader && hinfo.ColumnIndex >= PERM_COLS)
                {
                    selectedColumnIndex = hinfo.ColumnIndex;
                    if (department.SubDepartments[(selectedColumnIndex - PERM_COLS) / COLS_PER_DEP].Hidden)
                        minimaxiToolStripMenuItem.Text = Texts.Maximalize;
                    else
                        minimaxiToolStripMenuItem.Text = Texts.Minimalize;
                    columnContextMenu.Show(dataGridView1, new Point(e.X, e.Y));
                }
                // sloupec s vnitrni rezii
                else if (hinfo.Type == DataGridViewHitTestType.ColumnHeader && hinfo.ColumnIndex == DIR_COL)
                {
                    innerDirContextMenu.Show(dataGridView1, new Point(e.X, e.Y));
                }
                // context menu pro radky
                else if (hinfo.Type == DataGridViewHitTestType.RowHeader)
                {
                    selectedRowIndex = hinfo.RowIndex;
                    rowContextMenu.Show(dataGridView1, new Point(e.X, e.Y));
                }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            updateData(e.ColumnIndex, e.RowIndex);
        }

        // Smaze pododdeleni
        private void delColToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedColumnIndex != -1)
            {
                int index = (selectedColumnIndex - PERM_COLS) / COLS_PER_DEP;
                department.SubDepartments.RemoveAt(index);

                selectedColumnIndex = -1;
            }
        }

        // Zobrazi vlastnosti pododdeleni
        private void propToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedColumnIndex != -1)
            {
                SubDepartment sub = department.SubDepartments[(selectedColumnIndex - PERM_COLS) / COLS_PER_DEP];
                SubDepartmentProperties prop = new SubDepartmentProperties(ref sub);
                prop.ShowDialog();
                selectedColumnIndex = -1;
            }
        }

        // Smaze radek uctu
        private void delRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex != -1)
            {
                department.Parent.Budgets.RemoveAt(selectedRowIndex);
                selectedRowIndex = -1;
            }
        }

        // Smaze vnitrni rezii
        private void delDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            department.InternalDir = null;
            newDirButton.Enabled = true;
        }

        // Schova nebo rozbali detaily pododdeleni
        private void minimaxiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedColumnIndex != -1)
            {
                int depIndex = (selectedColumnIndex - PERM_COLS) / COLS_PER_DEP;
                SubDepartment sub = department.SubDepartments[depIndex];
                miniMaxiColumns(sub.Hidden, depIndex);
                // zmenit label v context menu
                if (sub.Hidden)
                    minimaxiToolStripMenuItem.Text = Texts.Minimalize;
                else // schovat
                    minimaxiToolStripMenuItem.Text = Texts.Maximalize;
                sub.Hidden = !sub.Hidden;
                selectedColumnIndex = -1;
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            //MessageBox.Show("Špatně zadaná hodnota, zadejte prosím číslo.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            if (anError.ColumnIndex < dataGridView1.Columns.Count && anError.RowIndex < dataGridView1.Rows.Count)
                ; // dataGridView1[anError.ColumnIndex, anError.RowIndex].Value = 0;
        }
        #endregion

        #region List event handlery
        private void ParentDirValues_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                int i = PERM_COLS + 1;
                double val = department.Parent.Dir.Values[e.NewIndex] * ((double)department.Proc / 100) / 100;
                foreach (SubDepartment sub in department.SubDepartments)
                {
                    dataGridView1.Rows[e.NewIndex].Cells[i].Value = val * (double)sub.Proc;
                    calcSum(e.NewIndex, i);
                    i += COLS_PER_DEP;
                }
                dataGridView1.Rows[e.NewIndex].Cells[TOTAL_COL].Value = sumRow(e.NewIndex);
                calcSum(e.NewIndex, TOTAL_COL);
                updateStats();
            }
        }

        private void InternalDirValues_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                int i = PERM_COLS + 2;
               // double val = department.Parent.Dir.Values[e.NewIndex] * ((double)department.Proc / 100) / 100;
                foreach (SubDepartment sub in department.SubDepartments)
                {
                    dataGridView1.Rows[e.NewIndex].Cells[i].Value = department.InternalDir.Values[e.NewIndex] * (((double)sub.Proc) / 100);
                    calcSum(e.NewIndex, i);
                    i += COLS_PER_DEP;
                }
                updateStats();
            }
        }

        private void Budgets_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                Budget b = department.Parent.Budgets[e.NewIndex];
                dataGridView1.Rows.Insert(e.NewIndex,createNewRow(b.Name, b.ID));

                if (b.Sum)
                {
                    DataGridViewCellStyle s = dataGridView1.Rows[e.NewIndex].DefaultCellStyle;
                    s.Font = new Font(DefaultFont, FontStyle.Bold);
                    dataGridView1.Rows[e.NewIndex].DefaultCellStyle = s;

                    for (int col = 2; col < dataGridView1.ColumnCount; col++)
                    {
                        dataGridView1.Rows[e.NewIndex].Cells[col].ReadOnly = true;
                        calcSum(e.NewIndex, col);
                    }
                    for (int i = e.NewIndex + 1; i < dataGridView1.RowCount; i++)
                    {
                        if (i < department.Parent.Budgets.Count && department.Parent.Budgets[i].Sum)
                        {
                            for (int col = 2; col < dataGridView1.ColumnCount; col++)
                            {
                                calcSum(i, col);
                            }
                            break;
                        }
                    }
                }

                if (b.Income)
                    setRowColorAsIncome(dataGridView1.Rows[e.NewIndex], -20);
                else
                    setRowColorAsExpense(dataGridView1.Rows[e.NewIndex]);
            }
            else if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                dataGridView1.Rows.RemoveAt(e.NewIndex);
                for (int i = TOTAL_COL; i < dataGridView1.ColumnCount; i++)
                {
                    calcSum(e.NewIndex, i);
                }
                updateStats();
            }
        }

        private void Subs_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                // vlozi se sloupec pro podstredisko
                SubDepartment sub = department.SubDepartments[e.NewIndex];
                dataGridView1.Columns.Add(createColumn(sub.Name, true, true));
                // sloupec pro vnejsi rezii

                sub.DepartmentChanged += new DepartmentChangedHandler(sub_DepartmentChanged);
                dataGridView1.Columns.Add(createColumn("FR " + sub.Name + " (" + sub.Proc + "%)", department.Parent.Dir != null));
                // sloupec pro vnitrni rezii
                dataGridView1.Columns.Add(createColumn("SR " + sub.Name + " (" + sub.IntProc + "%)", department.InternalDir != null));
                // sloupec pro subtotal
                dataGridView1.Columns.Add(createColumn(sub.Name + " celkem", true));

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    // nastavi se hodnota pro toto podstredisko
                    dataGridView1.Rows[i].Cells[dataGridView1.Columns.Count - COLS_PER_DEP].Value = sub.Values[i];
                    //zabranime mozne editaci sum budgets
                    if (department.Parent.Budgets[i].Sum)
                        dataGridView1.Rows[i].Cells[dataGridView1.Columns.Count - COLS_PER_DEP].ReadOnly = true;
                    // pokud je nastavena vnejsi rezie, spocte se pro nove suboddeleni
                    double outer = 0, inner = 0;
                    if (department.Parent.Dir != null)
                        outer = department.Parent.Dir.Values[i] * ((double)department.Proc / 100) * ((double)sub.Proc / 100);
                    dataGridView1.Rows[i].Cells[dataGridView1.Columns.Count - COLS_PER_DEP + 1].Value = outer;
                    // totez pro vnitrni rezii
                    if(department.InternalDir != null)
                        inner = department.InternalDir.Values[i] * ((double)sub.IntProc / 100);
                    dataGridView1.Rows[i].Cells[dataGridView1.Columns.Count - COLS_PER_DEP + 2].Value = inner;
                    // subtotal
                    dataGridView1.Rows[i].Cells[dataGridView1.Columns.Count - COLS_PER_DEP + 3].Value = inner + outer;
                    //prepocita se suma
                    for (int j = dataGridView1.Columns.Count - COLS_PER_DEP; j < dataGridView1.Columns.Count - COLS_PER_DEP + 4; j++)
                        calcSum(i, j);

                    if (department.Parent.Budgets[i].Income)
                        setRowColorAsIncome(dataGridView1.Rows[i], -20);
                    else
                        setRowColorAsExpense(dataGridView1.Rows[i]);                     

                }
            }
            else if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                List<DataGridViewColumn> toRemove = new List<DataGridViewColumn>();
                for(int i = 0; i < COLS_PER_DEP; i++)
                    toRemove.Add(dataGridView1.Columns[e.NewIndex * COLS_PER_DEP + PERM_COLS + i]);
                for (int i = 0; i < COLS_PER_DEP; i++)
                    dataGridView1.Columns.Remove(toRemove[i]);
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (department.Parent.Budgets[i].Income)
                        setRowColorAsExpense(dataGridView1.Rows[i]);
                    else
                        setRowColorAsIncome(dataGridView1.Rows[i], -20);
                }
                
            }
            updateStats();
            checkProcentual();
        }

        #endregion

        #region Data event handlery

        private void d_InternalDirChanged(object sender, EventArgs e)
        {
            bool visible = department.InternalDir != null;
            dataGridView1.Columns[DIR_COL].Visible = visible;
            dirColsVisibility(false, visible);
            int i = 0;
            foreach (Budget b in department.Parent.Budgets)
                dataGridView1.Rows[i++].Cells[PERM_COLS - 1].Value = 0;
            if (visible)
                department.InternalDir.Values.ListChanged += new ListChangedEventHandler(InternalDirValues_ListChanged);
            checkProcentual();
            updateStats();
        }

        private void d_DepartmentChanged(object sender, DepartmentChangeEventArgs e)
        {
            if (e.Type == DepartmentChangeTypes.Proc)
            {
                foreach (SubDepartment sub in department.SubDepartments)
                {
                    sub_DepartmentChanged(sub, null);
                    calcDir(sub);
                }
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    if (!department.Parent.Budgets[i].Sum)
                        dataGridView1.Rows[i].Cells[TOTAL_COL].Value = sumRow(i);
                    else
                    {
                        for (int j = TOTAL_COL; j < dataGridView1.ColumnCount; j++)
                            calcSum(i, j);
                    }
                }
                updateStats();
            }
        }

        private void sub_DepartmentChanged(AbstractDepartment sender, DepartmentChangeEventArgs e)
        {
            SubDepartment sub = (SubDepartment)sender;
            int index = department.SubDepartments.IndexOf(sub);
            dataGridView1.Columns[COLS_PER_DEP * index + PERM_COLS].HeaderText = sender.Name;
            dataGridView1.Columns[COLS_PER_DEP * index + PERM_COLS + 1].HeaderText = "FR " + sender.Name + " (" + sender.Proc + "%)";
            dataGridView1.Columns[COLS_PER_DEP * index + PERM_COLS + 2].HeaderText = "SR " + sender.Name + " (" + sub.IntProc + "%)";
            dataGridView1.Columns[COLS_PER_DEP * index + PERM_COLS + 3].HeaderText = sender.Name + " celkem";
            calcDir(sub);
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                if (!department.Parent.Budgets[i].Sum)
                {
                    for (int j = COLS_PER_DEP * index + PERM_COLS; j < COLS_PER_DEP * index + PERM_COLS + 4; j++)
                        calcSum(i, j);
                }
            }
            updateStats();
            checkProcentual();
        }

        private void Parent_DirChanged(object sender, EventArgs e)
        {
            bool visible = department.Parent.Dir != null;
            if(visible)
                department.Parent.Dir.Values.ListChanged += new ListChangedEventHandler(ParentDirValues_ListChanged);
            dirColsVisibility(true, visible);
            updateStats();
            checkProcentual();
        }

        private void budgetChanged(object sender, BudgetEventArgs e)
        {
            int index = e.Index;
            if (index < 0 || index >= dataGridView1.RowCount - 1)
            {
                dataGridView1.Rows.Insert(dataGridView1.RowCount - 1, createNewRow(Texts.Budget, Texts.Id));
                index = index < 0 ? 0 : index;
            }
            dataGridView1.Rows[index].Cells[1].Value = ((Budget)sender).Name;
            dataGridView1.Rows[index].Cells[0].Value = ((Budget)sender).ID;

            if (((Budget)sender).Income)
                setRowColorAsIncome(dataGridView1.Rows[index], -20);
            else
                setRowColorAsExpense(dataGridView1.Rows[index]);
            updateStats();
        }
        #endregion

        #region Soukromi pomocnici

        private void setRowColorAsIncome(DataGridViewRow row, int dim)
        {
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                row.Cells[i].Style.BackColor = PortaflexColors.dimColor(getIndexColor(i), dim);
            }
        }

        private void setRowColorAsExpense(DataGridViewRow row)
        {
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                row.Cells[i].Style.BackColor = getIndexColor(i);
            }
        }

        private Color getIndexColor(int index)
        {
            if (index < PERM_COLS - 1)
                return PortaflexColors.getColumnExpenseColor(0);
            if (index == PERM_COLS - 1)
                return PortaflexColors.getColumnExpenseColor(1);
            return PortaflexColors.getColumnExpenseColor((index - PERM_COLS) / COLS_PER_DEP + 2);
        }

        private double calcNaklad()
        {
            double sum = 0;
            int i = 0;
            foreach (Budget b in department.Parent.Budgets)
            {
                if (!b.Income && !b.Sum)
                    sum += sumRow(i);
                i++;
            }
            return sum;
        }

        private double calcObrat()
        {
            double sum = 0;
            int i = 0;
            foreach (Budget b in department.Parent.Budgets)
            {
                if (b.Income)
                    sum += sumRow(i);
                i++;
            }
            return sum;
        }

        private double calcVysledek()
        {
            return calcObrat() - calcNaklad();
        }

        private void createTotalRow()
        {
            dataGridView1.Rows.Add(createNewRow("Σ", ""));
            DataGridViewRow row = dataGridView1.Rows[dataGridView1.RowCount - 1];

            row.ReadOnly = true;

            DataGridViewCellStyle s = new DataGridViewCellStyle();
            s.Font = new Font(DefaultFont.FontFamily, 10, FontStyle.Bold);
            s.Alignment = DataGridViewContentAlignment.MiddleRight;
            s.BackColor = Color.Gold;//Color.FromArgb(255, 255, 192);
            s.Padding = new System.Windows.Forms.Padding(3);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle = s;
            row.Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void updateStats()
        {
            obratTextLabel.Text = calcObrat().ToString("C");
            vysledekTextLabel.Text = calcVysledek().ToString("C");
            nakladTextLabel.Text = calcNaklad().ToString("C");

            DataGridViewRow totalRow = dataGridView1.Rows[dataGridView1.RowCount - 1];
            for (int i = 2; i < dataGridView1.ColumnCount; i++)
            {
                totalRow.Cells[i].Value = calcColumn(i);
            }
        }

        private double calcColumn(int col)
        {
            double sum = 0;
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                if (!department.Parent.Budgets[i].Sum)
                {
                    if (department.Parent.Budgets[i].Income)
                        sum += Convert.ToDouble(dataGridView1.Rows[i].Cells[col].Value);
                    else
                        sum -= Convert.ToDouble(dataGridView1.Rows[i].Cells[col].Value);
                }
            }
            return sum;
        }

        private void formateNumberColumn(DataGridViewColumn col)
        {           
            col.DefaultCellStyle.Format = "N2";
            col.ValueType = typeof(System.Double);
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.DefaultCellStyle.NullValue = 0;
        }

        public void checkProcentual()
        {
            if (department.SubDepartments.Count != 0 && (department.InternalDir != null || department.Parent.Dir != null))
            {
                decimal counter = 0, intcounter = 0;
                foreach (SubDepartment sub in department.SubDepartments)
                {
         
                    intcounter += sub.IntProc;
                    counter += sub.Proc;
                }
                if ((intcounter != 100 && department.InternalDir != null) || (counter != 100 && department.Proc > 0) && tabpage != null)
                {
                    tabpage.Text = department.Name + " (" + department.Proc + "%)" + Texts.DirErr;
                    tabpage.ImageIndex = 5;
                }
                else
                {
                    tabpage.Text = department.Name + " (" + department.Proc + "%)";
                    tabpage.ImageIndex = department.Locked && department.Password != "" ? 1 : 2;
                }
            }
        }
        /// <summary>
        /// Spocte sumu pro budget sum ktery je nejblizsi zadanemu radku.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void calcSum(int row, int col)
        {
            if (row >= department.Parent.Budgets.Count)
                return;
            // najdeme nejblizsi nasledujici sum budget
            int rowSum;  
            if (!department.Parent.Budgets[row].Sum)
            {
                for (int i = row; i < department.Parent.Budgets.Count; i++)
                {
                    if (department.Parent.Budgets[i].Sum)
                    {
                        row = i;
                        break;
                    }
                    // pokud neexistuje zadny nasledujici budget sum -> konec
                    else if (i + 1 == department.Parent.Budgets.Count)
                        return;
                }
            }
            rowSum = row;
            double sum = 0;
            for (row--; row >= 0 && !department.Parent.Budgets[row].Sum; row--)
            {
                sum += Convert.ToDouble(dataGridView1.Rows[row].Cells[col].Value);
            }
            dataGridView1.Rows[rowSum].Cells[col].Value = sum;
        }

        /// <summary>
        /// Prepocita rezie pro dane podstredisko a zapise vysledek do dataGridView
        /// </summary>
        private void calcDir(SubDepartment sub)
        {
            int i = 0, index = department.SubDepartments.IndexOf(sub);
            foreach (Budget b in department.Parent.Budgets)
            {
                if (department.Parent.Dir != null)
                {
                    // vnejsi
                    double val = department.Parent.Dir.Values[i] * ((double)department.Proc / 100);
                    dataGridView1.Rows[i].Cells[COLS_PER_DEP * index + PERM_COLS + 1].Value = val * ((double)sub.Proc / 100);
                }
                if (department.InternalDir != null)
                {
                    // vnitrni
                    double val = department.InternalDir.Values[i] * (((double)sub.IntProc) / 100);
                    dataGridView1.Rows[i].Cells[COLS_PER_DEP * index + PERM_COLS + 2].Value = val;
                }
                i++;
            }
        }

        private DataGridViewColumn createColumn(string header, bool visible, bool write = false)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();

            col.SortMode = DataGridViewColumnSortMode.NotSortable;
            col.HeaderText = header;

            col.ValueType = typeof(System.Double);
            col.ReadOnly = !write;
            col.Visible = visible;
            return col;
        }

        /// <summary>
        /// Vytvori zacatek radku a naplni jej daty.
        /// </summary>
        /// <param name="b">Ucet ze ktereho budou data cerpana.</param>
        private DataGridViewRow fillRowWithData(Budget b)
        {
            DataGridViewRow row = new DataGridViewRow();
            DataGridViewTextBoxCell idcell = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell namecell = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell totalcell = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell dircell = new DataGridViewTextBoxCell();
            idcell.Value = b.ID;
            namecell.Value = b.Name;
            totalcell.Value = "0";
            dircell.Value = department.InternalDir != null ? department.InternalDir.Values[department.Parent.Budgets.IndexOf(b)].ToString() : "0";
            row.Cells.Add(idcell);
            row.Cells.Add(namecell);
            row.Cells.Add(totalcell);
            row.Cells.Add(dircell);
            return row;
        }

        private void miniMaxiColumns(bool maxi, int subIndex)
        {
            int colIndex = subIndex * COLS_PER_DEP + PERM_COLS;
            dataGridView1.Columns[colIndex++].Visible = maxi;
            if (department.Parent != null)
            {
                dataGridView1.Columns[colIndex++].Visible = maxi && department.Parent.Dir != null;
                dataGridView1.Columns[colIndex].Visible = maxi && department.InternalDir != null;
            }
        }

        private string[] createNewRow(string name, string id)
        {
            string[] s = new string[dataGridView1.Columns.Count];
            s[ID_COL] = id;
            s[NAME_COL] = name;
            s[TOTAL_COL] = "0,00";
            s[DIR_COL] = "0,00";

            for (int i = PERM_COLS; i < s.Length; i++)
                s[i] = "0,00";
            return s;
        }

        private void updateData(int x, int y)
        {
            if (x < 0 || y < 0 || y >= dataGridView1.Rows.Count - 1)
                return;
            string value = dataGridView1.Rows[y].Cells[x].Value.ToString();
            if (x >= PERM_COLS && !department.Parent.Budgets[y].Sum)
            {
                int deppos = (x - PERM_COLS) / COLS_PER_DEP; 
                if (x % COLS_PER_DEP == 0) // subdepartment
                {
                    department.SubDepartments[deppos].Values[y] = Convert.ToDouble(value);
                    calcSum(y, x);
                }
                else if (x % COLS_PER_DEP == 1 || x % COLS_PER_DEP == 2) // vnejsi, vnitrni rezie
                {
                    calcSum(y, x);
                }
                
                dataGridView1.Rows[y].Cells[PERM_COLS - 1 + (deppos + 1) * COLS_PER_DEP].Value = sumSubDep(y,(SubDepartment) department.SubDepartments[deppos]);
                calcSum(y, PERM_COLS - 1 + (deppos + 1) * COLS_PER_DEP);
                dataGridView1.Rows[y].Cells[TOTAL_COL].Value = sumRow(y);
                calcSum(y, TOTAL_COL);
                updateStats();

            }
            else if (x == DIR_COL && department.InternalDir != null && !department.Parent.Budgets[y].Sum) //vnitrni rezie
            {
                department.InternalDir.Values[y] = Convert.ToDouble(value);
                dataGridView1.Rows[y].Cells[TOTAL_COL].Value = sumRow(y);
                calcSum(y, TOTAL_COL);
                calcSum(y, x);
                int deppos = (x - PERM_COLS) / COLS_PER_DEP;
                if (department.SubDepartments.Count > 0)
                {
                    dataGridView1.Rows[y].Cells[PERM_COLS - 1 + (deppos + 1) * COLS_PER_DEP].Value = sumSubDep(y, (SubDepartment)department.SubDepartments[deppos]);
                    calcSum(y, PERM_COLS - 1 + (deppos + 1) * COLS_PER_DEP);
                }
                updateStats();
            }
            else if (x == NAME_COL)
            {
                department.Parent.Budgets[y].Name = value;
            }
            else if (x == ID_COL)
            {
                department.Parent.Budgets[y].ID = value;
            }
        }

        /// <summary>
        /// Projde vsechny sloupce rezie a nastavi jejich viditelnost. Pokud maji byt sloupce schovany, jsou zaroven vynulovany.
        /// dirType urcuje o kterou rezii se jedna: true = vnejsi, false = vnitrni.
        /// </summary>
        /// <param name="visible">Viditelnost</param>
        /// <param name="dirType">true = vnejsi, false = vnitrni</param>
        private void dirColsVisibility(bool dirType, bool visible)
        {
            int i = dirType ? PERM_COLS + 1 : PERM_COLS + 2;
            foreach (SubDepartment sub in department.SubDepartments)
            {
                dataGridView1.Columns[i].Visible = visible && !sub.Hidden;
                if (!visible)
                {
                    int j = 0;
                    foreach (Budget b in department.Parent.Budgets)
                        dataGridView1.Rows[j++].Cells[i].Value = 0.0;
                }
                i += COLS_PER_DEP;
            }
        }

        private double sumSubDep(int row, SubDepartment sub)
        {
            double sum = 0;
            if(row < sub.Values.Count)            
                sum = sub.Values[row]; //hodnota uctu pododdeleni
            // plus vnejsi rezie
            if (department.Parent.Dir != null && row < department.Parent.Dir.Values.Count)
                sum += ((double)department.Parent.Dir.Values[row]) * ((double)department.Proc / 100) * ((double)sub.Proc / 100);
            if (department.InternalDir != null && row < department.InternalDir.Values.Count)
                sum += ((double)department.InternalDir.Values[row]) * ((double)sub.Proc / 100);
            return sum;
        }


        /// <summary>
        /// Secte cely radek vcetne vnitrni a vnejsi rezie.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private double sumRow(int row)
        {
            double sum = 0;
            if (department.Parent.Dir != null)
                sum = ((double)(department.Parent.Dir.Values[row])) * ((double)department.Proc / 100);
            if (department.InternalDir != null)
                sum += department.InternalDir.Values[row];
            foreach (SubDepartment sub in department.SubDepartments)
            {
                sum += sub.Values[row];
            }
            return sum;
        }
        #endregion

        #region Getters a setters
        public DataGridView getData()
        {
            return dataGridView1;
        }

        public void setTab(TabPage t)
        {
            this.tabpage = t;
        }
        #endregion
    }
}
