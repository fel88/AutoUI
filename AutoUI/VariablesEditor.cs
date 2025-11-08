using AutoUI.Common;
using AutoUI.TestItems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace AutoUI
{
    public partial class VariablesEditor : Form
    {
        public VariablesEditor()
        {
            InitializeComponent();
        }

        private void addKeyvaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var test = listView3.Tag as AutoTest;
            KeyValueDialog kvd = new KeyValueDialog();
            if (kvd.ShowDialog() == DialogResult.OK)
            {
                test.Data.Add(kvd.Key, kvd.Value);
            }
            //update lv3
            updateKeyValueList();
        }
        void updateKeyValueList()
        {
            if (listView3.Tag is AutoTest test)
            {
                listView3.Items.Clear();
                foreach (var item in test.Data)
                {
                    listView3.Items.Add(new ListViewItem(new string[] { item.Key, item.Value.ToString() }) { Tag = item });
                }
            }
        }
        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count == 0) return;
            var p = (KeyValuePair<string, object>)listView3.SelectedItems[0].Tag;
            var test = listView3.Tag as AutoTest;
            test.Data.Remove(p.Key);
            updateKeyValueList();
        }

        private void listView3_MouseClick(object sender, MouseEventArgs e)
        {

        }

        public void Init(AutoTest test)
        {
            listView3.Items.Clear();
            foreach (var item in test.Data)
            {
                listView3.Items.Add(new ListViewItem(new string[] { item.Key, item.Value.ToString() }) { Tag = item });
            }
            listView3.Tag = test;
        }

        public void Init(EmittedSubTest et)
        {
            listView3.Items.Clear();
            foreach (var item in et.Data)
            {
                listView3.Items.Add(new ListViewItem(new string[] { item.Key, item.Value.ToString() }) { Tag = item });
            }
            listView3.Tag = et;
        }
        private void listView3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
            {
                var tag = listView3.SelectedItems[0].Tag;
                if (tag is KeyValuePair<string, object> s1)
                {
                    if (s1.Value is Bitmap b)
                    {
                        b.Save("temp1.png");
                        Process.Start("temp1.png");
                    }
                }
            }


            if (listView3.Tag is AutoTest test)
            {
                var pair = (KeyValuePair<string, object>)listView3.SelectedItems[0].Tag;
                KeyValueDialog kvd = new KeyValueDialog();
                kvd.Init(pair);
                if (kvd.ShowDialog() == DialogResult.OK)
                {
                    test.Data[pair.Key] = kvd.Value;
                    updateKeyValueList();
                }
            }
            if (listView3.Tag is EmittedSubTest esub)
            {
                if (listView3.SelectedItems.Count == 0) return;
                var tag = listView3.SelectedItems[0].Tag;
            }
        }

        private void contextMenuStrip3_Opening(object sender, CancelEventArgs e)
        {
            if (!(listView3.Tag is AutoTest))
                e.Cancel = true;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count == 0) 
                return;

            listView3_MouseDoubleClick(sender, null);
        }
    }
}
