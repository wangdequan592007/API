using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model.TMES
{
    [Table("ODM_TEMPERATUREBOARD")]
    public class ODM_TEMPERATUREBOARD
    {
        [Key]
        [MaxLength(50)]
        [Column("SN")]
        public string SN { get; set; }
        [Column("FSTATUS")]
        public int? FSTATUS { get; set; }
        [Column("FCOUNT")]
        public int? FCOUNT { get; set; }
    }
}
