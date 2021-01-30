using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_COM.Modelclass
{
   
    public class pOrderReceive
    {
        public pOrderReceive()
        {
            data = new List<Datum>();
        } 
        public string appKey { get; set; }
        public string appMethod { get; set; }
        public List<Datum>data { get; set; }


    }

    public class Datum
    {
        public string delivery_id { get; set; }
        public string batch_no { get; set; }
        public string batch_line { get; set; }
        public string prod_code_cust { get; set; }
        public string actual_inbound_qty { get; set; }
        public string actual_inbound_date { get; set; }
        public string cust_purchaseorder { get; set; }
        public string cust_purchaseline { get; set; }
        public string totalreceiv_qty { get; set; }
    }

}
