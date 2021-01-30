using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model.TMES
{
    [Table("ODM_LOCKMO")]
    public class LOCKMO
    { 
        [MaxLength(64)]
        [Column("MO")] //指定数据库对应表栏位名称 
        public string MO { get; set; }
        [MaxLength(64)]
        [Column("LINE")] //指定数据库对应表栏位名称 
        public string LINE { get; set; }
        [Column("ISLOCK")]
        public int? ISLOCK { get; set; }
        [Column("CRT_DATE", TypeName = "DATE")]
        public DateTime? CRT_DATE { get; set; }
        [MaxLength(50)]
        [Column("CRT_USER")]
        public string CRT_USER { get; set; }
        [MaxLength(200)]
        [Column("DESCRIBE")]
        public string DESCRIBE { get; set; }
    }
}
