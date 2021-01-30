using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EF_ORACLE.Model.TMES
{
    [Table("RE_BUG_TYPE")]
    public class RE_BUG_TYPE
    {

        [Key]
        [MaxLength(64)]
        [Column("ID")]
        public string ID { get; set; }
        [MaxLength(50)]
        [Column("CODE")]
        public string CODE { get; set; }
        [MaxLength(240)]
        [Column("NAME")]
        public string NAME { get; set; }
        [MaxLength(32)]
        [Column("BUG_TYPE")]
        public string BUG_TYPE { get; set; }
        [MaxLength(240)]
        [Column("DESCRIBE")]
        public string DESCRIBE { get; set; }
        [MaxLength(64)]
        [Column("CLASS")]
        public string CLASS { get; set; }
        [MaxLength(128)]
        [Column("TESTPOSITION")]
        public string TESTPOSITION { get; set; }
        [MaxLength(128)]
        [Column("WORKCODE")]
        public string WORKCODE { get; set; }
        [MaxLength(64)]
        [Column("REASONTYPE")]
        public string REASONTYPE { get; set; }
        [MaxLength(16)]
        [Column("CODE2")]
        public string CODE2 { get; set; }
        [MaxLength(240)]
        [Column("NAME2")]
        public string NAME2 { get; set; }
        [MaxLength(16)]
        [Column("ISHW")]
        public string ISHW { get; set; }

    }
}
