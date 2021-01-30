using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model.TMES
{
    [Table("MESTOOL")]
    public class MESTOOL
    {
        [Key]
        [MaxLength(50)]
        [Column("MESTOOL")]
        public string TOOL { get; set; }
        [MaxLength(50)]
        [Column("TOOLVER")]
        public string TOOLVER { get; set; }
    }
}
