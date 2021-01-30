using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES.Dtos
{
    public class ListDataList
    {
        public List<SenDataList> SENNUMBER { get; set; }
        public List<SenDataList> RENUMBER { get; set; }
        public List<SenDataList> RTNUMBER { get; set; }
        public List<SenDataList> PIERES { get; set; }
        public List<SenDataList> PIESTATION { get; set; }
        public List<SenDataList> ERR_DESCRIBE { get; set; }
    }
}
