using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model
{
    [Table("WORKPRODUCE")]
    public class WORKPRODUCE
    {
        [Key]
        [MaxLength(32)]
        [Column("LINE")] //指定数据库对应表栏位名称 
        public string LINE { get; set; } 
        [MaxLength(64)]
        [Column("WORKSHOP2")] //指定数据库对应表栏位名称 
        public string WORKSHOP2 { get; set; }
    }
}
