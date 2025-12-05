using AutoUI.CodeEditor;
using AutoUI.Common;
using AutoUI.Editors;
using System;
using System.IO;
using System.Windows.Forms;

namespace AutoUI
{
    public partial class ResourcesDialog : Form
    {
        public ResourcesDialog()
        {
            InitializeComponent();
        }
        TestSet Set;
        internal void Init(TestSet set)
        {
            Set = set;
            UpdateList();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            Set.Resources.Add(new TextAutoTestResource()
            {
                Name = Path.GetFileName(ofd.FileName),
                Path = $"Resources\\" + Path.GetFileName(ofd.FileName),
                ResourceLoadType = ResourceLoadTypeEnum.External,
                Text = File.ReadAllText(ofd.FileName)
            });
            UpdateList();
        }

        private void UpdateList()
        {
            listView1.Items.Clear();
            foreach (var item in Set.Resources)
            {
                listView1.Items.Add(new ListViewItem([
                    item.Name,
                    item.GetType().Name,
                    item.ResourceLoadType.ToString(), 
                    item.Path,                    
                ])
                {
                    Tag = item
                });
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            if (!UIHelpers.ShowQuestion($"Are you sure you want to delete selected resources ({listView1.SelectedItems.Count})?"))
                return;

            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                var item = listView1.SelectedItems[i].Tag as AutoTestResource;
                Set.Resources.Remove(item);
            }
            UpdateList();
        }

        private void contentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var item = listView1.SelectedItems[0].Tag as TextAutoTestResource;
            TextEditor ted = new TextEditor();
            ted.Save += () =>
            {
                item.Text = ted.Editor.Text;
            };
            ted.Init(item.Text);
            ted.MdiParent = MdiParent;
            ted.Show();

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var item = listView1.SelectedItems[0].Tag as TextAutoTestResource;
            var d = AutoDialog.DialogHelpers.StartDialog();
            d.AddStringField("name", "Name", item.Name);
            d.AddStringField("path", "Path", item.Path);
            d.AddOptionsField("location", "Location", Enum.GetNames<ResourceLoadTypeEnum>(), item.ResourceLoadType.ToString());
            d.AddOptionsField("internalStorageMode", "Internal storage mode", Enum.GetNames<InternalResourceStorageModeEnum>(), item.InternalStorageMode.ToString());

            if (!d.ShowDialog())
                return;

            item.Name = d.GetStringField("name");
            item.Path = d.GetStringField("path");
            item.ResourceLoadType = Enum.Parse<ResourceLoadTypeEnum>(d.GetOptionsField("location"));
            item.InternalStorageMode = Enum.Parse<InternalResourceStorageModeEnum>(d.GetOptionsField("internalStorageMode"));
            UpdateList();
        }
    }
}
