using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES.Dtos
{
    public class DA_ODM_PRESSFREE
    {
        public string BARCODE { get; set; }
        public string TYPE { get; set; }
        public string CREAT_TIME { get; set; }
        public string CREAT_PERSON { get; set; }
        public string LINKSN { get; set; }
        public string FRR_TIME { get; set; }
        public string CRT_USER { get; set; }
    }
}
