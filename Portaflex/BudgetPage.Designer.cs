namespace Portaflex
{
    partial class BudgetPage
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BudgetPage));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.newSubButton = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.newDirButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.collapseAllButton = new System.Windows.Forms.Button();
            this.expandAllButton = new System.Windows.Forms.Button();
            this.columnContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smazatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimaxiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rowContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.smazatToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.innerDirContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.nakladTextLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.vysledekTextLabel = new System.Windows.Forms.Label();
            this.obratTextLabel = new System.Windows.Forms.Label();
            this.vysledekLabel = new System.Windows.Forms.Label();
            this.obratLabel = new System.Windows.Forms.Label();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InnerDir = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.columnContextMenu.SuspendLayout();
            this.rowContextMenu.SuspendLayout();
            this.innerDirContextMenu.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(963, 487);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.name,
            this.total,
            this.InnerDir});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 33);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(957, 376);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnAdded);
            this.dataGridView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseUp);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel2, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(963, 30);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.newSubButton);
            this.flowLayoutPanel1.Controls.Add(this.newDirButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(481, 30);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // newSubButton
            // 
            this.newSubButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.newSubButton.ImageIndex = 0;
            this.newSubButton.ImageList = this.imageList;
            this.newSubButton.Location = new System.Drawing.Point(3, 3);
            this.newSubButton.Name = "newSubButton";
            this.newSubButton.Size = new System.Drawing.Size(142, 23);
            this.newSubButton.TabIndex = 8;
            this.newSubButton.Text = "Přidat podstředisko";
            this.newSubButton.UseVisualStyleBackColor = true;
            this.newSubButton.Click += new System.EventHandler(this.newSubButton_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "1345922240_add.png");
            this.imageList.Images.SetKeyName(1, "lock-6.png");
            this.imageList.Images.SetKeyName(2, "un-lock.png");
            this.imageList.Images.SetKeyName(3, "1345922841_coins.png");
            this.imageList.Images.SetKeyName(4, "1345922571_table_money.png");
            this.imageList.Images.SetKeyName(5, "bonus-icon2.png");
            this.imageList.Images.SetKeyName(6, "expandall.gif");
            this.imageList.Images.SetKeyName(7, "1362835077_collapseall.gif");
            // 
            // newDirButton
            // 
            this.newDirButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.newDirButton.ImageIndex = 3;
            this.newDirButton.ImageList = this.imageList;
            this.newDirButton.Location = new System.Drawing.Point(151, 3);
            this.newDirButton.Name = "newDirButton";
            this.newDirButton.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.newDirButton.Size = new System.Drawing.Size(138, 23);
            this.newDirButton.TabIndex = 10;
            this.newDirButton.Text = "Středisková režie";
            this.newDirButton.UseVisualStyleBackColor = true;
            this.newDirButton.Click += new System.EventHandler(this.newDirButton_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.collapseAllButton);
            this.flowLayoutPanel2.Controls.Add(this.expandAllButton);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(481, 0);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(482, 30);
            this.flowLayoutPanel2.TabIndex = 5;
            // 
            // collapseAllButton
            // 
            this.collapseAllButton.ImageIndex = 7;
            this.collapseAllButton.ImageList = this.imageList;
            this.collapseAllButton.Location = new System.Drawing.Point(456, 3);
            this.collapseAllButton.Name = "collapseAllButton";
            this.collapseAllButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.collapseAllButton.Size = new System.Drawing.Size(23, 23);
            this.collapseAllButton.TabIndex = 12;
            this.collapseAllButton.UseVisualStyleBackColor = true;
            this.collapseAllButton.Click += new System.EventHandler(this.collapseAllButton_Click);
            // 
            // expandAllButton
            // 
            this.expandAllButton.ImageIndex = 6;
            this.expandAllButton.ImageList = this.imageList;
            this.expandAllButton.Location = new System.Drawing.Point(427, 3);
            this.expandAllButton.Name = "expandAllButton";
            this.expandAllButton.Size = new System.Drawing.Size(23, 23);
            this.expandAllButton.TabIndex = 11;
            this.expandAllButton.UseVisualStyleBackColor = true;
            this.expandAllButton.Click += new System.EventHandler(this.expandAllButton_Click);
            // 
            // columnContextMenu
            // 
            this.columnContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem,
            this.smazatToolStripMenuItem,
            this.minimaxiToolStripMenuItem,
            this.exportSubToolStripMenuItem});
            this.columnContextMenu.Name = "contextMenuStrip1";
            this.columnContextMenu.Size = new System.Drawing.Size(150, 92);
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.menuToolStripMenuItem.Text = "Vlastnosti";
            this.menuToolStripMenuItem.Click += new System.EventHandler(this.propToolStripMenuItem_Click);
            // 
            // smazatToolStripMenuItem
            // 
            this.smazatToolStripMenuItem.Name = "smazatToolStripMenuItem";
            this.smazatToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.smazatToolStripMenuItem.Text = "Smazat";
            this.smazatToolStripMenuItem.Click += new System.EventHandler(this.delColToolStripMenuItem_Click);
            // 
            // minimaxiToolStripMenuItem
            // 
            this.minimaxiToolStripMenuItem.Name = "minimaxiToolStripMenuItem";
            this.minimaxiToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.minimaxiToolStripMenuItem.Text = "Minimalizovat";
            this.minimaxiToolStripMenuItem.Click += new System.EventHandler(this.minimaxiToolStripMenuItem_Click);
            // 
            // exportSubToolStripMenuItem
            // 
            this.exportSubToolStripMenuItem.Name = "exportSubToolStripMenuItem";
            this.exportSubToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.exportSubToolStripMenuItem.Text = "Exportovat";
            this.exportSubToolStripMenuItem.Click += new System.EventHandler(this.exportSubToolStripMenuItem_Click);
            // 
            // rowContextMenu
            // 
            this.rowContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smazatToolStripMenuItem1});
            this.rowContextMenu.Name = "rowContextMenu";
            this.rowContextMenu.Size = new System.Drawing.Size(113, 26);
            // 
            // smazatToolStripMenuItem1
            // 
            this.smazatToolStripMenuItem1.Name = "smazatToolStripMenuItem1";
            this.smazatToolStripMenuItem1.Size = new System.Drawing.Size(112, 22);
            this.smazatToolStripMenuItem1.Text = "Smazat";
            this.smazatToolStripMenuItem1.Click += new System.EventHandler(this.delRowToolStripMenuItem_Click);
            // 
            // innerDirContextMenu
            // 
            this.innerDirContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cToolStripMenuItem});
            this.innerDirContextMenu.Name = "innerDirContextMenu";
            this.innerDirContextMenu.Size = new System.Drawing.Size(113, 26);
            // 
            // cToolStripMenuItem
            // 
            this.cToolStripMenuItem.Name = "cToolStripMenuItem";
            this.cToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.cToolStripMenuItem.Text = "Smazat";
            this.cToolStripMenuItem.Click += new System.EventHandler(this.delDirToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.nakladTextLabel);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.vysledekTextLabel);
            this.panel1.Controls.Add(this.obratTextLabel);
            this.panel1.Controls.Add(this.vysledekLabel);
            this.panel1.Controls.Add(this.obratLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 412);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(963, 75);
            this.panel1.TabIndex = 4;
            // 
            // nakladTextLabel
            // 
            this.nakladTextLabel.AutoSize = true;
            this.nakladTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.nakladTextLabel.ForeColor = System.Drawing.SystemColors.Highlight;
            this.nakladTextLabel.Location = new System.Drawing.Point(186, 28);
            this.nakladTextLabel.Name = "nakladTextLabel";
            this.nakladTextLabel.Size = new System.Drawing.Size(37, 16);
            this.nakladTextLabel.TabIndex = 11;
            this.nakladTextLabel.Text = "0 Kč";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(3, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "Celkový náklad:";
            // 
            // vysledekTextLabel
            // 
            this.vysledekTextLabel.AutoSize = true;
            this.vysledekTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.vysledekTextLabel.ForeColor = System.Drawing.SystemColors.Highlight;
            this.vysledekTextLabel.Location = new System.Drawing.Point(186, 52);
            this.vysledekTextLabel.Name = "vysledekTextLabel";
            this.vysledekTextLabel.Size = new System.Drawing.Size(37, 16);
            this.vysledekTextLabel.TabIndex = 9;
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
            this.obratTextLabel.TabIndex = 8;
            this.obratTextLabel.Text = "0 Kč";
            // 
            // vysledekLabel
            // 
            this.vysledekLabel.AutoSize = true;
            this.vysledekLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.vysledekLabel.Location = new System.Drawing.Point(3, 52);
            this.vysledekLabel.Name = "vysledekLabel";
            this.vysledekLabel.Size = new System.Drawing.Size(172, 16);
            this.vysledekLabel.TabIndex = 7;
            this.vysledekLabel.Text = "Hospodářský výsledek:";
            // 
            // obratLabel
            // 
            this.obratLabel.AutoSize = true;
            this.obratLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.obratLabel.Location = new System.Drawing.Point(3, 6);
            this.obratLabel.Name = "obratLabel";
            this.obratLabel.Size = new System.Drawing.Size(108, 16);
            this.obratLabel.TabIndex = 6;
            this.obratLabel.Text = "Celkový obrat:";
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
            // total
            // 
            this.total.FillWeight = 81.47208F;
            this.total.HeaderText = "Celkem";
            this.total.Name = "total";
            // 
            // InnerDir
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(234)))), ((int)(((byte)(185)))));
            this.InnerDir.DefaultCellStyle = dataGridViewCellStyle2;
            this.InnerDir.HeaderText = "Středisková režie";
            this.InnerDir.Name = "InnerDir";
            this.InnerDir.Visible = false;
            // 
            // BudgetPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BudgetPage";
            this.Size = new System.Drawing.Size(963, 487);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.columnContextMenu.ResumeLayout(false);
            this.rowContextMenu.ResumeLayout(false);
            this.innerDirContextMenu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip columnContextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smazatToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip rowContextMenu;
        private System.Windows.Forms.ToolStripMenuItem smazatToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip innerDirContextMenu;
        private System.Windows.Forms.ToolStripMenuItem cToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minimaxiToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStripMenuItem exportSubToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button newSubButton;
        private System.Windows.Forms.Button newDirButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button collapseAllButton;
        private System.Windows.Forms.Button expandAllButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label nakladTextLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label vysledekTextLabel;
        private System.Windows.Forms.Label obratTextLabel;
        private System.Windows.Forms.Label vysledekLabel;
        private System.Windows.Forms.Label obratLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewTextBoxColumn InnerDir;
    }
}
