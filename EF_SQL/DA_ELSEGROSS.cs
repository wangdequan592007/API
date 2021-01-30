using System;
namespace EF_SQL
{
    public class DA_ELSEGROSS
    {
        public Guid Id { get; set; }
        public string DT_CLIENTNAME { get; set; }
        public string DT_CLIENTCODE { get; set; }
        public int? DT_MODEL { get; set; }
        public string DT_NAME { get; set; }
        public string DT_VALUE { get; set; }
        public DateTime? DT_DATE { get; set; }
        public DateTime? CRT_DATE { get; set; }
        public string CRT_USER { get; set; }
    }
}
