using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EF_ORACLE.Model
{
   
        [Table("SYSTUDEN")]  //指定数据库对应表名
        public class SYSTUDEN
        {
            [Key]
             [Column("ITEMCODE")] //指定数据库对应表栏位名称
            [MaxLength(64)]
            public string ITEMCODE { get; set; }

            /// <summary>
            /// 学生姓名
            /// </summary>
            [MaxLength(10)]
            [Column("OB")]
            public string OB { get; set; }
        } 
}
