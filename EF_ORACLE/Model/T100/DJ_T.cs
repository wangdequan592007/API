using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model.T100
{
    [Table("DJ_T")]  //指定数据库对应表名
    public class DJ_T
    {
        [Key]
        // [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]//不自动增长
        [Column("XMDL003")]
        public string XMDL003 { get; set; }
        [Column("XMDL004")]
        public int? XMDL004 { get; set; }
    }
}
