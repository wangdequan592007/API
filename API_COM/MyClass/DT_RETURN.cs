using API_COM.MyClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_COM.Modelclass
{
    public class DT_RETURN
    {
        public bool Success { get; set; }
        public string Service { get; set; }
        public string Msg { get; set; }
        public string Nw { get; set; }

        public string Gw { get; set; }
        public List<DT_SODATA> LSO_DATA { get; internal set; }
        public List<DT_DNDATA> LDN_DATA { get; internal set; }
        public List<DT_QACHECK_DATA> QA_DATA { get; internal set; }
        public List<DT_NETCODE> DT_NETCODE { get; internal set; }
    }
}
