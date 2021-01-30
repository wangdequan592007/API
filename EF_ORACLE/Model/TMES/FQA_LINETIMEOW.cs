using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model.TMES
{
    [Table("FQA_LINETIMEOW")]
    public class FQA_LINETIMEOW
    {
        [Key] 
        [MaxLength(50)]
        [Column("ID")]
        public string ID { get; set; }
        [MaxLength(50)]
        [Column("LINE")]
        public string LINE { get; set; }
        [MaxLength(10)]
        [Column("TIME1")]
        public string TIME1 { get; set; }
        [MaxLength(10)]
        [Column("TIME2")]
        public string TIME2 { get; set; }
        [Column("CREAT_DT", TypeName = "DATE")]
        public DateTime? CREAT_DT { get; set; }
    }
}
