using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model.TMES
{
    [Table("ODM_SIDKEY")]
    public class ODM_SIDKEY
    {
        [Key]
        [Column("SID")]
        [MaxLength(50)]
        public string SID { get; set; }
        [Column("SN")]
        [MaxLength(50)]
        public string SN { get; set; }
        [Column("KEY")]
        [MaxLength(50)]
        public string KEY { get; set; }
        [Column("CREATE_USER")]
        [MaxLength(50)]
        public string CREATE_USER { get; set; }
        [Column("CREATE_DATE", TypeName = "DATE")]
        public DateTime? CREATE_DATE { get; set; }
        [Column("ISUSER")]
        [MaxLength(5)]
        public int? ISUSER { get; set; }
    }
}
