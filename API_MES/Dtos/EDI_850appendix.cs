using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES.Dtos
{
    public class EDI_850appendix
    {
        public string PONUMBER { get; set; }
        public string SHIPEARLYSTATUS { get; set; }
        public string STOP_RELEASE { get; set; }
        public string STOP_SHIP { get; set; }
        public string SHIP_TO_NAME1 { get; set; }
        public string MATERIALDES { get; set; }
        public string SHIP_TO_ADDRESS1 { get; set; }
      

    }
}
