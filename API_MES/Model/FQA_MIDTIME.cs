using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES.Model
{
    [Table("FQA_MIDTIME")]
    public class FQA_MIDTIME
    { /// <summary>
      /// 学生学号
      /// </summary>
        [Key]
        // [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]//不自动增长
        [Column("ITEMCODE")] //指定数据库对应表栏位名称
        [MaxLength(64)]
        public string ITEMCODE { get; set; } 
        [Column("DLTIME")]
        public int DLTIME { get; set; }
        [Column("DTYPE")]
        public int DTYPE { get; set; }
        [Column("INPUTDATE", TypeName = "DATE")]
        public DateTime? INPUTDATE { get; set; }
        //主建
        //[Key, Column("ITEMCODE", TypeName = "VARCHAR2")]
        //public string ITEMCODE { get; set; }
        //[Column("DLTIME")]
        //public int DLTIME { get; set; }
        //[Column("DTYPE")]
        //public int DTYPE { get; set; }
        /////<summary>
        /////创建时间 
        /////</summary> 
        //[Column("INPUTDATE", TypeName = "DATE")]
        //public DateTime? INPUTDATE { get; set; }
    }
}
