using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model.TMES
{
    [Table("ODM_LOCKREWORK")]
    public class ODM_LOCKREWORK
    {
        [Key]
        [MaxLength(150)]
        [Column("UDID")]
        public string UDID { get; set; }
        [Column("SUBID")]
        [MaxLength(5)]
        public int? SUBID { get; set; }
        [MaxLength(50)]
        [Column("SN")]
        public string SN { get; set; }
        [MaxLength(50)]
        [Column("MO")]
        public string MO { get; set; }
        [MaxLength(50)]
        [Column("STATION")]
        public string STATION { get; set; } 
        [Column("CRT_DATE", TypeName = "DATE")]
        public DateTime? CRT_DATE { get; set; }
        [MaxLength(50)]
        [Column("CRT_USER")]
        public string CRT_USER { get; set; }
        [Column("LOCKIN")]
        [MaxLength(5)]
        public int? LOCKIN { get; set; }
    }
}
