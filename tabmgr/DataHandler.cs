using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tabmgr
{
    public class Tab
    {
        public string Name;
        public string URL;
        public bool addp;
        List<int> tags;
        public Tab(string pURL, string pname = "NULL")
        {
            this.URL = pURL;
            if (pname != null) this.Name = pname;
        }
    }
    internal class DataHandler
    {
        public static DataHandler datahandler;  //UNIQUE INSTANCE SHOULD NOT BE CALLED RECURSIVELY
        public List<Tab> tabs;
        public DataHandler()
        {
            tabs = new List<Tab>();
        }
    }
}
