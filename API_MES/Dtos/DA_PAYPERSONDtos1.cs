using System;
namespace API_MES.Dtos
{
    public class DA_PAYPERSONDtos1
    {
        public string DT_CLIENTNAME { get; set; }//用户 
        public string DT_NAME { get; set; }//名称
        public string DT_INDIRECTWORKTIME { get; set; }//间接人员出勤数 
        public string DT_INDIRECTMOUTH { get; set; }//间接人员月薪
        public string DT_DIRECTHOUR { get; set; }//直接人员时薪 
        public string DT_DATE { get; set; } 
        public string CRT_USER { get; set; }
        public string DT_REMARKS { get; set; }
    }
}
