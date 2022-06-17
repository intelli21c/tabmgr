namespace tabmgr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\_tabdbdebug.json"))
            {
                DataHandler.datahandler = Newtonsoft.Json.JsonConvert.DeserializeObject<DataHandler>(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\_tabdbdebug.json"));
            }
            else DataHandler.datahandler=new DataHandler();
        }
        public enum updatedinfoenum
        {
            all, index, listadd, listrem, viewntab, tabinfo, clearall
        }
        public void updatetabviewinfo(updatedinfoenum updated = updatedinfoenum.all, object opt = null)
        {
            switch (updated)
            {
                case updatedinfoenum.all:
                //update all

                case updatedinfoenum.index:
                    //index changed then change focus to index and update correspondingly
                    checkedListBox1.SelectedIndexChanged -= checkedListBox1_SelectedIndexChanged;
                    checkedListBox1.SelectedIndex = ImportMachine.index;
                    label5.Text = ImportMachine.list[ImportMachine.index].Name;
                    webView21.Source = new System.Uri(ImportMachine.list[ImportMachine.index].URL);
                    checkedListBox1.SelectedIndexChanged += checkedListBox1_SelectedIndexChanged;
                    break;
                case updatedinfoenum.listadd:
                    //only list updated(push delete load or so.)
                    checkedListBox1.Items.Add(((Tab)opt).Name);
                    break;
                case updatedinfoenum.listrem:
                    checkedListBox1.Items.RemoveAt(ImportMachine.index);
                    break;
                case updatedinfoenum.viewntab:
                    webView21.Source = new System.Uri(ImportMachine.list[ImportMachine.index].URL);
                    goto case updatedinfoenum.tabinfo;
                case updatedinfoenum.tabinfo:
                    label5.Text = ImportMachine.list[ImportMachine.index].Name;
                    //label1.Text = webView21.CoreWebView2.DocumentTitle;
                    break;
                case updatedinfoenum.clearall:
                    checkedListBox1.Items.Clear();
                    webView21.Source = new Uri("http://example.com");
                    label1.Text = "";
                    label2.Text = "";
                    break;
                default:
                    break;
            }
        }


        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox1 = new AboutBox1();
            aboutBox1.ShowDialog();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ImportMachine.index = checkedListBox1.SelectedIndex == -1 ? ImportMachine.index : checkedListBox1.SelectedIndex;
            updatetabviewinfo(updatedinfoenum.index);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (ImportMachine.index + 1 < ImportMachine.list.Count)
            {
                ImportMachine.list[ImportMachine.index].addp = false;
                checkedListBox1.SetItemChecked(ImportMachine.index, false);
                ImportMachine.index++;
                updatetabviewinfo(updatedinfoenum.index);
            }
        }
        private void initunlockbtns()
        {
            button3.Enabled = true;
            button8.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button2.Enabled = true;
            button4.Enabled = true;
            button16.Enabled = true;
            button15.Enabled = true;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (ImportMachine.initp == false)
            {
                ImportMachine.ctor();
                ImportMachine.initp = true;
                initunlockbtns();
                this.button1.Text = "More";
            }
            TabPushDialog tabPushDialog = new TabPushDialog();
            tabPushDialog.ShowDialog();
            if (TabPushDialog.added != null) updatetabviewinfo(updatedinfoenum.listadd, TabPushDialog.added);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ImportMachine.initp == false)
            {
                ImportMachine.ctor();
                /*((ListBox)checkedListBox1).ValueMember = "addp";
                ((ListBox)checkedListBox1).DisplayMember = "Name";
                ((ListBox)checkedListBox1).DataSource = ImportMachine.list;*/
                //finally initialise import logics if valid.
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        ImportJSONMatchDialog importJSONMatchDialog = new ImportJSONMatchDialog();
                        importJSONMatchDialog.ShowDialog();
                        break;
                }
                ImportMachine.initp = true;
                foreach (var x in ImportJSONMatchDialog.added)
                {
                    ImportMachine.list.Add(x);
                    checkedListBox1.Items.Add(x.Name);
                }
                initunlockbtns();
                this.button1.Text = "More";
            }
            else
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        ImportJSONMatchDialog importJSONMatchDialog = new ImportJSONMatchDialog();
                        importJSONMatchDialog.ShowDialog();
                        break;
                }
                ImportMachine.initp = true;
                foreach (var x in ImportJSONMatchDialog.added)
                {
                    ImportMachine.list.Add(x);
                    checkedListBox1.Items.Add(x.Name);
                }
            }
        }

        private void hideStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (statusStrip1.Visible)
            {
                statusStrip1.Visible = false;
                hideStatusToolStripMenuItem.Checked = false;
            }
            else
            {
                statusStrip1.Visible = true;
                hideStatusToolStripMenuItem.Checked = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ImportMachine.index > 0)
            {
                ImportMachine.index--;
                updatetabviewinfo(updatedinfoenum.index);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (ImportMachine.index + 1 < ImportMachine.list.Count)
            {
                ImportMachine.index++;
                updatetabviewinfo(updatedinfoenum.index);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete all imported tabs?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ImportMachine.list.Clear();
                ImportMachine.index = 0;
                updatetabviewinfo(updatedinfoenum.clearall);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            ImportMachine.list.RemoveAt(ImportMachine.index);
            checkedListBox1.Items.RemoveAt(ImportMachine.index);
            updatetabviewinfo(updatedinfoenum.index);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ImportMachine.index + 1 < ImportMachine.list.Count)
            {
                ImportMachine.list[ImportMachine.index].addp = true;
                checkedListBox1.SetItemChecked(ImportMachine.index, true);
                ImportMachine.index++;
                updatetabviewinfo(updatedinfoenum.index);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //webView21.CoreWebView2.DocumentTitle;
        }

        private void button13_Click(object sender, EventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //check list sanity
            MessageBox.Show(null, "Adding"+1+"Tabs", "Adding Tabs", MessageBoxButtons.YesNoCancel);
            DataHandler.datahandler.tabs.AddRange(ImportMachine.list);
            //ImportMachine.list.Clear();
            foreach(var tab in ImportMachine.list)
            {
                if(tab.addp==true)
                {
                    listView2.Items.Add(tab.Name);
                }
            }
            //updatetabviewinfo(updatedinfoenum.all);
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button23_Click(object sender, EventArgs e)
        {
            checkedListBox2.Items.Add("");
        }

        private void button25_Click(object sender, EventArgs e)
        {
            checkedListBox2.SelectedIndexChanged -= checkedListBox2_SelectedIndexChanged;
            checkedListBox2.Items[checkedListBox2.SelectedIndex] = richTextBox1.Text;
            checkedListBox2.SelectedIndexChanged += checkedListBox2_SelectedIndexChanged;
        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = checkedListBox2.SelectedItem.ToString();
        }

        private void button35_Click(object sender, EventArgs e)
        {

        }

        private void button19_Click(object sender, EventArgs e)
        {
            checkedListBox2.SelectedIndexChanged -= checkedListBox2_SelectedIndexChanged;
            var ti = checkedListBox2.SelectedIndex;
            checkedListBox2.Items.RemoveAt(checkedListBox2.SelectedIndex);
            checkedListBox2.SelectedIndex = ti - 1;
            checkedListBox2.SelectedIndexChanged += checkedListBox2_SelectedIndexChanged;
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }
    }
}