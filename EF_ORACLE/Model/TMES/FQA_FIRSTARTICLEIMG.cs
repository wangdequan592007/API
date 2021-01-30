using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model.TMES
{
    [Table("FQA_FIRSTARTICLEIMG")]
    public class FQA_FIRSTARTICLEIMG
    {
        [Key]
        [Column("ID")] //指定数据库对应表栏位名称 
        public string ID { get; set; }
        [Column("MO")] //指定数据库对应表栏位名称
        [MaxLength(64)]
        public string MO { get; set; }
        [MaxLength(10)]
        [Column("LINE")]
        public string LINE { get; set; }
        //[Key]IMGNEW
        [Column("MTYPE")]
        public int? MTYPE { get; set; }
        [MaxLength(64)]
        [Column("IMGNAME")]
        public string IMGNAME { get; set; }
        [MaxLength(100)]
        [Column("IMGLIST")]
        public string IMGLIST { get; set; }
        [MaxLength(64)]
        [Column("IMGNEW")]
        public string IMGNEW { get; set; }
    }
}
