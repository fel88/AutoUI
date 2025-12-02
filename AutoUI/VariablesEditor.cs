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

            var d = AutoDialog.DialogHelpers.StartDialog();

            d.AddStringField("key", "Key", "new_key");
            d.AddStringField("value", "Value", "");

            if (d.ShowDialog())
            {
                if (listView3.Tag is AutoTest test)
                {
                    test.Data.Add(d.GetStringField("key"), d.GetStringField("value"));
                } else
                if (listView3.Tag is TestSet set)
                {
                    set.Vars.Add(d.GetStringField("key"), d.GetStringField("value"));
                }
                updateKeyValueList();
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
            if (listView3.Tag is TestSet set)
            {
                listView3.Items.Clear();
                foreach (var item in set.Vars)
                {
                    listView3.Items.Add(new ListViewItem(new string[] { item.Key, item.Value.ToString() }) { Tag = item });
                }
            }
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count == 0)
                return;

            var p = (KeyValuePair<string, object>)listView3.SelectedItems[0].Tag;
            if (listView3.Tag is AutoTest test)
            {
                test.Data.Remove(p.Key);
            }
            else
            if (listView3.Tag is TestSet set)
            {
                set.Vars.Remove(p.Key);

            }
            updateKeyValueList();
        }

        private void listView3_MouseClick(object sender, MouseEventArgs e)
        {

        }

        public void Init(IAutoTest test)
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
        public void Init(TestSet set)
        {
            listView3.Items.Clear();
            foreach (var item in set.Vars)
            {
                listView3.Items.Add(new ListViewItem(new string[] { item.Key, item.Value.ToString() }) { Tag = item });
            }
            listView3.Tag = set;
        }
        private void listView3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EditSelected();
        }
        private void EditSelected()
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

            if (listView3.Tag is TestSet set)
            {
                var pair = (KeyValuePair<string, object>)listView3.SelectedItems[0].Tag;
                if (pair.Value is string)
                {
                    var d = AutoDialog.DialogHelpers.StartDialog();

                    d.AddStringField("key", "Key", pair.Key);
                    d.AddStringField("value", "Value", pair.Value as string);
                    d.CreatedControls["key"][1].Enabled = false;

                    if (d.ShowDialog())
                    {
                        set.Vars[pair.Key] = d.GetStringField("value");
                        updateKeyValueList();
                    }
                }
            }

            if (listView3.Tag is AutoTest test)
            {
                var pair = (KeyValuePair<string, object>)listView3.SelectedItems[0].Tag;
                if (pair.Value is string)
                {
                    var d = AutoDialog.DialogHelpers.StartDialog();

                    d.AddStringField("key", "Key", pair.Key);
                    d.AddStringField("value", "Value", pair.Value as string);
                    d.CreatedControls["key"][1].Enabled = false;

                    if (d.ShowDialog())
                    {
                        test.Data[pair.Key] = d.GetStringField("value");
                        updateKeyValueList();
                    }
                }
            }
            if (listView3.Tag is EmittedSubTest esub)
            {
                if (listView3.SelectedItems.Count == 0)
                    return;

                var tag = listView3.SelectedItems[0].Tag;
            }
        }

        private void contextMenuStrip3_Opening(object sender, CancelEventArgs e)
        {
            if ((listView3.Tag is AutoTest))
            {
                return;
            }
            if ((listView3.Tag is TestSet))
            {
                return;
            }
            e.Cancel = true;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count == 0)
                return;

            EditSelected();
        }
    }
}
