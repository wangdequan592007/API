using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES.Model
{
    [Table("USERTABLE")]  //指定数据库对应表名
    public class UserTable
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]//不自动增长
        [Column("USID")] //指定数据库对应表栏位名称
        public int USID { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        [MaxLength(50)]
        [Column("USNAME")]
        public string USNAME { get; set; }
    }
}
