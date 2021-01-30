using System;
namespace EF_SQL
{
    public class DA_PAYPERSON
    {
        public Guid Id { get; set; }
        public string DT_CLIENTNAME { get; set; }//用户
        public int? DT_MODEL { get; set; }//类型
        public string DT_NAME { get; set; }//名称
        public string DT_INDIRECTWORKTIME { get; set; }//间接人员出勤数
        public string DT_INDIRECTWAGE { get; set; }//间接人员工资
        public string DT_INDIRECTMOUTH { get; set; }//间接人员月薪
        public string DT_DIRECTHOUR { get; set; }//直接人员时薪
        public string DT_WAGE { get; set; }
        public DateTime? DT_DATE { get; set; }
        public DateTime? CRT_DATE { get; set; }
        public string CRT_USER { get; set; }
        public string DT_REMARKS { get; set; }
    }
}
