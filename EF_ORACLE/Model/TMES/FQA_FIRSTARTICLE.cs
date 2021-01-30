using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EF_ORACLE.Model
{
    [Table("FQA_FIRSTARTICLE")]
    public  class FQA_FIRSTARTICLE
    {
        //[Key]
        [Column("MO")] //指定数据库对应表栏位名称
        [MaxLength(64)]
        public string MO { get; set; }
        [MaxLength(10)]
        [Column("LINE")]
        public string LINE { get; set; }
        //[Key]
        [Column("MTYPE")]
        public int? MTYPE { get; set; }
        [MaxLength(100)]
        [Column("MODEL")]
        public string MODEL { get; set; } 
        [Column("CREAT_DT", TypeName = "DATE")]
        public DateTime? CREAT_DT { get; set; }
        [MaxLength(50)]
        [Column("CREAT_US")]
        public string CREAT_US { get; set; }
        [Column("DTYPE")]
        public int? DTYPE { get; set; }
        [MaxLength(1000)]
        [Column("NOTES")]
        public string NOTES { get; set; }
    }
}
