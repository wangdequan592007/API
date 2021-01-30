using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model.TMES
{
    [Table("FQA_CK_SNLOG")]  
    public class FQA_CK_SNLOG
    {
        [Key]
        [MaxLength(50)]
        [Column("TGUID")]
        public string TGUID { get; set; }
        [MaxLength(50)]
        [Column("MO")]
        public string MO { get; set; }
        [MaxLength(50)]
        [Column("SN")]
        public string SN { get; set; }
        [MaxLength(50)]
        [Column("LINE")]
        public string LINE { get; set; }
        [Column("ISOK")]
        public int? ISOK { get; set; }
        [MaxLength(100)]
        [Column("PCNAME")]
        public string PCNAME { get; set; }
        [Column("CREATEDATE", TypeName = "DATE")]
        public DateTime? CREATEDATE { get; set; }
        [Column("TLL")]
        public int? TLL { get; set; }
        [MaxLength(100)]
        [Column("T_DES")]
        public string T_DES { get; set; } 
        [Column("SUBID")]
        public int? SUBID { get; set; }
    }
}
