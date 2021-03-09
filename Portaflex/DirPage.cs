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
    public partial class DirPage : UserControl
    {
        private Total total;
        private Directing dir;

        public DirPage(Total t)
        {
            if (t == null || t.Dir == null)
                return;
            InitializeComponent();
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null,
            dataGridView1, new object[] { true });
            dataGridView1.DataError += new DataGridViewDataErrorEventHandler(dataGridView1_DataError);
            InitializeData(t);
            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
            DataGridViewColumn col = dataGridView1.Columns[2];
            col.DefaultCellStyle.Format = "N2";
            col.ValueType = typeof(System.Double);
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.DefaultCellStyle.NullValue = 0;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            MessageBox.Show("Špatně zadaná hodnota, zadejte prosím číslo.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void InitializeData(Total t)
        {
            total = t;
            dir = t.Dir;

            total.BudgetChanged += new BudgetChangedHandler(total_BudgetChanged);
            total.Budgets.ListChanged += new ListChangedEventHandler(Budgets_ListChanged);
            dir.Values.ListChanged += new ListChangedEventHandler(DirValues_ListChanged);

            foreach (Budget b in total.Budgets)
            {
                int index = total.Budgets.IndexOf(b);
                dataGridView1.Rows.Add(createNewRow(b.Name, b.ID, dir.Values[index]));
                if (b.Sum)
                {
                    DataGridViewCellStyle s = new DataGridViewCellStyle();
                    s.Font = new Font(DefaultFont, FontStyle.Bold);
                    dataGridView1.Rows[index].DefaultCellStyle = s;
                    dataGridView1.Rows[index].Cells[2].ReadOnly = true;
                    calcSum(index, 2); 
                }

                setRowColor(b.Income, dataGridView1.Rows[index]);
            }
            createTotalRow();
            updateStats();
        }

        private void DirValues_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                dataGridView1.Rows[e.NewIndex].Cells[2].Value = dir.Values[e.NewIndex];
                calcSum(e.NewIndex, 2);
                updateStats();
            }
        }

        private void Budgets_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                Budget b = total.Budgets[e.NewIndex];
                dataGridView1.Rows.Insert(e.NewIndex,createNewRow(b.Name, b.ID, 0));

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
                    for (int i = e.NewIndex + 1; i < dataGridView1.RowCount - 1; i++)
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

                setRowColor(b.Income, dataGridView1.Rows[e.NewIndex]);
            }
            else if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                dataGridView1.Rows.RemoveAt(e.NewIndex);
                calcSum(e.NewIndex, 2);
            }
            updateStats();
        }

        private void total_BudgetChanged(object sender, BudgetEventArgs e)
        {
            int index = e.Index;
            if (index < 0 || index >= dataGridView1.RowCount - 1)
            {
                dataGridView1.Rows.Add(createNewRow(Texts.Budget, Texts.Id, 0));
                index = index < 0 ? 0 : index;
            }
            dataGridView1.Rows[index].Cells[1].Value = ((Budget)sender).Name;
            dataGridView1.Rows[index].Cells[0].Value = ((Budget)sender).ID;

            setRowColor(((Budget)sender).Income, dataGridView1.Rows[index]);
            updateStats();
        }

        private void setRowColor(bool income, DataGridViewRow row)
        {
            if (income)
                setRowColorAsIncome(row, -20);
            else
                setRowColorAsExpense(row);
        }

        private void setRowColorAsIncome(DataGridViewRow row, int dim)
        {
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                row.Cells[i].Style.BackColor = PortaflexColors.dimColor(PortaflexColors.getColumnExpenseColor(1), dim);
            }
        }

        private void setRowColorAsExpense(DataGridViewRow row)
        {
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                row.Cells[i].Style.BackColor = PortaflexColors.getColumnExpenseColor(1);
            }
        }

        /// <summary>
        /// Vytvori pole stringu pro vlozeni do dataGridView
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private string[] createNewRow(string name, string id, double val)
        {
            string[] s = new string[dataGridView1.Columns.Count];
            s[0] = id;
            s[1] = name;
            s[2] = val.ToString("N2");
            return s;
        }

        private void newAcountButton_Click(object sender, EventArgs e)
        {
            total.Budgets.Add(new Budget(Texts.Budget, Texts.Id));
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            updateData(e.ColumnIndex, e.RowIndex);
        }

        /// <summary>
        /// Spocte sumu pro budget sum ktery je nejblizsi zadanemu radku.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void calcSum(int row, int col)
        {
            // najdeme nejblizsi nasledujici sum budget
            int rowSum;
            if (row >= total.Budgets.Count || row < 0 || col >= dataGridView1.ColumnCount || col < 0)
                return;
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
            else if (x == 2 && total.Dir != null && !total.Budgets[y].Sum) //dir
            {
                total.Dir.Values[y] = Convert.ToDouble(value);
                updateStats();
            }
        }

        private void newSumButton_Click(object sender, EventArgs e)
        {
            total.Budgets.Add(new Budget(Texts.Sum, Texts.Id, true));
        }

        private void createTotalRow()
        {
            dataGridView1.Rows.Add(createNewRow("Σ", "", 0));
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
    }
}
