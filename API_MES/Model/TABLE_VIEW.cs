using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES.Model
{
    public class TABLE_VIEW
    {
        public string field { get; set; }
        public string title { get; set; }
        public string align { get; set; }

        public int width { get; set; }
        public cellStyle cellStyle { get; set; }
       
    }
    public class cellStyle
    {
        public string css { get; set; } 
    }
}
