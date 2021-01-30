using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES.Dtos
{
    public class DA_ODM_PRESSURE
    {
        public string ID { get; set; }
        public string BARCODE { get; set; }
        public string TYPE { get; set; }
        public string CREAT_TIME { get; set; }
        public string MO { get; set; }
        public string LINE { get; set; }
        public string CREAT_PERSON { get; set; }
        public string SEG1 { get; set; }
        public string SEG2 { get; set; }
        public string SEG3 { get; set; }
        public string CLIENTCODE { get; internal set; }
    }
}
