using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Portaflex.Data;
using System.IO;

namespace Portaflex
{
    public partial class Main : Form
    {
        private Total total;
        private TotalPage tp;
        private TabPage dirPage;
        private bool saved = false;
        private string path;
        private FileStream stream;

        public Main(string[] args)
        {
            InitializeComponent();
            //dataToolStripMenuItem.Visible = false;
            total = new Total();
            this.Icon = Properties.Resources.logo;
            this.Text = Texts.PrgName + " - " + Texts.NewTotal;
            total.DirChanged += total_DirChanged;
            total.Departments.ListChanged += Departments_ListChanged;
            createTotalPage();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            checkProcentual();

            if (args.Length > 0)
                openFile(args[0]);
        }

        private void total_DirChanged(object sender, EventArgs e)
        {
            manageDir(total.Dir == null);
        }

        /// <summary>
        /// Prida nebo odstrani zalozku Rezie.
        /// </summary>
        /// <param name="remove">Indikuje zda se ma zalozka pridat nebo odstranit.</param>
        private void manageDir(bool remove)
        {
            if (!remove)
            {
                dirPage = new TabPage(Texts.Dir);
                dirPage.ImageIndex = 3;
                createDirPage();
                insertPage(1, tabControl1, dirPage);
                checkProcentual();
                tabControl1.SelectedIndex = 0;
            }
            else if(dirPage != null)
            {
                tabControl1.TabPages.Remove(dirPage);
                dirPage = null;
            }
        }

        private void insertPage(int position, TabControl tc, TabPage tp)
        {
            var list = new List<TabPage>();
            var i = 0;
            foreach (TabPage p in tc.TabPages)
                list.Add(p);
            tc.TabPages.Clear();
            foreach (var p in list)
            {
                if (i++ == position)
                    tc.TabPages.Add(tp);
                tc.TabPages.Add(p);
            }
            tc.SelectedIndex = position;
        }

        private void createDirPage()
        {
            var dp = new DirPage(total);
            dp.Dock = DockStyle.Fill;
            dirPage.Controls.Clear();
            dirPage.Controls.Add(dp);
        }

        private void createTotalPage()
        {
            tp = new TotalPage(total);
            tp.Dock = DockStyle.Fill;
            totalPage.Controls.Clear();
            totalPage.Controls.Add(tp);
        }

        private void Departments_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                tabControl1.SelectedIndex = e.NewIndex;
                tabControl1.TabPages.RemoveAt(e.NewIndex + permPages());                
            }
            else if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                var d = total.Departments[e.NewIndex];
                insertTabPage(createTabPage(d));
                d.DepartmentChanged += departmentChanged;                
            }
            checkProcentual();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == addPage)
            {
                var d = new Department(total);                
                var prop = new DepartmentProperties(ref d, true, "Nové středisko");
                if (prop.ShowDialog() != DialogResult.OK)
                {
                    tabControl1.SelectedTab = tabControl1.TabPages[0];
                    return;
                }
                d.Locked = false;
                total.Departments.Add(d);
            }
        }

        private void departmentChanged(AbstractDepartment d, DepartmentChangeEventArgs e)
        {
            var page = tabControl1.TabPages[total.Departments.IndexOf((Department)d) + permPages()];
            page.Text = d.Name + " (" + d.Proc + "%)";
            if (((Department)d).Locked)
                page.ImageIndex = 1;
            else
                page.ImageIndex = 2;
            if (e.Type == DepartmentChangeTypes.Proc)
            {
                checkProcentual();
                ((BudgetPage)page.Controls[Texts.BudgetPage]).checkProcentual();
            }
        }

        private void checkProcentual()
        {
            if (total.Dir != null)
            {
                decimal counter = 0;
                foreach (var dep in total.Departments)
                    counter += dep.Proc;
                if (counter != 100 && total.Departments.Count > 0)
                {
                    dirPage.Text = Texts.Dir + Texts.DirErr;
                    dirPage.ImageIndex = 5;
                }
                else
                {
                    dirPage.Text = Texts.Dir;
                    dirPage.ImageIndex = 3;
                }
            }
        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            var selected = tabControl1.SelectedTab;
            
            if(selected != addPage && selected != totalPage)
            {
                if (e.Button == MouseButtons.Right)
                {
                    this.contextMenuStrip1.Show(this.tabControl1, e.Location);
                }
                if (e.Button == MouseButtons.Left)
                {
                    AbstractDepartment dep;
                    if (selected != dirPage)
                    {
                        var index = tabControl1.TabPages.IndexOf(selected) - permPages();
                        dep = total.Departments[index];
                    }
                    else
                        dep = total.Dir;
                    if (dep.Locked && dep.Password != "")
                    {
                        tabControl1.SelectedTab = totalPage;
                        while (true)
                        {
                            var form = new UnlockForm();
                            if (form.ShowDialog() == DialogResult.OK)
                            {
                                if (form.Password == dep.Password)
                                {
                                    tabControl1.SelectedTab = selected;
                                    dep.Locked = false;
                                    return;
                                }
                            }
                            else
                                return;
                        }
                    }
                }
            }
        }

        private void tabControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (tabControl1.SelectedTab != addPage && tabControl1.SelectedTab != totalPage)
            {
                propItem_Click(this, null);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            var p = this.tabControl1.PointToClient(Cursor.Position);
            for (var i = 0; i < this.tabControl1.TabCount; i++)
            {
                var r = this.tabControl1.GetTabRect(i);
                if (r.Contains(p))
                {
                    this.tabControl1.SelectedIndex = i; // i is the index of tab under cursor
                    contextMenuStrip1.Items.Clear();

                    if (this.tabControl1.SelectedTab != dirPage)
                    {
                        var expItem = contextMenuStrip1.Items.Add("Exportovat");
                        expItem.Click += expItem_Click;
                        var propItem = contextMenuStrip1.Items.Add("Vlastnosti");
                        propItem.Click += propItem_Click;
                        var delItem = contextMenuStrip1.Items.Add("Smazat");
                        delItem.Click += delItem_Click;
                    }
                    else
                    {
                        ToolStripItem lockItem;
                        if (total.Dir.Password == "")
                            lockItem = contextMenuStrip1.Items.Add("Zamknout");
                        else
                            lockItem = contextMenuStrip1.Items.Add("Odebrat zámek");
                        
                        lockItem.Click += dirLock_Click;
                    }

                    return;
                }
            }
            e.Cancel = true;
        }

        private void dirLock_Click(object sender, EventArgs e)
        {
            if (total.Dir.Password != "")
                total.Dir.Password = "";
            else
            {
                var dirlock = new DirLock(ref total);
                dirlock.ShowDialog();
            }
        }

        private void propItem_Click(object sender, EventArgs e)
        {
            var d = total.Departments[tabControl1.SelectedIndex - permPages()];
            var prop = new DepartmentProperties(ref d, false);
            prop.ShowDialog();
        }

        private void delItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != dirPage)
            {
                var d = total.Departments[tabControl1.SelectedIndex - permPages()];
                total.Departments.Remove(d);
            }
            else
            {
                total.Dir = null;
            }
        }

        private void expItem_Click(object sender, EventArgs e)
        {
            var d = total.Departments[tabControl1.SelectedIndex - permPages()];

            var expDialog = new SaveFileDialog();
            expDialog.Filter = "FLX soubor|*.flx";
            expDialog.Title = "Exportovat středisko";
            expDialog.AddExtension = true;
            if (expDialog.ShowDialog() == DialogResult.OK)
            {
                var ser = new XMLSerializer(expDialog.FileName);
                ser.SaveTotal(total);
                var t = ser.LoadTotal();

                var toRemove = new List<Department>();
                foreach (var dep in t.Departments)
                    if (dep.Name != d.Name)
                        toRemove.Add(dep);
                foreach (var dep in toRemove)
                    t.Departments.Remove(dep);

                ser.SaveTotal(t);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {                                             
            var openDialog = new OpenFileDialog();
            openDialog.Filter = "FLX file|*.flx";
            openDialog.Title = "Otevřít rozpočet";
            unlockStream();
            if (openDialog.ShowDialog() == DialogResult.OK)
            {/*
                XMLSerializer ser = new XMLSerializer(openDialog.FileName);
                path = openDialog.FileName;
                saved = true;

                total = ser.LoadTotal();
                //odstrani se vsechny dosavadni stranky stredisek
                for (int i = 1; i < tabControl1.TabCount - 1; )
                    tabControl1.TabPages.RemoveAt(i);

                total.Departments.ListChanged += new ListChangedEventHandler(Departments_ListChanged);
                total.DirChanged += new EventHandler(total_DirChanged);
                this.Text = Texts.PrgName + " - " + openDialog.SafeFileName.Substring(0, openDialog.SafeFileName.IndexOf("."));
                manageDir(total.Dir == null);
                createTotalPage();                
                populateTabs();
                tabControl1.SelectedTab = totalPage;*/
                openFile(openDialog.FileName);
            }
            lockStream(path);
        }

        private void openFile(string filename)
        {
            var ser = new XMLSerializer(filename);
            path = filename;
            saved = true;

            total = ser.LoadTotal();
            //odstrani se vsechny dosavadni stranky stredisek
            for (var i = 1; i < tabControl1.TabCount - 1; )
                tabControl1.TabPages.RemoveAt(i);

            total.Departments.ListChanged += Departments_ListChanged;
            total.DirChanged += total_DirChanged;
            var start = filename.LastIndexOf('\\') + 1;
            var length = filename.IndexOf(".") - start;
            if (start >= 0 && length > 0)
                this.Text = Texts.PrgName + " - " + filename.Substring(start, length);
            else
                this.Text = Texts.PrgName;
            manageDir(total.Dir == null);
            createTotalPage();
            populateTabs();
            tabControl1.SelectedTab = totalPage;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saved)
            {
                unlockStream();
                var ser = new XMLSerializer(path);
                ser.SaveTotal(total);
                lockStream(path);
            }
            else
            {
                saveAsToolStripMenuItem_Click(this, null);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = "FLX soubor|*.flx";
            saveDialog.Title = "Uložit rozpočet";
            saveDialog.AddExtension = true;
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                var ser = new XMLSerializer(saveDialog.FileName);
                ser.SaveTotal(total);
                saved = true;
                if (stream != null)
                    stream.Close();                
                this.path = saveDialog.FileName;
                lockStream(this.path);
                var path = new Uri(saveDialog.FileName);
                var filename = path.Segments[path.Segments.Length - 1];
                this.Text = Texts.PrgName + " - " + filename.Substring(0, filename.IndexOf("."));
            }
        }

        private void populateTabs()
        {

            foreach (var d in total.Departments)
            {
                d.DepartmentChanged += departmentChanged;
               insertTabPage(createTabPage(d));
            }
        }

        private TabPage createTabPage(Department d)
        {
            var newtab = new TabPage(d.Name + " (" + d.Proc + "%)");
            if (d.Locked && d.Password != "")
                newtab.ImageIndex = 1;
            else
                newtab.ImageIndex = 2;
            var bp = new BudgetPage(d,newtab);
            bp.Dock = DockStyle.Fill;
            newtab.Controls.Add(bp);
            return newtab;
        }

        private void insertTabPage(TabPage page)
        {
            tabControl1.TabPages.Remove(addPage);
            tabControl1.TabPages.Add(page);
            tabControl1.SelectedTab = page;
            tabControl1.TabPages.Add(addPage);
        }

        /// <summary>
        /// Vrati pocet permanentnich stranek v zavislosti na tom jestli existuje zalozka s rezii.
        /// </summary>
        /// <returns></returns>
        private int permPages()
        {
            if (total.Dir != null)
                return 2;
            else
                return 1;
        }

        private void readExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Filter = "XLS soubor|*.xls|XLSX soubor|*.xlsx";
            openDialog.Title = "Otevřít excel";
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                var args = new Dictionary<string, object>();
                args.Add("total", total);
                args.Add("path", openDialog.FileName);
                var workerDialog = new BackgroundWorkDialog(readExcel_DoWork, readExcel_RunWorkerCompleted, args);
                workerDialog.ShowDialog();
            }
        }

        private void readExcel_DoWork(object sender, DoWorkEventArgs e)
        {
            var args = e.Argument as Dictionary<string, object>;
            var total = args["total"] as Total;
            var path = args["path"] as string;
            var list = XLSWorker.readBudgets(total, path);
            args.Add("budgetlist", list);
            e.Result = args;
        }

        private void readExcel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var args = e.Result as Dictionary<string, object>;
            var list = args["budgetlist"] as List<Budget>;
            foreach (var b in list)
                total.Budgets.Add(b);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            total = new Total();
            unlockStream();
            //odstrani se vsechny dosavani stranky stredisek
            for (var i = 1; i < tabControl1.TabCount - 1; )
                tabControl1.TabPages.RemoveAt(i);
            this.Text = Texts.PrgName + " - " + Texts.NewTotal;
            total.Departments.ListChanged += Departments_ListChanged;
            total.DirChanged += total_DirChanged;
            createTotalPage();
            path = "";
            saved = false;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            var close = saveBeforeQuit();
            if (!close)
                e.Cancel = true;
            else
                unlockStream();
        }

        private void unlockStream()
        {
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }
        }

        private void lockStream(string path)
        {
            if(path != null)
                stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
        }

        private void readDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Filter = "XLS soubor|*.xls|XLSX soubor|*.xlsx";
            openDialog.Title = "Otevřít excel";
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                var args = new Dictionary<string,object>();
                args.Add("total",total);
                args.Add("path", openDialog.FileName);
                var workDialog = new BackgroundWorkDialog(readData_DoWork, readData_RunWorkerCompleted, args);
                workDialog.ShowDialog();
            }
        }

        private void readData_DoWork(object sender, DoWorkEventArgs e)
        {
            var args = e.Argument as Dictionary<string, object>;
            var total = args["total"] as Total;
            var path = args["path"] as string;
            var list = XLSWorker.readBudgets(total, path);
            var complete = new List<Budget>();
            complete.AddRange(total.Budgets.ToList());
            complete.AddRange(list);
            var sub = XLSWorker.readData(complete, path);
            args.Add("budgetlist", list);
            args.Add("newsubdep", sub);
            e.Result = args;
        }

        private void readData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var args = e.Result as Dictionary<string, object>;
            var list = args["budgetlist"] as List<Budget>;
            var sub = args["newsubdep"] as SubDepartment;

            foreach (var b in list)
                total.Budgets.Add(b);

            sub.Name = "Nové podstředisko";
            var d = new Department(total);
            d.Locked = false;
            d.Password = "";
            d.Name = "Nové středisko";
            d.SubDepartments.Add(sub);
            total.Departments.Add(d); 
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = "XLS soubor|*.xls|XLSX soubor|*.xlsx";
            saveDialog.Title = "Exportovat";
            saveDialog.AddExtension = true;
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                var filename = saveDialog.FileName;
                if (IsFileLocked(filename))
                {
                    MessageBox.Show("Soubor " + filename.Substring(filename.LastIndexOf('\\') + 1) + " je používán jiným procesem.", "Chyba při exportu", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                var list = new List<DataGridView>();
                list.Add(tp.getData());
                foreach (TabPage page in tabControl1.TabPages)
                {
                    foreach (Control c in page.Controls)
                    {
                        if (c is BudgetPage)
                            list.Add(((BudgetPage)c).getData());
                    }
                }
                var path = saveDialog.FileName;
                var args = new Dictionary<string, object>();
                args.Add("total", total);
                args.Add("list", list);
                args.Add("path", path);
                var label = "Probíhá export do Excelu...";
                var workDialog = new BackgroundWorkDialog(export_DoWork, export_RunWorkerCompleted, args, "Export", label);
                workDialog.ShowDialog();
                
            }
        }

        private void export_DoWork(object sender, DoWorkEventArgs e)
        {
            var args = e.Argument as Dictionary<string, object>;
            var total = args["total"] as Total;
            var path = args["path"] as string;
            var list = args["list"] as List<DataGridView>;
            XLSWorker.writeData(total, list, path);
        }

        private void export_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // empty
        }

        private bool IsFileLocked(String path)
        {
            var file = new FileInfo(path);
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return file.Exists;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        private void importDepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var importDialog = new OpenFileDialog();
            importDialog.Filter = "FLX file|*.flx";
            importDialog.Title = "Importovat střediska";
            if (importDialog.ShowDialog() == DialogResult.OK)
            {
                var ser = new XMLSerializer(importDialog.FileName);
                var t = ser.LoadTotal();
                total.Dir.Password = t.Dir.Password;
                foreach (var b in t.Budgets)
                {
                    if(!total.ContainsBudget(b.ID, b.Name))
                    {
                        total.Budgets.Add(b);
                    }
                    var i = total.BudgetIndex(b.ID, b.Name);
                    if (i >= 0 && total.Dir.Values[i] == 0)
                    {
                        total.Dir.Values[i] = t.Dir.Values[t.Budgets.IndexOf(b)];
                    }
               
                }
                foreach (var d in t.Departments)
                {
                    var newDep = new Department(total);
                    
                    newDep.Name = d.Name;
                    newDep.Locked = d.Locked;
                    newDep.Password = d.Password;
                    newDep.Proc = d.Proc;

                    foreach (var sub in d.SubDepartments)
                    {
                        newDep.SubDepartments.Add(sub);
                        foreach (var b in t.Budgets)
                        {
                            var from = t.Budgets.IndexOf(b);
                            var to = total.BudgetIndex(b.ID, b.Name);
                            if(to >= 0 && from >= 0 && to != from)
                            {
                                sub.Values[to] = sub.Values[from];
                                sub.Values[from] = 0;
                            }
                        }                        
                    }
                  
                    total.Departments.Add(newDep);
                    
                }
            }
        }

        private void importSubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == addPage || tabControl1.SelectedTab == totalPage || tabControl1.SelectedTab == dirPage)
                return;

            var department = total.Departments[tabControl1.SelectedIndex - permPages()];
            var importDialog = new OpenFileDialog();
            importDialog.Filter = "FLX file|*.flx";
            importDialog.Title = "Importovat podstřediska";
            if (importDialog.ShowDialog() == DialogResult.OK)
            {
                var ser = new XMLSerializer(importDialog.FileName);
                var t = ser.LoadTotal();
                if (t.Departments.Count < 1 || t.Departments[0].SubDepartments.Count < 1)
                    return;
                foreach (var b in t.Budgets)
                {
                    if (!total.ContainsBudget(b.ID, b.Name))
                    {
                        total.Budgets.Add(b);
                    }
                    var i = total.BudgetIndex(b.ID, b.Name);
                    if (i >= 0 && total.Dir.Values[i] == 0)
                    {
                        total.Dir.Values[i] = t.Dir.Values[t.Budgets.IndexOf(b)];
                    }
                }

                var sub = t.Departments[0].SubDepartments[0];
                department.SubDepartments.Add(sub);
                foreach (var b in t.Budgets)
                {
                    var from = t.Budgets.IndexOf(b);
                    var to = total.BudgetIndex(b.ID, b.Name);
                    if (to >= 0 && from >= 0 && to != from)
                    {
                        sub.Values[to] = sub.Values[from];
                        sub.Values[from] = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Zepta se uzivatele zda chce soubor ulozit, pokud ano, ulozi jej/
        /// </summary>
        /// <returns>true pokud se ma pokracovat v ukonceni programu</returns>
        private bool saveBeforeQuit()
        {
            var res = MessageBox.Show("Soubor nebyl uložen, chcete jej uložit?", "Portaflex", MessageBoxButtons.YesNoCancel);
            if(res == DialogResult.Cancel)
                return false;
            if (res == DialogResult.Yes)
                saveToolStripMenuItem_Click(this, null);
            return true;            
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            PortaflexColors.DrawTab(sender, e);
        }

    }
}
