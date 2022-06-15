using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUI
{
    public partial class KeyValueDialog : Form
    {
        public KeyValueDialog()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        public string Value { get; internal set; }
        public string Key { get; internal set; }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Key = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Value = textBox2.Text;
        }

        internal void Init(KeyValuePair<string, object> pair)
        {
            textBox1.ReadOnly = true;
            textBox1.Text = pair.Key;
            textBox2.Text = pair.Value.ToString();
        }
    }
}
