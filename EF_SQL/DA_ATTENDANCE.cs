using System;
namespace EF_SQL
{
    public class DA_ATTENDANCE
    {
        public Guid ID { get; set; }
        public DateTime? ID_DATE { get; set; }//用户
        public string ID_LINE { get; set; }
        public string ID_MANAGER { get; set; }//类型
        public string ID_ONWORK { get; set; }//名称
        public string ID_OFFWORK { get; set; }
        public string ID_OUTWORK { get; set; }
        public string ID_FREEWORK { get; set; }//消耗
        public string ID_NONWORK { get; set; }

        public string ID_CODE { get; set; }
        public string ID_MODEL { get; set; }
        public string ID_TIME { get; set; }
        public string ID_TUSER { get; set; }
        public string ID_CLASS { get; set; }

        public DateTime? CRT_DATE { get; set; }
        public string CRT_USER { get; set; } 
    }
}
