using System;
namespace EF_SQL
{
    public class DA_DATEINSTORE
    {
        public Guid ID { get; set; }
        public DateTime? DT_DATE { get; set; }
        public string DT_LINE { get; set; }
        public string DT_MO { get; set; }
        public string DT_BOM { get; set; }
        public string DT_MODEL { get; set; }
        public string DT_PLAN { get; set; }
        public string DT_STORENB { get; set; }
        public string DT_MANAGER { get; set; }
        public string DT_MANNAME { get; set; }
        public string DT_PERSON { get; set; }
        public string DT_USERHOUR { get; set; }
        public string DT_OFFUSERNB { get; set; }
        public string DT_OFFTIMENB { get; set; }
        public string DT_CLASS { get; set; }
        public string DT_REMARK { get; set; }
        public DateTime? CRT_DATE { get; set; }
        public string CRT_USER { get; set; }
        public string DT_TYPE { get; set; }
    }
}
