using System;
namespace API_MES.Dtos
{
    public class ODM_TEMPERATUREBOARD_DTOS
    { 
        public string DID { get; set; } 
        public string SN { get; set; }
        public int? FSTATUS { get; set; }
        public int? FSTATUS_DTL { get; set; } 
        public string REMART { get; set; } 
        public DateTime? CRTTIME { get; set; } 
        public string CRTUSER { get; set; }
        public int? FCOUNT { get; set; }
    }
}
