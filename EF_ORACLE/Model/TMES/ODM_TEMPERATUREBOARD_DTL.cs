using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model.TMES
{
    [Table("ODM_TEMPERATUREBOARD_DTL")]
    public class ODM_TEMPERATUREBOARD_DTL
    {
        [Key]
        [MaxLength(50)]
        [Column("DID")]
        public string DID { get; set; }
        [MaxLength(60)]
        [Column("SN")]
        public string SN { get; set; } 
        [Column("FSTATUS")]
        public int? FSTATUS { get; set; }
        [MaxLength(100)]
        [Column("REMART")]
        public string REMART { get; set; }
        [Column("CRTTIME", TypeName = "DATE")]
        public DateTime? CRTTIME { get; set; }
        [MaxLength(20)]
        [Column("CRTUSER")]
        public string CRTUSER { get; set; } 
    }
}
