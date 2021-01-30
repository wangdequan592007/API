using System;
namespace API_MES.Dtos
{
    public class FQA_CK_SNLOGDtos
    { 
        public string MO { get; set; } 
        public string SN { get; set; } 
        public string LINE { get; set; } 
        public string ISOK { get; set; } 
        public string PCNAME { get; set; } 
        public DateTime? CREATEDATE { get; set; } 
        public int? TLL { get; set; } 
        public string T_DES { get; set; }  
    }
}
