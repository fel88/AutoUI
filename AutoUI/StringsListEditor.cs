using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace AutoUI
{
    public partial class StringsListEditor : Form
    {
        public StringsListEditor()
        {
            InitializeComponent();
        }

        private void addKeyvaleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var d = AutoDialog.DialogHelpers.StartDialog();

            d.AddStringField("key", "Key", "new_key");
            d.AddStringField("value", "Value", "");

            if (!d.ShowDialog())
                return;

            Dictionary.Add(d.GetStringField("key"), d.GetStringField("value"));            
            UpdateKeyValueList();
        }

        void UpdateKeyValueList()
        {
            listView3.Items.Clear();
            foreach (var item in Dictionary)
            {
                listView3.Items.Add(new ListViewItem(new string[] { item.Key, item.Value.ToString() }) { Tag = item });
            }
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count == 0)
                return;

            var p = (KeyValuePair<string, string>)listView3.SelectedItems[0].Tag;

            Dictionary.Remove(p.Key);

            UpdateKeyValueList();
        }

        private void listView3_MouseClick(object sender, MouseEventArgs e)
        {

        }

        Dictionary<string, string> Dictionary;
        public void Init(Dictionary<string, string> set, bool isEditable = true)
        {
            IsEditable = isEditable;
            Dictionary = set;
            listView3.Items.Clear();
            foreach (var item in set)
            {
                listView3.Items.Add(new ListViewItem(new string[] { item.Key, item.Value.ToString() }) { Tag = item });
            }
        }
        private void listView3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EditSelected();
        }

        public bool IsEditable = true;

        private void EditSelected()
        {
            var pair = (KeyValuePair<string, string>)listView3.SelectedItems[0].Tag;

            var d = AutoDialog.DialogHelpers.StartDialog();

            d.AddStringField("key", "Key", pair.Key);
            d.AddStringField("value", "Value", pair.Value);
            d.CreatedControls["key"][1].Enabled = false;
            (d.CreatedControls["value"][1] as TextBox).ReadOnly = !IsEditable;

            if (!d.ShowDialog() || !IsEditable)
                return;

            Dictionary[pair.Key] = d.GetStringField("value");
            UpdateKeyValueList();
        }

        private void contextMenuStrip3_Opening(object sender, CancelEventArgs e)
        {
            deleteToolStripMenuItem1.Enabled = IsEditable;
            addKeyvaleToolStripMenuItem.Enabled = IsEditable;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count == 0)
                return;

            EditSelected();
        }
    }
}
