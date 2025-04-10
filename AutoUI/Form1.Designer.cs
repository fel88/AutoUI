namespace AutoUI
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchByPatternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clickToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.delayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mouseUpDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.screenshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.terminateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cursorPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waitPatternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iterateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wheelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.upToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.setPatternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.runToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.runFromHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.sectionToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.mainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.finalizerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.emitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.edirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(412, 3);
            this.listView1.Name = "listView1";
            this.tableLayoutPanel1.SetRowSpan(this.listView1, 2);
            this.listView1.Size = new System.Drawing.Size(403, 397);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Action";
            this.columnHeader1.Width = 260;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.clearToolStripMenuItem,
            this.toolStripSeparator1,
            this.upToolStripMenuItem,
            this.topToolStripMenuItem,
            this.downToolStripMenuItem,
            this.toolStripSeparator2,
            this.setPatternToolStripMenuItem,
            this.toolStripSeparator3,
            this.runToolStripMenuItem1,
            this.runFromHereToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 264);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchByPatternToolStripMenuItem,
            this.clickToolStripMenuItem,
            this.delayToolStripMenuItem,
            this.mouseUpDownToolStripMenuItem,
            this.scriptToolStripMenuItem,
            this.screenshotToolStripMenuItem,
            this.gotoToolStripMenuItem,
            this.labelToolStripMenuItem,
            this.processToolStripMenuItem,
            this.cursorPositionToolStripMenuItem,
            this.waitPatternToolStripMenuItem,
            this.findAllToolStripMenuItem,
            this.iterateToolStripMenuItem,
            this.wheelToolStripMenuItem});
            this.addToolStripMenuItem.Image = global::AutoUI.Properties.Resources.plus;
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addToolStripMenuItem.Text = "add";
            this.addToolStripMenuItem.DropDownOpening += new System.EventHandler(this.addToolStripMenuItem_DropDownOpening);
            // 
            // searchByPatternToolStripMenuItem
            // 
            this.searchByPatternToolStripMenuItem.Name = "searchByPatternToolStripMenuItem";
            this.searchByPatternToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.searchByPatternToolStripMenuItem.Text = "search by pattern";
            this.searchByPatternToolStripMenuItem.Click += new System.EventHandler(this.searchByPatternToolStripMenuItem_Click);
            // 
            // clickToolStripMenuItem
            // 
            this.clickToolStripMenuItem.Name = "clickToolStripMenuItem";
            this.clickToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.clickToolStripMenuItem.Text = "click";
            this.clickToolStripMenuItem.Click += new System.EventHandler(this.clickToolStripMenuItem_Click_1);
            // 
            // delayToolStripMenuItem
            // 
            this.delayToolStripMenuItem.Name = "delayToolStripMenuItem";
            this.delayToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.delayToolStripMenuItem.Text = "delay";
            this.delayToolStripMenuItem.Click += new System.EventHandler(this.delayToolStripMenuItem_Click_1);
            // 
            // mouseUpDownToolStripMenuItem
            // 
            this.mouseUpDownToolStripMenuItem.Name = "mouseUpDownToolStripMenuItem";
            this.mouseUpDownToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.mouseUpDownToolStripMenuItem.Text = "mouseUpDown";
            this.mouseUpDownToolStripMenuItem.Click += new System.EventHandler(this.mouseUpDownToolStripMenuItem_Click_1);
            // 
            // scriptToolStripMenuItem
            // 
            this.scriptToolStripMenuItem.Name = "scriptToolStripMenuItem";
            this.scriptToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.scriptToolStripMenuItem.Text = "script";
            this.scriptToolStripMenuItem.Click += new System.EventHandler(this.scriptToolStripMenuItem_Click);
            // 
            // screenshotToolStripMenuItem
            // 
            this.screenshotToolStripMenuItem.Name = "screenshotToolStripMenuItem";
            this.screenshotToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.screenshotToolStripMenuItem.Text = "screenshot";
            this.screenshotToolStripMenuItem.Click += new System.EventHandler(this.screenshotToolStripMenuItem_Click);
            // 
            // gotoToolStripMenuItem
            // 
            this.gotoToolStripMenuItem.Name = "gotoToolStripMenuItem";
            this.gotoToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.gotoToolStripMenuItem.Text = "goto";
            this.gotoToolStripMenuItem.Click += new System.EventHandler(this.gotoToolStripMenuItem_Click_1);
            // 
            // labelToolStripMenuItem
            // 
            this.labelToolStripMenuItem.Name = "labelToolStripMenuItem";
            this.labelToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.labelToolStripMenuItem.Text = "label";
            this.labelToolStripMenuItem.Click += new System.EventHandler(this.labelToolStripMenuItem_Click_1);
            // 
            // processToolStripMenuItem
            // 
            this.processToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem,
            this.terminateToolStripMenuItem});
            this.processToolStripMenuItem.Name = "processToolStripMenuItem";
            this.processToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.processToolStripMenuItem.Text = "process ";
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.runToolStripMenuItem.Text = "run";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // terminateToolStripMenuItem
            // 
            this.terminateToolStripMenuItem.Name = "terminateToolStripMenuItem";
            this.terminateToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.terminateToolStripMenuItem.Text = "terminate";
            this.terminateToolStripMenuItem.Click += new System.EventHandler(this.terminateToolStripMenuItem_Click);
            // 
            // cursorPositionToolStripMenuItem
            // 
            this.cursorPositionToolStripMenuItem.Name = "cursorPositionToolStripMenuItem";
            this.cursorPositionToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.cursorPositionToolStripMenuItem.Text = "cursor position";
            this.cursorPositionToolStripMenuItem.Click += new System.EventHandler(this.cursorPositionToolStripMenuItem_Click);
            // 
            // waitPatternToolStripMenuItem
            // 
            this.waitPatternToolStripMenuItem.Name = "waitPatternToolStripMenuItem";
            this.waitPatternToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.waitPatternToolStripMenuItem.Text = "wait image pattern";
            this.waitPatternToolStripMenuItem.Click += new System.EventHandler(this.waitPatternToolStripMenuItem_Click);
            // 
            // findAllToolStripMenuItem
            // 
            this.findAllToolStripMenuItem.Name = "findAllToolStripMenuItem";
            this.findAllToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.findAllToolStripMenuItem.Text = "find all";
            this.findAllToolStripMenuItem.Click += new System.EventHandler(this.findAllToolStripMenuItem_Click);
            // 
            // iterateToolStripMenuItem
            // 
            this.iterateToolStripMenuItem.Name = "iterateToolStripMenuItem";
            this.iterateToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.iterateToolStripMenuItem.Text = "iterate";
            this.iterateToolStripMenuItem.Click += new System.EventHandler(this.iterateToolStripMenuItem_Click);
            // 
            // wheelToolStripMenuItem
            // 
            this.wheelToolStripMenuItem.Name = "wheelToolStripMenuItem";
            this.wheelToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.wheelToolStripMenuItem.Text = "wheel";
            this.wheelToolStripMenuItem.Click += new System.EventHandler(this.wheelToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Image = global::AutoUI.Properties.Resources.pencil;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.editToolStripMenuItem.Text = "edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::AutoUI.Properties.Resources.cross;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.deleteToolStripMenuItem.Text = "delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Image = global::AutoUI.Properties.Resources.eraser;
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.clearToolStripMenuItem.Text = "clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // upToolStripMenuItem
            // 
            this.upToolStripMenuItem.Image = global::AutoUI.Properties.Resources.arrow_090;
            this.upToolStripMenuItem.Name = "upToolStripMenuItem";
            this.upToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.upToolStripMenuItem.Text = "up";
            this.upToolStripMenuItem.Click += new System.EventHandler(this.upToolStripMenuItem_Click);
            // 
            // topToolStripMenuItem
            // 
            this.topToolStripMenuItem.Image = global::AutoUI.Properties.Resources.arrow_stop_090;
            this.topToolStripMenuItem.Name = "topToolStripMenuItem";
            this.topToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.topToolStripMenuItem.Text = "top";
            this.topToolStripMenuItem.Click += new System.EventHandler(this.topToolStripMenuItem_Click);
            // 
            // downToolStripMenuItem
            // 
            this.downToolStripMenuItem.Image = global::AutoUI.Properties.Resources.arrow_270;
            this.downToolStripMenuItem.Name = "downToolStripMenuItem";
            this.downToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.downToolStripMenuItem.Text = "down";
            this.downToolStripMenuItem.Click += new System.EventHandler(this.downToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // setPatternToolStripMenuItem
            // 
            this.setPatternToolStripMenuItem.Image = global::AutoUI.Properties.Resources.spectrum;
            this.setPatternToolStripMenuItem.Name = "setPatternToolStripMenuItem";
            this.setPatternToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.setPatternToolStripMenuItem.Text = "set pattern";
            this.setPatternToolStripMenuItem.Click += new System.EventHandler(this.setPatternToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            // 
            // runToolStripMenuItem1
            // 
            this.runToolStripMenuItem1.Image = global::AutoUI.Properties.Resources.control_double;
            this.runToolStripMenuItem1.Name = "runToolStripMenuItem1";
            this.runToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.runToolStripMenuItem1.Text = "run selected";
            this.runToolStripMenuItem1.Click += new System.EventHandler(this.runToolStripMenuItem1_Click);
            // 
            // runFromHereToolStripMenuItem
            // 
            this.runFromHereToolStripMenuItem.Name = "runFromHereToolStripMenuItem";
            this.runFromHereToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.runFromHereToolStripMenuItem.Text = "run from here";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.sectionToolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(818, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(651, 17);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // sectionToolStripStatusLabel
            // 
            this.sectionToolStripStatusLabel.Name = "sectionToolStripStatusLabel";
            this.sectionToolStripStatusLabel.Size = new System.Drawing.Size(34, 17);
            this.sectionToolStripStatusLabel.Text = "main";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(818, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(29, 22);
            this.toolStripButton1.Text = "run";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(71, 22);
            this.toolStripButton2.Text = "auto layout";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripMenuItem,
            this.finalizerToolStripMenuItem,
            this.emitterToolStripMenuItem,
            this.toolStripSeparator4});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(87, 22);
            this.toolStripDropDownButton1.Text = "code section";
            // 
            // mainToolStripMenuItem
            // 
            this.mainToolStripMenuItem.Name = "mainToolStripMenuItem";
            this.mainToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.mainToolStripMenuItem.Text = "main";
            this.mainToolStripMenuItem.Click += new System.EventHandler(this.mainToolStripMenuItem_Click);
            // 
            // finalizerToolStripMenuItem
            // 
            this.finalizerToolStripMenuItem.Name = "finalizerToolStripMenuItem";
            this.finalizerToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.finalizerToolStripMenuItem.Text = "finalizer";
            this.finalizerToolStripMenuItem.Click += new System.EventHandler(this.finalizerToolStripMenuItem_Click);
            // 
            // emitterToolStripMenuItem
            // 
            this.emitterToolStripMenuItem.Name = "emitterToolStripMenuItem";
            this.emitterToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.emitterToolStripMenuItem.Text = "emitter";
            this.emitterToolStripMenuItem.Click += new System.EventHandler(this.emitterToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(112, 6);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.listView2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.listView1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(818, 403);
            this.tableLayoutPanel1.TabIndex = 6;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3});
            this.listView2.ContextMenuStrip = this.contextMenuStrip2;
            this.listView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView2.FullRowSelect = true;
            this.listView2.GridLines = true;
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(3, 206);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(403, 194);
            this.listView2.TabIndex = 2;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            this.listView2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView2_MouseDoubleClick);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Section";
            this.columnHeader2.Width = 260;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Role";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem1,
            this.deleteToolStripMenuItem1,
            this.edirToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(107, 70);
            // 
            // addToolStripMenuItem1
            // 
            this.addToolStripMenuItem1.Name = "addToolStripMenuItem1";
            this.addToolStripMenuItem1.Size = new System.Drawing.Size(106, 22);
            this.addToolStripMenuItem1.Text = "add";
            this.addToolStripMenuItem1.Click += new System.EventHandler(this.addToolStripMenuItem1_Click);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(106, 22);
            this.deleteToolStripMenuItem1.Text = "delete";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.deleteToolStripMenuItem1_Click);
            // 
            // edirToolStripMenuItem
            // 
            this.edirToolStripMenuItem.Name = "edirToolStripMenuItem";
            this.edirToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.edirToolStripMenuItem.Text = "edit";
            this.edirToolStripMenuItem.Click += new System.EventHandler(this.edirToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 25;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test editor";
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem upToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ToolStripMenuItem searchByPatternToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clickToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem delayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mouseUpDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem setPatternToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem terminateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cursorPositionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem topToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem waitPatternToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gotoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem labelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem screenshotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iterateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem finalizerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem emitterToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel sectionToolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem wheelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runFromHereToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolStripMenuItem edirToolStripMenuItem;
    }
}

