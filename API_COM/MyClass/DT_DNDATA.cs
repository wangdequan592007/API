using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_COM.Modelclass
{
    public class DT_DNDATA
    {
        public string V_DN { get; set; }
        public string V_PO { get; set; }
        public string V_POLINE { get; set; }
        public string V_PN { get; set; }
        public string V_PROD_DESC_CUST { get; set; }
        public string V_DELIVERY_QUANTITY { get; set; }
        public string V_LOGISTICSORDER { get; set; }
        public string V_LOGISTICS { get; set; }
        public string V_CPN { get; internal set; }
    }
}
