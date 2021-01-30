using System;
namespace API_MES.Dtos
{
    public class DA_ELSEGROSSDtos
    {
        public Guid Id { get; set; }
        public string DT_CLIENTNAME { get; set; }
        public string DT_CLIENTCODE { get; set; }
        public int? DT_MODEL { get; set; }
        public string DT_NAME { get; set; }
        public string DT_VALUE { get; set; }
        public string DT_DATE { get; set; }
        public DateTime? CRT_DATE { get; set; }
        public string CRT_USER { get; set; }
    }
}
