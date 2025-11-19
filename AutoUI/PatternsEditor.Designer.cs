
namespace AutoUI
{
    partial class PatternsEditor
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
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            listView2 = new System.Windows.Forms.ListView();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader5 = new System.Windows.Forms.ColumnHeader();
            columnHeader6 = new System.Windows.Forms.ColumnHeader();
            contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(components);
            addItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            fromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            fromClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            asIsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            cropToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            grabScreenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            listView1 = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            addPatternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            newFromClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            generatePaaternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            fromFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            fromScreenshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            grabScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pictureBox1 = new PictureBoxWithInterpolationMode();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            tableLayoutPanel1.SuspendLayout();
            contextMenuStrip2.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanel1.Controls.Add(listView2, 0, 1);
            tableLayoutPanel1.Controls.Add(listView1, 0, 0);
            tableLayoutPanel1.Controls.Add(pictureBox1, 1, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new System.Drawing.Size(1060, 497);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // listView2
            // 
            listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader3, columnHeader4, columnHeader2, columnHeader5, columnHeader6 });
            listView2.ContextMenuStrip = contextMenuStrip2;
            listView2.Dock = System.Windows.Forms.DockStyle.Fill;
            listView2.Enabled = false;
            listView2.FullRowSelect = true;
            listView2.GridLines = true;
            listView2.Location = new System.Drawing.Point(4, 251);
            listView2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listView2.Name = "listView2";
            listView2.Size = new System.Drawing.Size(522, 243);
            listView2.TabIndex = 1;
            listView2.UseCompatibleStateImageBehavior = false;
            listView2.View = System.Windows.Forms.View.Details;
            listView2.SelectedIndexChanged += listView2_SelectedIndexChanged;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Name";
            columnHeader3.Width = 120;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Mode";
            columnHeader4.Width = 120;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Pixels mode";
            columnHeader2.Width = 80;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "Pixel dist";
            columnHeader5.Width = 80;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "Error rate (%)";
            columnHeader6.Width = 90;
            // 
            // contextMenuStrip2
            // 
            contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addItemToolStripMenuItem, ditToolStripMenuItem, deleteToolStripMenuItem1, saveToolStripMenuItem });
            contextMenuStrip2.Name = "contextMenuStrip2";
            contextMenuStrip2.Size = new System.Drawing.Size(122, 92);
            // 
            // addItemToolStripMenuItem
            // 
            addItemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { fromFileToolStripMenuItem, fromClipboardToolStripMenuItem, grabScreenToolStripMenuItem1 });
            addItemToolStripMenuItem.Image = Properties.Resources.plus;
            addItemToolStripMenuItem.Name = "addItemToolStripMenuItem";
            addItemToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            addItemToolStripMenuItem.Text = "add item";
            addItemToolStripMenuItem.Click += addItemToolStripMenuItem_Click;
            // 
            // fromFileToolStripMenuItem
            // 
            fromFileToolStripMenuItem.Name = "fromFileToolStripMenuItem";
            fromFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            fromFileToolStripMenuItem.Text = "from file";
            fromFileToolStripMenuItem.Click += fromFileToolStripMenuItem_Click;
            // 
            // fromClipboardToolStripMenuItem
            // 
            fromClipboardToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { asIsToolStripMenuItem, cropToolStripMenuItem });
            fromClipboardToolStripMenuItem.Name = "fromClipboardToolStripMenuItem";
            fromClipboardToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            fromClipboardToolStripMenuItem.Text = "from clipboard";
            fromClipboardToolStripMenuItem.Click += fromClipboardToolStripMenuItem_Click;
            // 
            // asIsToolStripMenuItem
            // 
            asIsToolStripMenuItem.Name = "asIsToolStripMenuItem";
            asIsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            asIsToolStripMenuItem.Text = "as is";
            asIsToolStripMenuItem.Click += asIsToolStripMenuItem_Click;
            // 
            // cropToolStripMenuItem
            // 
            cropToolStripMenuItem.Name = "cropToolStripMenuItem";
            cropToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            cropToolStripMenuItem.Text = "crop";
            cropToolStripMenuItem.Click += cropToolStripMenuItem_Click;
            // 
            // grabScreenToolStripMenuItem1
            // 
            grabScreenToolStripMenuItem1.Name = "grabScreenToolStripMenuItem1";
            grabScreenToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            grabScreenToolStripMenuItem1.Text = "grab screen";
            grabScreenToolStripMenuItem1.Click += grabScreenToolStripMenuItem1_Click;
            // 
            // ditToolStripMenuItem
            // 
            ditToolStripMenuItem.Image = Properties.Resources.pencil;
            ditToolStripMenuItem.Name = "ditToolStripMenuItem";
            ditToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            ditToolStripMenuItem.Text = "edit";
            ditToolStripMenuItem.Click += ditToolStripMenuItem_Click;
            // 
            // deleteToolStripMenuItem1
            // 
            deleteToolStripMenuItem1.Image = Properties.Resources.cross;
            deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            deleteToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            deleteToolStripMenuItem1.Text = "delete";
            deleteToolStripMenuItem1.Click += deleteToolStripMenuItem1_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Image = Properties.Resources.disk;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            saveToolStripMenuItem.Text = "save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1 });
            listView1.ContextMenuStrip = contextMenuStrip1;
            listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Location = new System.Drawing.Point(4, 3);
            listView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(522, 242);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = System.Windows.Forms.View.Details;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Name";
            columnHeader1.Width = 350;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addPatternToolStripMenuItem, editToolStripMenuItem, deleteToolStripMenuItem, toolStripSeparator1, updateToolStripMenuItem, newFromClipboardToolStripMenuItem, generatePaaternToolStripMenuItem, grabScreenToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(179, 164);
            // 
            // addPatternToolStripMenuItem
            // 
            addPatternToolStripMenuItem.Image = Properties.Resources.plus;
            addPatternToolStripMenuItem.Name = "addPatternToolStripMenuItem";
            addPatternToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            addPatternToolStripMenuItem.Text = "add pattern";
            addPatternToolStripMenuItem.Click += addPatternToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Image = Properties.Resources.pencil;
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            editToolStripMenuItem.Text = "edit";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Image = Properties.Resources.cross;
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            deleteToolStripMenuItem.Text = "delete";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(175, 6);
            // 
            // updateToolStripMenuItem
            // 
            updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            updateToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            updateToolStripMenuItem.Text = "update list";
            updateToolStripMenuItem.Click += updateToolStripMenuItem_Click;
            // 
            // newFromClipboardToolStripMenuItem
            // 
            newFromClipboardToolStripMenuItem.Image = Properties.Resources.plus_white;
            newFromClipboardToolStripMenuItem.Name = "newFromClipboardToolStripMenuItem";
            newFromClipboardToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            newFromClipboardToolStripMenuItem.Text = "new from clipboard";
            newFromClipboardToolStripMenuItem.Click += newFromClipboardToolStripMenuItem_Click;
            // 
            // generatePaaternToolStripMenuItem
            // 
            generatePaaternToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { fromFontToolStripMenuItem, fromScreenshotToolStripMenuItem });
            generatePaaternToolStripMenuItem.Name = "generatePaaternToolStripMenuItem";
            generatePaaternToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            generatePaaternToolStripMenuItem.Text = "generate pattern";
            generatePaaternToolStripMenuItem.Click += generatePaaternToolStripMenuItem_Click;
            // 
            // fromFontToolStripMenuItem
            // 
            fromFontToolStripMenuItem.Name = "fromFontToolStripMenuItem";
            fromFontToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            fromFontToolStripMenuItem.Text = "from font";
            fromFontToolStripMenuItem.Click += fromFontToolStripMenuItem_Click;
            // 
            // fromScreenshotToolStripMenuItem
            // 
            fromScreenshotToolStripMenuItem.Name = "fromScreenshotToolStripMenuItem";
            fromScreenshotToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            fromScreenshotToolStripMenuItem.Text = "from screenshot";
            fromScreenshotToolStripMenuItem.Click += fromScreenshotToolStripMenuItem_Click;
            // 
            // grabScreenToolStripMenuItem
            // 
            grabScreenToolStripMenuItem.Name = "grabScreenToolStripMenuItem";
            grabScreenToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            grabScreenToolStripMenuItem.Text = "grab screen";
            grabScreenToolStripMenuItem.Click += grabScreenToolStripMenuItem_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBox1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            pictureBox1.Location = new System.Drawing.Point(534, 3);
            pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            tableLayoutPanel1.SetRowSpan(pictureBox1, 2);
            pictureBox1.Size = new System.Drawing.Size(522, 491);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new System.Drawing.Point(0, 497);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusStrip1.Size = new System.Drawing.Size(1060, 22);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(13, 17);
            toolStripStatusLabel1.Text = "..";
            // 
            // PatternsEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1060, 519);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(statusStrip1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "PatternsEditor";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Patterns editor";
            tableLayoutPanel1.ResumeLayout(false);
            contextMenuStrip2.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private AutoUI.PictureBoxWithInterpolationMode pictureBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addPatternToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem addItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fromFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFromClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generatePaaternToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromFontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromScreenshotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ditToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem grabScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ToolStripMenuItem grabScreenToolStripMenuItem1;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ToolStripMenuItem asIsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cropToolStripMenuItem;
    }
}