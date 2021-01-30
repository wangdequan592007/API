using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace EF_ORACLE.Model
{
    [Table("DUAL")]  //指定数据库对应表名
    public class DUAL
    {
        [Key]
        // [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]//不自动增长
        [Column("DUMMY")] //指定数据库对应表栏位名称
        public string DUMMY { get; set; }
    }
}
