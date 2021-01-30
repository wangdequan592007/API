using System;
namespace EF_SQL
{
    public class DT_INSTORE
    {
        public Guid ID { get; set; } 
        public string DT_MODEL { get; set; }// 
        public string DT_NAME { get; set; }// 
        public string DT_BOM { get; set; }// 
        public string DT_PRICE { get; set; }// 
        public string DT_COUNT { get; set; }//  
        public DateTime? DT_DATE { get; set; }
        public DateTime? CRT_DATE { get; set; }
        public string CRT_USER { get; set; }
        public string KD_USER { get; set; }
    }
}
