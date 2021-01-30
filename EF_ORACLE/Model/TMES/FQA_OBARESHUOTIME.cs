using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model.TMES
{
    [Table("FQA_OBARESHUOTIME")]
    public class FQA_OBARESHUOTIME
    {
        [Key] 
        [Column("ITEMCODE")] //指定数据库对应表栏位名称
        [MaxLength(64)]
        public string ITEMCODE { get; set; }
        [Column("DLTIME")]
        public int? DLTIME { get; set; }
        [Column("CRT_DATE", TypeName = "DATE")]
        public DateTime? CRT_DATE { get; set; }
    }
}
