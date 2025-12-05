namespace AutoUI
{
    partial class StringsListEditor
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
            listView3 = new System.Windows.Forms.ListView();
            columnHeader7 = new System.Windows.Forms.ColumnHeader();
            columnHeader8 = new System.Windows.Forms.ColumnHeader();
            contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(components);
            addKeyvaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            contextMenuStrip3.SuspendLayout();
            SuspendLayout();
            // 
            // listView3
            // 
            listView3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader7, columnHeader8 });
            listView3.ContextMenuStrip = contextMenuStrip3;
            listView3.Dock = System.Windows.Forms.DockStyle.Fill;
            listView3.FullRowSelect = true;
            listView3.GridLines = true;
            listView3.Location = new System.Drawing.Point(0, 0);
            listView3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listView3.Name = "listView3";
            listView3.Size = new System.Drawing.Size(800, 226);
            listView3.TabIndex = 2;
            listView3.UseCompatibleStateImageBehavior = false;
            listView3.View = System.Windows.Forms.View.Details;
            listView3.MouseClick += listView3_MouseClick;
            listView3.MouseDoubleClick += listView3_MouseDoubleClick;
            // 
            // columnHeader7
            // 
            columnHeader7.Text = "Key";
            columnHeader7.Width = 250;
            // 
            // columnHeader8
            // 
            columnHeader8.Text = "Value";
            columnHeader8.Width = 250;
            // 
            // contextMenuStrip3
            // 
            contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addKeyvaleToolStripMenuItem, deleteToolStripMenuItem1, editToolStripMenuItem });
            contextMenuStrip3.Name = "contextMenuStrip3";
            contextMenuStrip3.Size = new System.Drawing.Size(181, 92);
            contextMenuStrip3.Opening += contextMenuStrip3_Opening;
            // 
            // addKeyvaleToolStripMenuItem
            // 
            addKeyvaleToolStripMenuItem.Image = Properties.Resources.plus;
            addKeyvaleToolStripMenuItem.Name = "addKeyvaleToolStripMenuItem";
            addKeyvaleToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            addKeyvaleToolStripMenuItem.Text = "add key/value";
            addKeyvaleToolStripMenuItem.Click += addKeyvaleToolStripMenuItem_Click;
            // 
            // deleteToolStripMenuItem1
            // 
            deleteToolStripMenuItem1.Image = Properties.Resources.cross;
            deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            deleteToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            deleteToolStripMenuItem1.Text = "delete";
            deleteToolStripMenuItem1.Click += deleteToolStripMenuItem1_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Image = Properties.Resources.pencil;
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            editToolStripMenuItem.Text = "edit";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            // 
            // StringsListEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 226);
            Controls.Add(listView3);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "StringsListEditor";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "VariablesEditor";
            contextMenuStrip3.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView3;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem addKeyvaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    }
}