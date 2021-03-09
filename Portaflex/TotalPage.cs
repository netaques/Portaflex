using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Portaflex.Data;
using System.Reflection;

namespace Portaflex
{
    public partial class TotalPage : UserControl
    {
        private Total total;
        private const int PERM_COLS = 4;
        private int selectedColumnIndex = -1;
        private int selectedRowIndex = -1;

        #region Life
        public TotalPage(Total total)
        {
            InitializeComponent();
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null,
            dataGridView1, new object[] { true });
            dataGridView1.DataError += new DataGridViewDataErrorEventHandler(dataGridView1_DataError);
            InitializeData(total);
        }

        private void InitializeData(Total total)
        {
            this.total = total;
            total.BudgetChanged += new BudgetChangedHandler(total_BudgetChanged);
            total.Departments.ListChanged += new ListChangedEventHandler(Departments_ListChanged);
            total.Budgets.ListChanged += new ListChangedEventHandler(Budgets_ListChanged);
            total.DirChanged += new EventHandler(total_DirChanged);
            formateNumberColumn(dataGridView1.Columns[2]);

            dataGridView1.Columns[0].DefaultCellStyle.BackColor = PortaflexColors.getColumnExpenseColor(0);
            dataGridView1.Columns[1].DefaultCellStyle.BackColor = PortaflexColors.getColumnExpenseColor(0);
            dataGridView1.Columns[2].DefaultCellStyle.BackColor = PortaflexColors.getColumnExpenseColor(0);

            if (total.Dir != null)
            {
                dataGridView1.Columns[PERM_COLS - 1].Visible = true;
                dataGridView1.Columns[PERM_COLS - 1].ReadOnly = true;
                dataGridView1.Columns[PERM_COLS - 1].DefaultCellStyle.BackColor = PortaflexColors.getColumnExpenseColor(1);
                formateNumberColumn(dataGridView1.Columns[PERM_COLS - 1]);
                total.Dir.Values.ListChanged += new ListChangedEventHandler(DirValues_ListChanged);
            }
            else
                total.Dir = new Directing();

            foreach (Department dep in total.Departments)
            {
                // vytvorime sloupec pro oddeleni
                string header = createDepHeader(dep);
                dep.DepartmentChanged += new DepartmentChangedHandler(d_DepartmentChanged);
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.DefaultCellStyle.BackColor = PortaflexColors.getColumnExpenseColor(2 + total.Departments.IndexOf(dep));
                formateNumberColumn(col);
                col.HeaderText = header;
                col.ReadOnly = true;
                dataGridView1.Columns.Add(col);
            }

            foreach (Budget b in total.Budgets)
            {
                int i = total.Budgets.IndexOf(b);
                double sum = 0;
                DataGridViewRow row = fillRowWithData(b);
                foreach (Department dep in total.Departments)
                {
                    DataGridViewTextBoxCell newcell = new DataGridViewTextBoxCell();
                    double depSum = sumDep(dep,i);
                    sum += depSum;
                    newcell.Value = depSum;
                    row.Cells.Add(newcell);
                }
                if (total.Dir != null)
                    ;// sum += total.Dir.Values[i];

                row.Cells[2].Value = sum;
                dataGridView1.Rows.Add(row);

                if (b.Sum)
                {
                    DataGridViewCellStyle s = new DataGridViewCellStyle();
                    s.Font = new Font(DefaultFont, FontStyle.Bold);
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle = s;

                    for (int col = 2; col < dataGridView1.ColumnCount; col++)
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[col].ReadOnly = true;
                        calcSum(i, col);
                    }
                }

                if (b.Income)
                    setRowColorAsIncome(row, -20);
            }

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            createTotalRow();
            updateStats();
            
        }
        #endregion

        #region Data event handlers

        private void total_DirChanged(object sender, EventArgs e)
        {
            bool hasDir = total.Dir != null;
            dataGridView1.Columns[PERM_COLS - 1].Visible = hasDir;
            formateNumberColumn(dataGridView1.Columns[PERM_COLS - 1]);
            if (hasDir)
            {
                int i = 0;
                foreach (Budget b in total.Budgets)
                    dataGridView1.Rows[i++].Cells[PERM_COLS - 1].Value = 0;
                total.Dir.Values.ListChanged += new ListChangedEventHandler(DirValues_ListChanged);
            }
            else
            {
                int i = 0;
                foreach (Budget b in total.Budgets)
                    dataGridView1.Rows[i++].Cells[PERM_COLS - 1].Value = 0;       
            }
        }

        private void DirValues_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                int row = e.NewIndex, i = 1;
                dataGridView1.Rows[row].Cells[3].Value = total.Dir.Values[row];
                calcSum(row, 3);
                dataGridView1.Rows[row].Cells[2].Value = sumRow(row);
                calcSum(row, 2);
                foreach (Department d in total.Departments)
                {
                    dataGridView1.Rows[row].Cells[i + 3].Value = sumDep(d, row);
                    calcSum(row, i + 3);
                    i++;
                }
            }
        }

        private void Budgets_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                Budget b = total.Budgets[e.NewIndex];
                dataGridView1.Rows.Insert(e.NewIndex, createNewRow(b.Name, b.ID));

                //dataGridView1.Rows.Add(createNewRow(b.Name, b.ID));
                 
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
                        if (i < total.Budgets.Count && total.Budgets[i].Sum)
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
                for (int i = 2; i < dataGridView1.ColumnCount; i++)
                {
                    calcSum(e.NewIndex, i);
                }
                updateStats();
            }
        }

        private void Departments_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                Department d = total.Departments[e.NewIndex];
                d.DepartmentChanged += new DepartmentChangedHandler(d_DepartmentChanged);
                dataGridView1.Columns.Add(d.Name, createDepHeader(d));
                dataGridView1.Columns[e.NewIndex + PERM_COLS].ReadOnly = true;
                dataGridView1.Columns[e.NewIndex + PERM_COLS].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[e.NewIndex + PERM_COLS].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns[e.NewIndex + PERM_COLS].ValueType = typeof(System.Double);
                dataGridView1.Columns[e.NewIndex + PERM_COLS].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    dataGridView1.Rows[i].Cells[dataGridView1.Columns.Count - 1].Value = sumDep(d, i);
                    calcSum(i, dataGridView1.Columns.Count - 1);
                    calcSum(i, 2);

                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        Color color = getIndexColor(j);
                        dataGridView1[j, i].Style.BackColor = total.Budgets[i].Income ? PortaflexColors.dimColor(color, -20) : color;
                    } 
                }
                
            }
            else if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                dataGridView1.Columns.RemoveAt(e.NewIndex + PERM_COLS);
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    dataGridView1.Rows[i].Cells[2].Value = sumRow(i);

                    for (int j = PERM_COLS; j < dataGridView1.ColumnCount; j++)
                    {
                        Color color = getIndexColor(j);
                        dataGridView1[j,i].Style.BackColor = total.Budgets[i].Income ? PortaflexColors.dimColor(color,-20) : color;
                    }    
                }

            }
            updateStats();
        }

        private void d_DepartmentChanged(object sender, DepartmentChangeEventArgs e)
        {
            Department dep = (Department) sender;
            int index = total.Departments.IndexOf(dep);
            if (e.Type == DepartmentChangeTypes.Name || e.Type == DepartmentChangeTypes.Proc)
            {                
                dataGridView1.Columns[index + PERM_COLS].Name = createDepHeader(dep);
                dataGridView1.Columns[index + PERM_COLS].HeaderText = createDepHeader(dep);
                if (e.Type == DepartmentChangeTypes.Proc)
                {
                    int i = 0;
                    foreach (Budget b in total.Budgets)
                    {
                        dataGridView1.Rows[i].Cells[index + PERM_COLS].Value = sumDep(dep, i);
                        if (b.Sum)
                            calcSum(i, index + PERM_COLS);
                        i++;
                    }
                }
            }
            else if (e.Type == DepartmentChangeTypes.Value)
            {
                int row = e.AffectedValueIndex;
                dataGridView1.Rows[row].Cells[2].Value = sumRow(row);
                calcSum(row, 2);
                dataGridView1.Rows[row].Cells[index + PERM_COLS].Value = sumDep(dep,row);
                calcSum(row, index + PERM_COLS);

            }
            else if (e.Type == DepartmentChangeTypes.Dir)
            {
                int i = 0;
                foreach (Budget b in total.Budgets)
                {
                    dataGridView1.Rows[i].Cells[2].Value = sumRow(i);
                    dataGridView1.Rows[i].Cells[index + PERM_COLS].Value = dep.TotalSum(i);
                    i++;
                }
            }
        }

        private void total_BudgetChanged(object sender, BudgetEventArgs e)
        {
            int index = e.Index;
            if (index < 0 || index >= dataGridView1.RowCount)
            {
                dataGridView1.Rows.Add(createNewRow(Texts.Budget, Texts.Id));
                index = index < 0 ? 0 : index;
            }
            dataGridView1.Rows[index].Cells[1].Value = ((Budget)sender).Name;
            dataGridView1.Rows[index].Cells[0].Value = ((Budget)sender).ID;
            if (((Budget)sender).Income)
                setRowColorAsIncome(dataGridView1.Rows[index], -20);
            else
                setRowColorAsExpense(dataGridView1.Rows[index]);
            if (e.Type == "income")
                updateStats();
        }

        #endregion

        #region UI event handlers

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
       /*     if(anError.RowIndex != dataGridView1.RowCount - 1)
                MessageBox.Show("Špatně zadaná hodnota, zadejte prosím číslo.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);*/
        }

        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            if (dataGridView1.Columns.IndexOf(e.Column) > 1)
            {
                formateNumberColumn(e.Column);
            }
        }

        private void sumButton_Click(object sender, EventArgs e)
        {
            total.Budgets.Add(new Budget(Texts.Sum, Texts.Id, true));    
        }

        private void newAcountButton_Click(object sender, EventArgs e)
        {
            total.Budgets.Add(new Budget(Texts.Budget, Texts.Id));
        }

        private void newDepButton_Click(object sender, EventArgs e)
        {
            Department d = new Department(total);
            DepartmentProperties prop = new DepartmentProperties(ref d, true, "Nové středisko");
            if (prop.ShowDialog() != DialogResult.OK)
                return;
            d.Locked = false;
            total.Departments.Add(d);
        }

        private void newDirButton_Click(object sender, EventArgs e)
        {
            total.Dir = new Directing();
        }

        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hinfo = dataGridView1.HitTest(e.X, e.Y);
                if (hinfo.Type == DataGridViewHitTestType.ColumnHeader && hinfo.ColumnIndex == 3 && total.Dir != null)
                {
                    selectedColumnIndex = hinfo.ColumnIndex;
                    columnContextMenu.Show(dataGridView1, new Point(e.X, e.Y));
                }
                else if (hinfo.Type == DataGridViewHitTestType.RowHeader)
                {
                    selectedRowIndex = hinfo.RowIndex;
                    dataGridView1.Rows[selectedRowIndex].Selected = true;
                    if (selectedRowIndex == dataGridView1.RowCount - 1)
                    {
                        totalContextMenu.Show(dataGridView1, new Point(e.X, e.Y));
                    }
                    else
                    {
                        if (total.Budgets[selectedRowIndex].Income)
                        {
                            incomeToolStripMenuItem.Text = Texts.Expense;
                            incomeToolStripMenuItem.Image = Portaflex.Properties.Resources.coins_delete_icon;
                        }
                        else
                        {
                            incomeToolStripMenuItem.Text = Texts.Income;
                            incomeToolStripMenuItem.Image = Portaflex.Properties.Resources.coins_add_icon;
                        }
                        rowContextMenu.Show(dataGridView1, new Point(e.X, e.Y));
                    }
                }
            }
        }

        // Smaze radek uctu
        private void smazatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex != -1)
            {
                List<int> list = new List<int>();
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                    list.Add(dataGridView1.SelectedRows[i].Index);
                foreach(int i in list)
                    total.Budgets.RemoveAt(i);
                selectedRowIndex = -1;
            }     
        }

        //nastavi prijem/vydaj
        private void incomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex != -1)
            {
                int i;
                bool income = !total.Budgets[selectedRowIndex].Income;
                for (i = selectedRowIndex - 1; i >= 0 && !total.Budgets[i].Sum; i--)
                    total.Budgets[i].Income = income;
                for (i = selectedRowIndex; i < dataGridView1.RowCount - 1 && !total.Budgets[i].Sum; i++)
                    total.Budgets[i].Income = income;
                if(i < dataGridView1.RowCount - 1)
                    total.Budgets[i].Income = income;
                selectedRowIndex = -1;
            }
        }

        //vlozi ucet
        private void insertBudgetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex != -1)
            {
                bool income = total.Budgets.Count > selectedRowIndex && total.Budgets[selectedRowIndex].Income;
                total.Budgets.Insert(selectedRowIndex, new Budget(Texts.Budget, Texts.Id, false, income));
            }
        }

        //vlozi soucet
        private void insertSumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex != -1)
            {
                bool income = selectedRowIndex > 0 && selectedRowIndex < total.Budgets.Count && total.Budgets[selectedRowIndex - 1].Income;
                total.Budgets.Insert(selectedRowIndex, new Budget(Texts.Sum, Texts.Id, true, income));
            }
        }

        //smaze rezii
        private void smazatToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            total.Dir = null;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            updateData(e.ColumnIndex, e.RowIndex);
        }
        #endregion

        #region Private helpers

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
            if (index < 3)
                return PortaflexColors.getColumnExpenseColor(0);
            if (index == 3)
                return PortaflexColors.getColumnExpenseColor(1);
            return PortaflexColors.getColumnExpenseColor(index - PERM_COLS + 2);
        }

        private void formateNumberColumn(DataGridViewColumn col)
        {
            col.DefaultCellStyle.Format = "N2";
            col.ValueType = typeof(System.Double);
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.DefaultCellStyle.NullValue = 0;
        }

        private void createTotalRow()
        {
            dataGridView1.Rows.Add(createNewRow("Σ", ""));
            DataGridViewRow row = dataGridView1.Rows[dataGridView1.RowCount - 1];
            
            row.ReadOnly = true;
            
            DataGridViewCellStyle s = new DataGridViewCellStyle();
            s.Font = new Font(DefaultFont.FontFamily, 10, FontStyle.Bold);
            s.Alignment = DataGridViewContentAlignment.MiddleRight;
            s.BackColor = Color.Gold ;//Color.FromArgb(255, 255, 192);
            s.Padding = new System.Windows.Forms.Padding(3);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle = s;
            row.Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private double calcNaklad()
        {
            double sum = 0;
            int i = 0;
            foreach (Budget b in total.Budgets)
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
            foreach (Budget b in total.Budgets)
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

        /// <summary>
        /// Spocte sumu pro budget sum ktery je nejblizsi zadanemu radku.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void calcSum(int row, int col)
        {
            if (row >= total.Budgets.Count || row < 0 || col >= dataGridView1.ColumnCount || col < 0)
                return;
            // najdeme nejblizsi nasledujici sum budget
            int rowSum;
            if (!total.Budgets[row].Sum)
            {
                for (int i = row; i < total.Budgets.Count; i++)
                {
                    if (total.Budgets[i].Sum)
                    {
                        row = i;
                        break;
                    }
                    // pokud neexistuje zadny nasledujici budget sum -> konec
                    else if (i + 1 == total.Budgets.Count)
                        return;
                }
            }
            rowSum = row;
            double sum = 0;
            for (row--; row >= 0 && !total.Budgets[row].Sum; row--)
            {
                sum += Convert.ToDouble(dataGridView1.Rows[row].Cells[col].Value);
            }
            dataGridView1.Rows[rowSum].Cells[col].Value = sum;
        }

        /// <summary>
        /// Vytvori retezec obsahujici jmeno a procenta predaneho oddeleni.
        /// </summary>
        /// <param name="d">Oddeleni z nehoz se ma retezec slozit</param>
        /// <returns>Vysledny string</returns>
        private string createDepHeader(Department d)
        {
            return d.Name + " (" + d.Proc + "%)";
        }

        /// <summary>
        /// Vytvori pole stringu pro vlozeni do dataGridView
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private string[] createNewRow(string name, string id)
        {
            string[] s = new string[dataGridView1.Columns.Count];
            s[0] = id;
            s[1] = name;
            s[2] = "0,00";
            s[3] = "0,00";
            for (int i = PERM_COLS; i < s.Length; i++)
                s[i] = "0,00";
            return s;
        }

        /// <summary>
        /// Aktualizuje data na zadanych souradnicich v dataGridView.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void updateData(int x, int y)
        {
            if (x < 0 || y < 0 || y >= dataGridView1.Rows.Count - 1)
                return;
            string value = dataGridView1.Rows[y].Cells[x].Value.ToString();
            if (x == 1) //name
            {
                total.Budgets[y].Name = value;
            }
            else if (x == 0) //id            
            {
                total.Budgets[y].ID = value;   
            }
            else if (x == 3 && total.Dir != null && !total.Budgets[y].Sum) //dir
            {
                total.Dir.Values[y] = Convert.ToDouble(value);
            }
            if (!total.Budgets[y].Sum)
            {
                dataGridView1.Rows[y].Cells[2].Value = sumRow(y);
                updateStats();
            }
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
                if (!total.Budgets[i].Sum)
                {
                    if (total.Budgets[i].Income)
                        sum += Convert.ToDouble(dataGridView1.Rows[i].Cells[col].Value);
                    else
                        sum -= Convert.ToDouble(dataGridView1.Rows[i].Cells[col].Value);
                }
            }
            return sum;
        }

        /// <summary>
        /// Vypocte soucet vsech oddeleni pro dany ucet.
        /// </summary>
        /// <param name="row">Index uctu</param>
        /// <returns>Suma oddeleni</returns>
        private double sumRow(int row)
        {
            double sum = 0;
            if (total.Dir != null && row < total.Dir.Values.Count)
                sum = total.Dir.Values[row];
            foreach (Department dep in total.Departments)
            {
                sum += dep.TotalSum(row);                
            }
            return sum;
        }

        /// <summary>
        /// Vytvori radek a naplni jej daty z predaneho uctu.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private DataGridViewRow fillRowWithData(Budget b)
        {
            DataGridViewRow row = new DataGridViewRow();
            DataGridViewTextBoxCell idcell = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell namecell = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell totalcell = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell dircell = new DataGridViewTextBoxCell();
            idcell.Value = b.ID;
            namecell.Value = b.Name;
            totalcell.Value = 0;
            if(total.Dir != null)
                dircell.Value = total.Dir.Values[total.Budgets.IndexOf(b)];
            else
                dircell.Value = 0;
            row.Cells.Add(idcell);
            row.Cells.Add(namecell);
            row.Cells.Add(totalcell);
            row.Cells.Add(dircell);
            return row;
        }

        /// <summary>
        /// Spocte soucet za cele oddeleni vcetne podilu na rezii
        /// </summary>
        /// <param name="d"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private double sumDep(Department d, int row)
        {
            double sum = d.TotalSum(row);
            if (total.Dir != null && total.Dir.Values.Count < row)
                sum += (total.Dir != null ? total.Dir.Values[row] * ((double)d.Proc) / 100 : 0);

            return sum;
        }

        #endregion

        #region Getters
        public DataGridView getData()
        {
            return dataGridView1;
        }
        #endregion

    }
}
