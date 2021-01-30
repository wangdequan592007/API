using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_COM.Modelclass
{
    public class DT_QACHECK_DATA
    {
        public string DN1 { get; set; }
        public string INPUTDATE { get; set; }
        public string INPUTER { get; set; }
        public string V_DELIVERY_QUANTITY { get; set; }
        public string V_PO { get; set; }
        public string V_POLINE { get; set; }
        public string V_PONB { get; set; }
        public string V_PN { get; set; }
        public string V_CPN { get; set; }
        public string V_PROD_DESC_CUST { get; set; }
        public string T100MO { get; set; }
        public string V_CODE { get; set; }
        public string V_NAME { get; set; }
        public string V_DELIVERY_ID { get; set; }
        public string V_LOGISTICSORDER { get; set; }
        public string V_LOGISTICS { get; set; }
        public string V_ADDRESS { get; set; }
        public string V_DELIEVRYDATE { get; set; }
        public string V_WAREHOUSE_ID { get; set; }
        public string V_DELIVERY_STOCK { get; set; }
        public string V_SHIPUSER { get; set; }
        public string V_SHIPADDRESS { get; set; }

        public string SSCC { get; set; }
        public string DNTYPE { get; internal set; }
        public string CRT_USER { get; internal set; }
        public string CRT_DATE { get; internal set; }
        public string V_EMSCODE { get; internal set; }
    }
}
