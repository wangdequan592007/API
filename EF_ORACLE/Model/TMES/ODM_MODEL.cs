using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model.TMES
{
    [Table("ODM_MODEL")]
    public class ODM_MODEL
    {
        [Key] 
        [MaxLength(64)]
        [Column("BOM")] //指定数据库对应表栏位名称 
        public string BOM { get; set; }
        [MaxLength(64)]
        [Column("PRODUCT_MODEL")]
        public string PRODUCT_MODEL { get; set; }
        [MaxLength(64)]
        [Column("MODEL")]
        public string MODEL { get; set; }
    }
}
