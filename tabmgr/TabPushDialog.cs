using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tabmgr
{
    public partial class TabPushDialog : Form
    {
        public static Tab added=null;
        public TabPushDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains(".") | !(textBox1.Text.StartsWith("http"))) textBox1.Text = "https://"+textBox1.Text;
            added = new Tab(textBox1.Text, textBox2.Text);
            ImportMachine.list.Add(added);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
