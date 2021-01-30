using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES.Model
{
    public class DT_BEAT
    {
        //internal object dtMax;

        public string minNb { get; set; }
        public string maxNb { get; set; }
        public string malNb { get; set; }
        public string mtpNb { get; set; }
        public string Ave { get; set; }
        public string ValTb { get; set; }
        public int TYP1 { get; set; }
        public List<TABLE_VIEW> CloumTb { get; set; }
        public List<TIME_VIEW> RowTb { get; set; }
        public int CNaC { get; set; }
        public List<TIME_VIEW> ReTb { get; internal set; }
        public string Fty { get; internal set; }
        public string TrePairct { get; internal set; }
        public string TALLDT { get; internal set; }
        public string Cqy { get; internal set; }
        public string DTMAX { get; set; }
        public List<TIME_VIEW> LisViewDt { get; internal set; }
    }
}
