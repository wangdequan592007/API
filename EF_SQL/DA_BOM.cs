using System;
using System.ComponentModel;

namespace EF_SQL
{
    public class DA_BOM
    {
        public Guid Id { get; set; }
        public string BOM { get; set; }
        public string PRICE { get; set; } 
        public DateTime? CRT_DATE { get; set; }
        public string CRT_USER { get; set; }
        public string MODEL { get; set; }
        public string CLIENTNAME { get; set; }
        public string CLIENTCODE { get; set; }
    }
}
