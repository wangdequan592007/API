using System;
using System.ComponentModel.DataAnnotations;

namespace EF_SQL
{
    public class DA_CLENTNAME
    { 
        public string CODE { get; set; }
        [Key]
        public string NAME { get; set; }
        public int? BU { get; set; }
        public string PAY1 { get; set; }
        public string PAY2 { get; set; }
        public string PAY3 { get; set; }
    }
}
