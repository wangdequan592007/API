using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model.TMES
{
    [Table("EDI_SSCCINFOLOG")]
    public class EDI_SSCCINFOLOG
    {
        [MaxLength(64)]
        [Column("SSCC")]
        public string SSCC { get; set; }
        [MaxLength(64)]
        [Column("BOXSN")]
        public string BOXSN { get; set; }
        [MaxLength(64)]
        [Column("RESULT")]
        public string RESULT { get; set; }
        [Column("CRT_DATE", TypeName = "DATE")]
        public DateTime? CRT_DATE { get; set; }
        [MaxLength(64)]
        [Column("CRT_USER")]
        public string CRT_USER { get; set; }
    }
}
