using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tabmgr
{
    internal static class ImportMachine
    {
        public static bool initp = false;
        public static Thread BGloadjson = null;
        static public List<Tab> list;
        static public int index = 0;
        public static int last;


        public static void ctor()
        {
            list = new List<Tab>();
        }
    }

}
