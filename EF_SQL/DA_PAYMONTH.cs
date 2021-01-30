using System;
namespace EF_SQL
{
    public class DA_PAYMONTH
    {
        public Guid Id { get; set; }
        public string DT_CLIENTNAME { get; set; }//用户
        public int? DT_MODEL { get; set; }//类型
        public string DT_NAME { get; set; }//名称
        public string DT_EXPEND { get; set; }
        public DateTime? DT_DATE { get; set; }
        public DateTime? CRT_DATE { get; set; }
        public string CRT_USER { get; set; }
        public string DT_REMARKS { get; set; }
    }
}
