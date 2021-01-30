using System;
using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EF_ORACLE.Model
{
    [Table("WORK_WORKJOB")]
   public class ModelMO
    {
        [Key]
        // [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]//不自动增长
        [MaxLength(64)]
        [Column("WORKJOB_CODE")] //指定数据库对应表栏位名称 CLIENTCODE
        public string WORKJOB_CODE { get; set; }
        [MaxLength(64)]
        [Column("ITEM_CODE")]
        public string ITEM_CODE { get; set; }
        [MaxLength(64)]
        [Column("CLIENTNAME")]
        public string CLIENTNAME { get; set; }
        [MaxLength(64)]
        [Column("CLIENTCODE")]
        public string CLIENTCODE { get; set; }
        [Column("ITEM_NUM")]
        //[System.ComponentModel.DefaultValue(0)]
        public int? ITEM_NUM { get; set; }
        [Column("INPUTDATE", TypeName = "DATE")]
        public DateTime? INPUTDATE { get; set; }
        [MaxLength(64)]
        [Column("BEGIN_BARCODE")]
        public string BEGIN_BARCODE { get; set; }
        [MaxLength(64)]
        [Column("END_BARCODE")]
        public string END_BARCODE { get; set; }
        [MaxLength(64)]
        [Column("WORKORDER")]
        public string WORKORDER { get; set; }
        [MaxLength(64)]
        [Column("EMS_CODE")]
        public string EMS_CODE { get; set; }
        [MaxLength(2000)]
        [Column("ITEM_TYPE")]
        public string ITEM_TYPE { get; set; }
    }
}
