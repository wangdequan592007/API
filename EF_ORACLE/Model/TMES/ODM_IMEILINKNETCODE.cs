using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model.TMES
{
    [Table("ODM_IMEILINKNETCODE")]
    public class ODM_IMEILINKNETCODE
    {
        [Key]
        [MaxLength(64)]
        [Column("PHYSICSNO")]
        public string PHYSICSNO { get; set; }
        [MaxLength(64)]
        [Column("WORKORDER")]
        public string WORKORDER { get; set; }
        [MaxLength(64)]
        [Column("SN")]
        public string SN { get; set; }
        [MaxLength(64)]
        [Column("IMEI2")]
        public string IMEI2 { get; set; }
        [MaxLength(64)]
        [Column("MEID")]
        public string MEID { get; set; }
    }
}
