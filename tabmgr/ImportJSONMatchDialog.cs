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
    public partial class ImportJSONMatchDialog : Form
    {
        public ImportJSONMatchDialog()
        {
            InitializeComponent();
        }

        private struct JSONTravesereturn_t
        {
            public TreeNode childs;
            public List<object> tree;
        };
        public static List<Tab> added=new List<Tab>();
        int unamobjcount = 0;
        List<string> arrayref = new List<string>();
        List<string> arrprop = new List<string>();
        JSONTravesereturn_t JSONTraverse(Newtonsoft.Json.Linq.JObject json, String name)
        {
            //will push for treeview at the same time
            JSONTravesereturn_t retv;
            retv.tree = new List<object>();
            retv.childs = new TreeNode(name);
            JSONTravesereturn_t prev;
            List<Object> tmp;
            foreach (Newtonsoft.Json.Linq.JProperty x in json.Properties().ToArray())
            {
                switch (x.Value.Type)
                {
                    case Newtonsoft.Json.Linq.JTokenType.Object:
                        prev = JSONTraverse((Newtonsoft.Json.Linq.JObject)x.Value, x.Name);
                        retv.childs.Nodes.Add(prev.childs);
                        tmp = new List<Object>();
                        tmp.Add(x.Name);
                        tmp.Add(prev.childs);
                        retv.tree.Add(tmp);
                        tmp = null;
                        break;
                    case Newtonsoft.Json.Linq.JTokenType.Array:
                        //register a array type to list
                        arrayref.Add(x.Value.Path);
                        var artmp = new List<object>();
                        JSONTravesereturn_t arretv;
                        arretv.tree = new List<object>();
                        arretv.childs = new TreeNode(x.Name + "(Array)");
                        JSONTravesereturn_t arprev;
                        foreach (Newtonsoft.Json.Linq.JToken y in ((Newtonsoft.Json.Linq.JArray)x.Value))
                        {
                            switch (y.Type)
                            {
                                case Newtonsoft.Json.Linq.JTokenType.Object:
                                    arprev = JSONTraverse((Newtonsoft.Json.Linq.JObject)y, "Unanymous Object " + unamobjcount.ToString());
                                    arretv.childs.Nodes.Add(arprev.childs);
                                    artmp = new List<Object>();
                                    artmp.Add(new string("Unanymous Object" + unamobjcount.ToString()));
                                    unamobjcount++;
                                    artmp.Add(arprev.childs);
                                    arretv.tree.Add(artmp);
                                    artmp = null;
                                    break;
                                case Newtonsoft.Json.Linq.JTokenType.String:
                                    retv.childs.Nodes.Add(new TreeNode(y.ToString()));
                                    artmp = new List<Object>();
                                    artmp.Add(new String("Unanymous String"));
                                    artmp.Add(y.ToString());
                                    arretv.tree.Add(artmp);
                                    artmp = null;
                                    break;
                                case Newtonsoft.Json.Linq.JTokenType.Null:
                                case Newtonsoft.Json.Linq.JTokenType.Undefined:
                                    break;
                            }

                        }
                        retv.childs.Nodes.Add(arretv.childs);
                        break;
                    case Newtonsoft.Json.Linq.JTokenType.String:
                        retv.childs.Nodes.Add(new TreeNode(x.Name.ToString() + " : " + x.Value.ToString()));
                        tmp = new List<Object>();
                        tmp.Add(x.Name.ToString());
                        tmp.Add(x.Value.ToString());
                        retv.tree.Add(tmp);
                        tmp = null;
                        break;
                    case Newtonsoft.Json.Linq.JTokenType.Null:
                    case Newtonsoft.Json.Linq.JTokenType.Undefined:
                        break;
                    default:
                        break;
                }
            }
            return retv;
        }
        Newtonsoft.Json.Linq.JObject json;
        private void JSONloadclass()
        {
            try
            {
                StreamReader stream = File.OpenText(textBox1.Text);
                var tgt = stream.ReadToEnd();
                stream.Close();
                json = Newtonsoft.Json.Linq.JObject.Parse(tgt);
                var rt = JSONTraverse(json, "Toplevel");
                treeView1.Nodes.Add(rt.childs);
                comboBox4.Items.AddRange(arrayref!.ToArray<String>());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                JSONloadclass();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0) JSONloadclass();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (var item in (Newtonsoft.Json.Linq.JArray)json.SelectToken(comboBox4.Items[comboBox4.SelectedIndex].ToString()))
            {
                added.Add(new Tab(((Newtonsoft.Json.Linq.JObject)item).SelectToken(comboBox1.Items[comboBox1.SelectedIndex].ToString()).ToString(), comboBox2.SelectedIndex == -1 ? "NULL" : ((Newtonsoft.Json.Linq.JObject)item).SelectToken(comboBox2.Items[comboBox2.SelectedIndex].ToString()).ToString()));
            }
            MessageBox.Show("Import Completed");
            this.Close();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (json != null)
            {
                try
                {
                    listBox1.Items.Clear();
                    arrprop.Clear();
                    foreach (var x in ((Newtonsoft.Json.Linq.JObject)json.SelectToken(comboBox4.Items[comboBox4.SelectedIndex].ToString())[0]).Properties())
                    {
                        listBox1.Items.Add(x.Name);
                        arrprop.Add(x.Name);
                    }
                    comboBox1.Items.Clear();
                    comboBox2.Items.Clear();
                    foreach (var x in arrprop)
                    {
                        comboBox1.Items.Add(x);
                        comboBox2.Items.Add(x);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button3.Enabled = true;
        }
    }
}
