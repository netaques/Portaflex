namespace Portaflex
{
    partial class TotalPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TotalPage));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.vysledekTextLabel = new System.Windows.Forms.Label();
            this.obratTextLabel = new System.Windows.Forms.Label();
            this.vysledekLabel = new System.Windows.Forms.Label();
            this.obratLabel = new System.Windows.Forms.Label();
            this.rowContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.insertBudgetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertSumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.incomeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smazatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.columnContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.smazatToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.totalContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addBudgetMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSumMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nakladTextLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Režie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.rowContextMenu.SuspendLayout();
            this.columnContextMenu.SuspendLayout();
            this.totalContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(910, 417);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.name,
            this.totalColumn,
            this.Režie});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 5);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(904, 334);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnAdded);
            this.dataGridView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.nakladTextLabel);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.vysledekTextLabel);
            this.panel1.Controls.Add(this.obratTextLabel);
            this.panel1.Controls.Add(this.vysledekLabel);
            this.panel1.Controls.Add(this.obratLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 342);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(910, 75);
            this.panel1.TabIndex = 4;
            // 
            // vysledekTextLabel
            // 
            this.vysledekTextLabel.AutoSize = true;
            this.vysledekTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.vysledekTextLabel.ForeColor = System.Drawing.SystemColors.Highlight;
            this.vysledekTextLabel.Location = new System.Drawing.Point(186, 51);
            this.vysledekTextLabel.Name = "vysledekTextLabel";
            this.vysledekTextLabel.Size = new System.Drawing.Size(37, 16);
            this.vysledekTextLabel.TabIndex = 3;
            this.vysledekTextLabel.Text = "0 Kč";
            // 
            // obratTextLabel
            // 
            this.obratTextLabel.AutoSize = true;
            this.obratTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.obratTextLabel.ForeColor = System.Drawing.SystemColors.Highlight;
            this.obratTextLabel.Location = new System.Drawing.Point(186, 6);
            this.obratTextLabel.Name = "obratTextLabel";
            this.obratTextLabel.Size = new System.Drawing.Size(37, 16);
            this.obratTextLabel.TabIndex = 2;
            this.obratTextLabel.Text = "0 Kč";
            // 
            // vysledekLabel
            // 
            this.vysledekLabel.AutoSize = true;
            this.vysledekLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.vysledekLabel.Location = new System.Drawing.Point(3, 51);
            this.vysledekLabel.Name = "vysledekLabel";
            this.vysledekLabel.Size = new System.Drawing.Size(172, 16);
            this.vysledekLabel.TabIndex = 1;
            this.vysledekLabel.Text = "Hospodářský výsledek:";
            // 
            // obratLabel
            // 
            this.obratLabel.AutoSize = true;
            this.obratLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.obratLabel.Location = new System.Drawing.Point(3, 6);
            this.obratLabel.Name = "obratLabel";
            this.obratLabel.Size = new System.Drawing.Size(108, 16);
            this.obratLabel.TabIndex = 0;
            this.obratLabel.Text = "Celkový obrat:";
            // 
            // rowContextMenu
            // 
            this.rowContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertBudgetToolStripMenuItem,
            this.insertSumToolStripMenuItem,
            this.incomeToolStripMenuItem,
            this.smazatToolStripMenuItem});
            this.rowContextMenu.Name = "rowContextMenu";
            this.rowContextMenu.Size = new System.Drawing.Size(142, 92);
            // 
            // insertBudgetToolStripMenuItem
            // 
            this.insertBudgetToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("insertBudgetToolStripMenuItem.Image")));
            this.insertBudgetToolStripMenuItem.Name = "insertBudgetToolStripMenuItem";
            this.insertBudgetToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.insertBudgetToolStripMenuItem.Text = "Vložit účet";
            this.insertBudgetToolStripMenuItem.Click += new System.EventHandler(this.insertBudgetToolStripMenuItem_Click);
            // 
            // insertSumToolStripMenuItem
            // 
            this.insertSumToolStripMenuItem.Image = global::Portaflex.Properties.Resources.table_sum;
            this.insertSumToolStripMenuItem.Name = "insertSumToolStripMenuItem";
            this.insertSumToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.insertSumToolStripMenuItem.Text = "Vložit součet";
            this.insertSumToolStripMenuItem.Click += new System.EventHandler(this.insertSumToolStripMenuItem_Click);
            // 
            // incomeToolStripMenuItem
            // 
            this.incomeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("incomeToolStripMenuItem.Image")));
            this.incomeToolStripMenuItem.Name = "incomeToolStripMenuItem";
            this.incomeToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.incomeToolStripMenuItem.Text = "Příjem";
            this.incomeToolStripMenuItem.Click += new System.EventHandler(this.incomeToolStripMenuItem_Click);
            // 
            // smazatToolStripMenuItem
            // 
            this.smazatToolStripMenuItem.Image = global::Portaflex.Properties.Resources.erase;
            this.smazatToolStripMenuItem.Name = "smazatToolStripMenuItem";
            this.smazatToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.smazatToolStripMenuItem.Text = "Smazat";
            this.smazatToolStripMenuItem.Click += new System.EventHandler(this.smazatToolStripMenuItem_Click);
            // 
            // columnContextMenu
            // 
            this.columnContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smazatToolStripMenuItem1});
            this.columnContextMenu.Name = "columnContextMenu";
            this.columnContextMenu.Size = new System.Drawing.Size(113, 26);
            // 
            // smazatToolStripMenuItem1
            // 
            this.smazatToolStripMenuItem1.Image = global::Portaflex.Properties.Resources.erase;
            this.smazatToolStripMenuItem1.Name = "smazatToolStripMenuItem1";
            this.smazatToolStripMenuItem1.Size = new System.Drawing.Size(112, 22);
            this.smazatToolStripMenuItem1.Text = "Smazat";
            this.smazatToolStripMenuItem1.Click += new System.EventHandler(this.smazatToolStripMenuItem1_Click);
            // 
            // totalContextMenu
            // 
            this.totalContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addBudgetMenuItem,
            this.addSumMenuItem});
            this.totalContextMenu.Name = "totalContextMenu";
            this.totalContextMenu.Size = new System.Drawing.Size(142, 48);
            // 
            // addBudgetMenuItem
            // 
            this.addBudgetMenuItem.Image = global::Portaflex.Properties.Resources.add_icon;
            this.addBudgetMenuItem.Name = "addBudgetMenuItem";
            this.addBudgetMenuItem.Size = new System.Drawing.Size(141, 22);
            this.addBudgetMenuItem.Text = "Vložit účet";
            this.addBudgetMenuItem.Click += new System.EventHandler(this.insertBudgetToolStripMenuItem_Click);
            // 
            // addSumMenuItem
            // 
            this.addSumMenuItem.Image = global::Portaflex.Properties.Resources.table_sum;
            this.addSumMenuItem.Name = "addSumMenuItem";
            this.addSumMenuItem.Size = new System.Drawing.Size(141, 22);
            this.addSumMenuItem.Text = "Vložit součet";
            this.addSumMenuItem.Click += new System.EventHandler(this.insertSumToolStripMenuItem_Click);
            // 
            // nakladTextLabel
            // 
            this.nakladTextLabel.AutoSize = true;
            this.nakladTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.nakladTextLabel.ForeColor = System.Drawing.SystemColors.Highlight;
            this.nakladTextLabel.Location = new System.Drawing.Point(186, 28);
            this.nakladTextLabel.Name = "nakladTextLabel";
            this.nakladTextLabel.Size = new System.Drawing.Size(37, 16);
            this.nakladTextLabel.TabIndex = 5;
            this.nakladTextLabel.Text = "0 Kč";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(3, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Celkový náklad:";
            // 
            // ID
            // 
            this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ID.FillWeight = 137.0558F;
            this.ID.HeaderText = "Číslo účtu";
            this.ID.Name = "ID";
            this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ID.Width = 90;
            // 
            // name
            // 
            this.name.FillWeight = 81.47208F;
            this.name.HeaderText = "Název účtu";
            this.name.Name = "name";
            // 
            // totalColumn
            // 
            this.totalColumn.FillWeight = 81.47208F;
            this.totalColumn.HeaderText = "Celkem";
            this.totalColumn.Name = "totalColumn";
            this.totalColumn.ReadOnly = true;
            // 
            // Režie
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(234)))), ((int)(((byte)(185)))));
            this.Režie.DefaultCellStyle = dataGridViewCellStyle1;
            this.Režie.HeaderText = "Firemní režie";
            this.Režie.Name = "Režie";
            this.Režie.ReadOnly = true;
            this.Režie.Visible = false;
            // 
            // TotalPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TotalPage";
            this.Size = new System.Drawing.Size(910, 417);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.rowContextMenu.ResumeLayout(false);
            this.columnContextMenu.ResumeLayout(false);
            this.totalContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip rowContextMenu;
        private System.Windows.Forms.ToolStripMenuItem smazatToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip columnContextMenu;
        private System.Windows.Forms.ToolStripMenuItem smazatToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem incomeToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label vysledekTextLabel;
        private System.Windows.Forms.Label obratTextLabel;
        private System.Windows.Forms.Label vysledekLabel;
        private System.Windows.Forms.Label obratLabel;
        private System.Windows.Forms.ToolStripMenuItem insertBudgetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertSumToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip totalContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addBudgetMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSumMenuItem;
        private System.Windows.Forms.Label nakladTextLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Režie;
    }
}
