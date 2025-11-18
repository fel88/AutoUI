namespace AutoUI
{
    partial class SimpleTestEditor
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleTestEditor));
            listView1 = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            searchByPatternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            clickToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            delayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            mouseUpDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            scriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            screenshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            gotoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            labelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            processToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            terminateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            cursorPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            waitPatternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            findAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            iterateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            wheelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            keyboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            upToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            topToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            downToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            setPatternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            runToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            runFromHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            sectionToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            columnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            columnsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            columnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            timer1 = new System.Windows.Forms.Timer(components);
            contextMenuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader3, columnHeader2 });
            listView1.ContextMenuStrip = contextMenuStrip1;
            listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Location = new System.Drawing.Point(4, 3);
            listView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(469, 466);
            listView1.TabIndex = 1;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = System.Windows.Forms.View.Details;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            listView1.KeyDown += listView1_KeyDown;
            listView1.MouseDoubleClick += listView1_MouseDoubleClick;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Name";
            columnHeader1.Width = 200;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Description";
            columnHeader3.Width = 200;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Type";
            columnHeader2.Width = 250;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addToolStripMenuItem, editToolStripMenuItem, deleteToolStripMenuItem, clearToolStripMenuItem, toolStripSeparator1, upToolStripMenuItem, topToolStripMenuItem, downToolStripMenuItem, toolStripSeparator2, setPatternToolStripMenuItem, toolStripSeparator3, runToolStripMenuItem1, runFromHereToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(148, 242);
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            // 
            // addToolStripMenuItem
            // 
            addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { searchByPatternToolStripMenuItem, clickToolStripMenuItem, delayToolStripMenuItem, mouseUpDownToolStripMenuItem, scriptToolStripMenuItem, screenshotToolStripMenuItem, gotoToolStripMenuItem, labelToolStripMenuItem, processToolStripMenuItem, cursorPositionToolStripMenuItem, waitPatternToolStripMenuItem, findAllToolStripMenuItem, iterateToolStripMenuItem, wheelToolStripMenuItem, keyboardToolStripMenuItem });
            addToolStripMenuItem.Image = Properties.Resources.plus;
            addToolStripMenuItem.Name = "addToolStripMenuItem";
            addToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            addToolStripMenuItem.Text = "add";
            addToolStripMenuItem.DropDownOpening += addToolStripMenuItem_DropDownOpening;
            // 
            // searchByPatternToolStripMenuItem
            // 
            searchByPatternToolStripMenuItem.Image = Properties.Resources.eye;
            searchByPatternToolStripMenuItem.Name = "searchByPatternToolStripMenuItem";
            searchByPatternToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            searchByPatternToolStripMenuItem.Text = "search by pattern";
            searchByPatternToolStripMenuItem.Click += searchByPatternToolStripMenuItem_Click;
            // 
            // clickToolStripMenuItem
            // 
            clickToolStripMenuItem.Image = Properties.Resources.mouse_select;
            clickToolStripMenuItem.Name = "clickToolStripMenuItem";
            clickToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            clickToolStripMenuItem.Text = "click";
            clickToolStripMenuItem.Click += clickToolStripMenuItem_Click_1;
            // 
            // delayToolStripMenuItem
            // 
            delayToolStripMenuItem.Image = Properties.Resources.clock_select;
            delayToolStripMenuItem.Name = "delayToolStripMenuItem";
            delayToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            delayToolStripMenuItem.Text = "delay";
            delayToolStripMenuItem.Click += delayToolStripMenuItem_Click_1;
            // 
            // mouseUpDownToolStripMenuItem
            // 
            mouseUpDownToolStripMenuItem.Image = Properties.Resources.mouse;
            mouseUpDownToolStripMenuItem.Name = "mouseUpDownToolStripMenuItem";
            mouseUpDownToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            mouseUpDownToolStripMenuItem.Text = "mouseUpDown";
            mouseUpDownToolStripMenuItem.Click += mouseUpDownToolStripMenuItem_Click_1;
            // 
            // scriptToolStripMenuItem
            // 
            scriptToolStripMenuItem.Image = Properties.Resources.script_binary;
            scriptToolStripMenuItem.Name = "scriptToolStripMenuItem";
            scriptToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            scriptToolStripMenuItem.Text = "script";
            scriptToolStripMenuItem.Click += scriptToolStripMenuItem_Click;
            // 
            // screenshotToolStripMenuItem
            // 
            screenshotToolStripMenuItem.Image = Properties.Resources.image_select;
            screenshotToolStripMenuItem.Name = "screenshotToolStripMenuItem";
            screenshotToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            screenshotToolStripMenuItem.Text = "screenshot";
            screenshotToolStripMenuItem.Click += screenshotToolStripMenuItem_Click;
            // 
            // gotoToolStripMenuItem
            // 
            gotoToolStripMenuItem.Image = Properties.Resources.arrow_branch;
            gotoToolStripMenuItem.Name = "gotoToolStripMenuItem";
            gotoToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            gotoToolStripMenuItem.Text = "goto";
            gotoToolStripMenuItem.Click += gotoToolStripMenuItem_Click_1;
            // 
            // labelToolStripMenuItem
            // 
            labelToolStripMenuItem.Image = Properties.Resources.tag_hash;
            labelToolStripMenuItem.Name = "labelToolStripMenuItem";
            labelToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            labelToolStripMenuItem.Text = "label";
            labelToolStripMenuItem.Click += labelToolStripMenuItem_Click_1;
            // 
            // processToolStripMenuItem
            // 
            processToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { runToolStripMenuItem, terminateToolStripMenuItem });
            processToolStripMenuItem.Image = Properties.Resources.application_terminal;
            processToolStripMenuItem.Name = "processToolStripMenuItem";
            processToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            processToolStripMenuItem.Text = "process ";
            // 
            // runToolStripMenuItem
            // 
            runToolStripMenuItem.Name = "runToolStripMenuItem";
            runToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            runToolStripMenuItem.Text = "run";
            runToolStripMenuItem.Click += runToolStripMenuItem_Click;
            // 
            // terminateToolStripMenuItem
            // 
            terminateToolStripMenuItem.Name = "terminateToolStripMenuItem";
            terminateToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            terminateToolStripMenuItem.Text = "terminate";
            terminateToolStripMenuItem.Click += terminateToolStripMenuItem_Click;
            // 
            // cursorPositionToolStripMenuItem
            // 
            cursorPositionToolStripMenuItem.Image = Properties.Resources.cursor;
            cursorPositionToolStripMenuItem.Name = "cursorPositionToolStripMenuItem";
            cursorPositionToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            cursorPositionToolStripMenuItem.Text = "cursor position";
            cursorPositionToolStripMenuItem.Click += cursorPositionToolStripMenuItem_Click;
            // 
            // waitPatternToolStripMenuItem
            // 
            waitPatternToolStripMenuItem.Image = Properties.Resources.hourglass;
            waitPatternToolStripMenuItem.Name = "waitPatternToolStripMenuItem";
            waitPatternToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            waitPatternToolStripMenuItem.Text = "wait image pattern";
            waitPatternToolStripMenuItem.Click += waitPatternToolStripMenuItem_Click;
            // 
            // findAllToolStripMenuItem
            // 
            findAllToolStripMenuItem.Image = Properties.Resources.binocular;
            findAllToolStripMenuItem.Name = "findAllToolStripMenuItem";
            findAllToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            findAllToolStripMenuItem.Text = "find all";
            findAllToolStripMenuItem.Click += findAllToolStripMenuItem_Click;
            // 
            // iterateToolStripMenuItem
            // 
            iterateToolStripMenuItem.Image = Properties.Resources.arrow_circle_double_135;
            iterateToolStripMenuItem.Name = "iterateToolStripMenuItem";
            iterateToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            iterateToolStripMenuItem.Text = "iterate";
            iterateToolStripMenuItem.Click += iterateToolStripMenuItem_Click;
            // 
            // wheelToolStripMenuItem
            // 
            wheelToolStripMenuItem.Image = Properties.Resources.mouse_select_wheel;
            wheelToolStripMenuItem.Name = "wheelToolStripMenuItem";
            wheelToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            wheelToolStripMenuItem.Text = "wheel";
            wheelToolStripMenuItem.Click += wheelToolStripMenuItem_Click;
            // 
            // keyboardToolStripMenuItem
            // 
            keyboardToolStripMenuItem.Image = Properties.Resources.keyboard_full_wireless;
            keyboardToolStripMenuItem.Name = "keyboardToolStripMenuItem";
            keyboardToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            keyboardToolStripMenuItem.Text = "keyboard";
            keyboardToolStripMenuItem.Click += keyboardToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Image = Properties.Resources.pencil;
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            editToolStripMenuItem.Text = "edit";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Image = Properties.Resources.cross;
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            deleteToolStripMenuItem.Text = "delete";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // 
            // clearToolStripMenuItem
            // 
            clearToolStripMenuItem.Image = Properties.Resources.eraser;
            clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            clearToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            clearToolStripMenuItem.Text = "clear";
            clearToolStripMenuItem.Click += clearToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(144, 6);
            // 
            // upToolStripMenuItem
            // 
            upToolStripMenuItem.Image = Properties.Resources.arrow_090;
            upToolStripMenuItem.Name = "upToolStripMenuItem";
            upToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            upToolStripMenuItem.Text = "up";
            upToolStripMenuItem.Click += upToolStripMenuItem_Click;
            // 
            // topToolStripMenuItem
            // 
            topToolStripMenuItem.Image = Properties.Resources.arrow_stop_090;
            topToolStripMenuItem.Name = "topToolStripMenuItem";
            topToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            topToolStripMenuItem.Text = "top";
            topToolStripMenuItem.Click += topToolStripMenuItem_Click;
            // 
            // downToolStripMenuItem
            // 
            downToolStripMenuItem.Image = Properties.Resources.arrow_270;
            downToolStripMenuItem.Name = "downToolStripMenuItem";
            downToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            downToolStripMenuItem.Text = "down";
            downToolStripMenuItem.Click += downToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(144, 6);
            // 
            // setPatternToolStripMenuItem
            // 
            setPatternToolStripMenuItem.Image = Properties.Resources.spectrum;
            setPatternToolStripMenuItem.Name = "setPatternToolStripMenuItem";
            setPatternToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            setPatternToolStripMenuItem.Text = "set pattern";
            setPatternToolStripMenuItem.Click += setPatternToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(144, 6);
            // 
            // runToolStripMenuItem1
            // 
            runToolStripMenuItem1.Image = Properties.Resources.control_double;
            runToolStripMenuItem1.Name = "runToolStripMenuItem1";
            runToolStripMenuItem1.Size = new System.Drawing.Size(147, 22);
            runToolStripMenuItem1.Text = "run selected";
            runToolStripMenuItem1.Click += runToolStripMenuItem1_Click;
            // 
            // runFromHereToolStripMenuItem
            // 
            runFromHereToolStripMenuItem.Name = "runFromHereToolStripMenuItem";
            runFromHereToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            runFromHereToolStripMenuItem.Text = "run from here";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1, toolStripStatusLabel2, sectionToolStripStatusLabel });
            statusStrip1.Location = new System.Drawing.Point(0, 497);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusStrip1.Size = new System.Drawing.Size(954, 22);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new System.Drawing.Size(785, 17);
            toolStripStatusLabel2.Spring = true;
            // 
            // sectionToolStripStatusLabel
            // 
            sectionToolStripStatusLabel.Name = "sectionToolStripStatusLabel";
            sectionToolStripStatusLabel.Size = new System.Drawing.Size(34, 17);
            sectionToolStripStatusLabel.Text = "main";
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButton1, toolStripButton2, toolStripDropDownButton1 });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(954, 25);
            toolStrip1.TabIndex = 5;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.Image = Properties.Resources.flag_checker;
            toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new System.Drawing.Size(45, 22);
            toolStripButton1.Text = "run";
            toolStripButton1.Click += toolStripButton1_Click;
            // 
            // toolStripButton2
            // 
            toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripButton2.Image = (System.Drawing.Image)resources.GetObject("toolStripButton2.Image");
            toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new System.Drawing.Size(71, 22);
            toolStripButton2.Text = "auto layout";
            toolStripButton2.Click += toolStripButton2_Click;
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { columnsToolStripMenuItem, columnsToolStripMenuItem1, columnToolStripMenuItem });
            toolStripDropDownButton1.Image = Properties.Resources.table;
            toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new System.Drawing.Size(98, 22);
            toolStripDropDownButton1.Text = "table layout";
            // 
            // columnsToolStripMenuItem
            // 
            columnsToolStripMenuItem.Name = "columnsToolStripMenuItem";
            columnsToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            columnsToolStripMenuItem.Text = "3 columns";
            columnsToolStripMenuItem.Click += columnsToolStripMenuItem_Click;
            // 
            // columnsToolStripMenuItem1
            // 
            columnsToolStripMenuItem1.Name = "columnsToolStripMenuItem1";
            columnsToolStripMenuItem1.Size = new System.Drawing.Size(129, 22);
            columnsToolStripMenuItem1.Text = "2 columns";
            columnsToolStripMenuItem1.Click += columnsToolStripMenuItem1_Click;
            // 
            // columnToolStripMenuItem
            // 
            columnToolStripMenuItem.Name = "columnToolStripMenuItem";
            columnToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            columnToolStripMenuItem.Text = "1 column";
            columnToolStripMenuItem.Click += columnToolStripMenuItem_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(listView1, 0, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(954, 472);
            tableLayoutPanel1.TabIndex = 6;
            tableLayoutPanel1.Paint += tableLayoutPanel1_Paint;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 25;
            timer1.Tick += timer1_Tick;
            // 
            // SimpleTestEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(954, 519);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(toolStrip1);
            Controls.Add(statusStrip1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "SimpleTestEditor";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Test editor";
            contextMenuStrip1.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

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
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel sectionToolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem wheelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runFromHereToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keyboardToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem columnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem columnsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem columnToolStripMenuItem;
    }
}

