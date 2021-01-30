using System;
namespace API_MES.Dtos
{
    public class DA_GROSSDtos
    {
        public string DT_CLIENTNAME { get; set; }
        public string DT_CLIENTCODE { get; set; }
        public string DT_NAME { get; set; }
        public string DT_MO { get; set; }
        public string DT_BOM { get; set; }
        public string DT_PRICE { get; set; }
        public string DT_STORENB { get; set; }
        public string DT_MANAGER { get; set; }//拉长
        public string DT_MANNAME { get; set; }//拉长
        public string DT_ON { get; set; }
        public string DT_IN { get; set; }
        public string DT_OUT { get; set; }
        public string DT_OFFWORK { get; set; }
        public string DT_ONWORK { get; set; }
        public string DT_PERSONHOUR { get; set; }
        public string DT_DATE { get; set; } 
        public string CRT_USER { get; set; }
        public string DT_DIRECTHOUR { get; set; }
        public string DT_MODELNAME { get; set; }
    }
}
