using System;
namespace API_MES.Dtos
{
    public class FQA_FIRSTARTICLEDtos
    { 
            public string MO { get; set; }
            public string BOM { get; set; }
            public string LINE { get; set; } 
            public int? MTYPE { get; set; } 
            public string MODEL { get; set; } 
            public DateTime? CREAT_DT { get; set; } 
            public string CREAT_US { get; set; }  
            public string NOTES { get; set; } 
    }
}
